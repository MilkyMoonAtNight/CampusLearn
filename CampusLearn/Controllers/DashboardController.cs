using CampusLearn.Data;
using CampusLearn.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusLearn.Controllers
{
    // Razor page wrapper
    public class DashboardController : Controller
    {
        private readonly CampusLearnContext _db;
        public DashboardController(CampusLearnContext db) => _db = db;

        [HttpGet]
        public IActionResult Index()
        {
            var studentId = HttpContext.Session.GetInt32("LoggedInUserID");

            if (studentId == null)
            {
                // if session expired or user not logged in
                return RedirectToAction("Index", "LogIn");
            }

            ViewData["StudentId"] = studentId.Value;
            return View();
        }
    }

    // JSON API used by the view
    [ApiController]
    public class DashboardApiController : ControllerBase
    {
        private readonly CampusLearnContext _db;
        public DashboardApiController(CampusLearnContext db) => _db = db;

        // --- Cards ---
        // GET /api/dashboard/stats
        [HttpGet("/api/dashboard/stats")]
        public async Task<ActionResult<DashboardStatsDto>> GetStats(CancellationToken ct)
        {
            var studentId = HttpContext.Session.GetInt32("LoggedInUserID");
            if (studentId == null)
                return Unauthorized();

            var totalModules = await _db.Database.SqlQuery<int>($@"
                SELECT COUNT(DISTINCT dm.moduleid)
                FROM enrollment e
                JOIN enrollmentdegree ed ON ed.enrollmentid = e.enrollmentid
                JOIN degreemodule dm ON dm.degreeid = ed.degreeid
                WHERE e.studentid = {studentId};
            ").SingleAsync(ct);

            var activeEnrollments = await _db.Database.SqlQuery<int>($@"
                SELECT COUNT(*) FROM enrollment WHERE studentid = {studentId};
            ").SingleAsync(ct);

            var pendingAssignments = await _db.Database.SqlQuery<int>($@"
                SELECT COUNT(*)
                FROM moduleassignments a
                WHERE a.datedue >= CURRENT_DATE
                  AND a.moduleid IN (
                    SELECT dm.moduleid
                    FROM enrollment e
                    JOIN enrollmentdegree ed ON ed.enrollmentid = e.enrollmentid
                    JOIN degreemodule dm ON dm.degreeid = ed.degreeid
                    WHERE e.studentid = {studentId}
                  );
            ").SingleAsync(ct);

            var upcomingTests = await _db.Database.SqlQuery<int>($@"
                SELECT COUNT(*)
                FROM moduletests t
                WHERE t.testdate >= CURRENT_DATE
                  AND t.moduleid IN (
                    SELECT dm.moduleid
                    FROM enrollment e
                    JOIN enrollmentdegree ed ON ed.enrollmentid = e.enrollmentid
                    JOIN degreemodule dm ON dm.degreeid = ed.degreeid
                    WHERE e.studentid = {studentId}
                  );
            ").SingleAsync(ct);

            return new DashboardStatsDto
            {
                TotalModules = totalModules,
                ActiveEnrollments = activeEnrollments,
                PendingAssignments = pendingAssignments,
                UpcomingTests = upcomingTests
            };
        }

        // --- Announcements ---
        [HttpGet("/api/dashboard/announcements")]
        public async Task<ActionResult<IEnumerable<AnnouncementItemDto>>> GetAnnouncements(
            [FromQuery] int skip = 0, [FromQuery] int take = 5, CancellationToken ct = default)
        {
            var items = await _db.Database.SqlQuery<AnnouncementItemDto>($@"
                SELECT announcementid AS ""AnnouncementId"",
                       topic, discussion, progress, createdat
                FROM announcements
                ORDER BY createdat DESC
                OFFSET {skip} LIMIT {take};
            ").ToListAsync(ct);

            return items;
        }

        // --- Chart: Assignments per module ---
        [HttpGet("/api/dashboard/chart-assignments")]
        public async Task<ActionResult<object>> GetAssignmentsChart(CancellationToken ct = default)
        {
            var studentId = HttpContext.Session.GetInt32("LoggedInUserID");
            if (studentId == null)
                return Unauthorized();

            var rows = await _db.Database.SqlQuery<ModuleItemCountDto>($@"
                SELECT tm.modulename AS ""ModuleName"",
                       COUNT(ma.assignmentid) AS ""Items""
                FROM topicmodule tm
                LEFT JOIN moduleassignments ma ON ma.moduleid = tm.moduleid
                WHERE tm.moduleid IN (
                    SELECT dm.moduleid
                    FROM enrollment e
                    JOIN enrollmentdegree ed ON ed.enrollmentid = e.enrollmentid
                    JOIN degreemodule dm ON dm.degreeid = ed.degreeid
                    WHERE e.studentid = {studentId}
                )
                  AND (ma.datedue IS NULL OR ma.datedue >= CURRENT_DATE)
                GROUP BY tm.modulename
                ORDER BY tm.modulename;
            ").ToListAsync(ct);

            return new
            {
                labels = rows.Select(r => r.ModuleName).ToList(),
                series = rows.Select(r => r.Items).ToList()
            };
        }

        // --- Calendar ---
        [HttpGet("/api/dashboard/calendar")]
        public async Task<ActionResult<IEnumerable<CalendarEventDto>>> GetCalendar(
            [FromQuery] int year, [FromQuery] int month, CancellationToken ct = default)
        {
            var studentId = HttpContext.Session.GetInt32("LoggedInUserID");
            if (studentId == null)
                return Unauthorized();

            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1);

            var assignments = await _db.Database.SqlQuery<CalendarEventDto>($@"
                SELECT ma.datedue         AS ""EventDate"",
                       ma.assignmenttitle AS ""Title"",
                       tm.modulename      AS ""ModuleName"",
                       'Assignment'       AS ""Type""
                FROM moduleassignments ma
                JOIN topicmodule tm ON tm.moduleid = ma.moduleid
                WHERE ma.datedue >= {start} AND ma.datedue < {end}
                  AND ma.moduleid IN (
                      SELECT dm.moduleid
                      FROM enrollment e
                      JOIN enrollmentdegree ed ON ed.enrollmentid = e.enrollmentid
                      JOIN degreemodule dm    ON dm.degreeid = ed.degreeid
                      WHERE e.studentid = {studentId}
                  );
            ").ToListAsync(ct);

            var tests = await _db.Database.SqlQuery<CalendarEventDto>($@"
                SELECT t.testdate     AS ""EventDate"",
                       t.testtitle    AS ""Title"",
                       tm.modulename  AS ""ModuleName"",
                       'Test'         AS ""Type""
                FROM moduletests t
                JOIN topicmodule tm ON tm.moduleid = t.moduleid
                WHERE t.testdate >= {start} AND t.testdate < {end}
                  AND t.moduleid IN (
                      SELECT dm.moduleid
                      FROM enrollment e
                      JOIN enrollmentdegree ed ON ed.enrollmentid = e.enrollmentid
                      JOIN degreemodule dm    ON dm.degreeid = ed.degreeid
                      WHERE e.studentid = {studentId}
                  );
            ").ToListAsync(ct);

            return assignments.Concat(tests)
                              .OrderBy(e => e.EventDate)
                              .ToList();
        }

        // --- Students (for testing/demo data) ---
        [HttpGet("/api/dashboard/students")]
        public async Task<ActionResult<IEnumerable<StudentOptionDto>>> GetStudents(CancellationToken ct = default)
        {
            var rows = await _db.Database.SqlQuery<StudentOptionDto>($@"
                SELECT s.studentid AS ""StudentId"",
                       (COALESCE(s.firstname,'') || ' ' || COALESCE(s.lastname,'')) AS ""DisplayName""
                FROM student s
                WHERE EXISTS (SELECT 1 FROM enrollment e WHERE e.studentid = s.studentid)
                ORDER BY 2;
            ").ToListAsync(ct);

            return rows;
        }
    }
}
