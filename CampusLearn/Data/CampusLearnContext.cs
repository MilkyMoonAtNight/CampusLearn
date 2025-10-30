using CampusLearn.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusLearn.Data
{
    public class CampusLearnContext : DbContext
    {
        public CampusLearnContext(DbContextOptions<CampusLearnContext> options) : base(options) { }

        // Core
        public DbSet<Student> Students { get; set; }
        public DbSet<Tutor> Tutors { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<TopicModule> Modules { get; set; }
        public DbSet<ModuleCluster> ModuleClusters { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<ModuleResource> ModuleResources { get; set; }

        // Users & Enrolment
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<EnrollmentDegree> EnrollmentDegrees { get; set; }

        // Admins
        public DbSet<Admin> Admins { get; set; }

        // Forum
        public DbSet<ForumTopic> ForumTopics { get; set; }
        public DbSet<Reply> Replies { get; set; }

        // Announcements
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<ReplyAnnouncement> ReplyAnnouncements { get; set; }

        // Sessions & Ratings
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<SessionRating> SessionRatings { get; set; }
        public DbSet<SessionTutor> SessionTutors { get; set; }
        public DbSet<SessionStudent> SessionStudents { get; set; }
        public DbSet<StudentTutor> StudentTutors { get; set; }

        // Chat
        public DbSet<ChatSession> ChatSessions { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        // Module planning
        public DbSet<ModulePlan> ModulePlans { get; set; }
        public DbSet<ModuleWeek> ModuleWeeks { get; set; }
        public DbSet<WeekContent> WeekContents { get; set; }
        public DbSet<ModuleAssignment> ModuleAssignments { get; set; }
        public DbSet<ModuleProject> ModuleProjects { get; set; }
        public DbSet<ModuleTest> ModuleTests { get; set; }

        // (You had MessageUser; keep if you actually use it)
        public DbSet<MessageUser> MessageUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // Core entities
            // =========================
            modelBuilder.Entity<Student>(e =>
            {
                e.ToTable("student");
                e.HasKey(x => x.StudentID);
                e.Property(x => x.StudentID).HasColumnName("studentid");
                e.Property(x => x.FirstName).HasColumnName("firstname");
                e.Property(x => x.LastName).HasColumnName("lastname");
                e.Property(x => x.PersonalEmail).HasColumnName("personalemail");
                e.Property(x => x.Phone).HasColumnName("phone");
                e.Property(x => x.PasswordHash).HasColumnName("passwordhash");
            });

            modelBuilder.Entity<Tutor>(e =>
            {
                e.ToTable("tutors");
                e.HasKey(x => x.TutorID);
                e.Property(x => x.TutorID).HasColumnName("tutorid");
                e.Property(x => x.TutorName).HasColumnName("tutorname");
                e.Property(x => x.TutorSurname).HasColumnName("tutorsurname");
                e.Property(x => x.SpecialityID).HasColumnName("specialityid");

                e.HasOne(x => x.Speciality)
                 .WithMany(x => x.Tutors)
                 .HasForeignKey(x => x.SpecialityID);
            });

            modelBuilder.Entity<Faculty>(e =>
            {
                e.ToTable("faculty");
                e.HasKey(x => x.FacultyID);
                e.Property(x => x.FacultyID).HasColumnName("facultyid");
                e.Property(x => x.FacultyName).HasColumnName("facultyname");
            });

            modelBuilder.Entity<Degree>(e =>
            {
                e.ToTable("degree");
                e.HasKey(x => x.DegreeID);
                e.Property(x => x.DegreeID).HasColumnName("degreeid");
                e.Property(x => x.DegreeName).HasColumnName("degreename");
                e.Property(x => x.FacultyID).HasColumnName("facultyid");
                e.HasOne<Faculty>()
                 .WithMany()
                 .HasForeignKey(x => x.FacultyID);
            });

            modelBuilder.Entity<ModuleCluster>(e =>
            {
                e.ToTable("modulecluster");
                e.HasKey(x => x.ClusterID);
                e.Property(x => x.ClusterID).HasColumnName("clusterid");
                e.Property(x => x.ClusterName).HasColumnName("clustername");
            });

            modelBuilder.Entity<Speciality>(e =>
            {
                e.ToTable("speciality");
                e.HasKey(x => x.SpecialityID);
                e.Property(x => x.SpecialityID).HasColumnName("specialityid");
                e.Property(x => x.SpecialityName).HasColumnName("specialityname");
            });

            modelBuilder.Entity<TopicModule>(e =>
            {
                e.ToTable("topicmodule");
                e.HasKey(x => x.ModuleID);
                e.Property(x => x.ModuleID).HasColumnName("moduleid");
                e.Property(x => x.ModuleName).HasColumnName("modulename");
                e.Property(x => x.ClusterID).HasColumnName("clusterid");
                e.Property(x => x.ModuleHeadID).HasColumnName("moduleheadid");

                e.HasOne(x => x.ModuleCluster)
                 .WithMany(x => x.TopicModules)
                 .HasForeignKey(x => x.ClusterID);

                e.HasOne(x => x.ModuleHead)
                 .WithMany(x => x.TopicModules)
                 .HasForeignKey(x => x.ModuleHeadID)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<ModuleResource>(e =>
            {
                e.ToTable("moduleresource");
                e.HasKey(x => x.ResourceID);
                e.Property(x => x.ResourceID).HasColumnName("resourceid");
                e.Property(x => x.ModuleID).HasColumnName("moduleid");
                e.Property(x => x.ResourceType).HasColumnName("resourcetype");
                e.Property(x => x.ResourceURL).HasColumnName("resourceurl");

                e.HasOne(x => x.Module)
                 .WithMany(x => x.ModuleResources)
                 .HasForeignKey(x => x.ModuleID);
            });

            // =========================
            // Users & Enrolment
            // =========================
            modelBuilder.Entity<Enrollment>(e =>
            {
                e.ToTable("enrollment");
                e.HasKey(x => x.EnrollmentID);
                e.Property(x => x.EnrollmentID).HasColumnName("enrollmentid");
                e.Property(x => x.StudentID).HasColumnName("studentid");
                e.Property(x => x.EnrollmentDate).HasColumnName("enrollmentdate");

                e.HasOne(x => x.Student)
                 .WithMany(x => x.Enrollments)
                 .HasForeignKey(x => x.StudentID);
            });

            modelBuilder.Entity<EnrollmentDegree>(e =>
            {
                e.ToTable("enrollmentdegree");
                e.HasKey(x => new { x.EnrollmentID, x.DegreeID });
                e.Property(x => x.EnrollmentID).HasColumnName("enrollmentid");
                e.Property(x => x.DegreeID).HasColumnName("degreeid");

                e.HasOne(x => x.Enrollment)
                 .WithMany(x => x.EnrollmentDegrees)
                 .HasForeignKey(x => x.EnrollmentID);

                e.HasOne(x => x.Degree)
                 .WithMany(x => x.EnrollmentDegrees)
                 .HasForeignKey(x => x.DegreeID);
            });

            // =========================
            // Admins
            // =========================
            modelBuilder.Entity<Admin>(e =>
            {
                e.ToTable("admin");
                e.HasKey(x => x.AdminID);
                e.Property(x => x.AdminID).HasColumnName("adminid");
                e.Property(x => x.AdminName).HasColumnName("adminname");
                e.Property(x => x.AdminSurname).HasColumnName("adminsurname");
                e.Property(x => x.AdminPhone).HasColumnName("adminphone");
                e.Property(x => x.AdminEmail).HasColumnName("adminemail");
                e.Property(x => x.AdminPasswordHash).HasColumnName("adminpasswordhash");
                e.Property(x => x.CreatedAt).HasColumnName("createdat");
                e.Property(x => x.IsSuperAdmin).HasColumnName("issuperadmin");
            });

            // =========================
            // Forums
            // =========================
            modelBuilder.Entity<ForumTopic>(e =>
            {
                e.ToTable("forumtopic");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.Title).HasColumnName("title");
                e.Property(x => x.Subject).HasColumnName("subject");
                e.Property(x => x.Description).HasColumnName("description");
                e.Property(x => x.Contributions).HasColumnName("contributions");
                e.Property(x => x.Progress).HasColumnName("progress");
                e.Property(x => x.CreatedAt).HasColumnName("createdat");
            });

            modelBuilder.Entity<Reply>(e =>
            {
                e.ToTable("reply");
                e.HasKey(x => x.ReplyID);
                e.Property(x => x.ReplyID).HasColumnName("replyid");
                e.Property(x => x.ForumTopicId).HasColumnName("forumtopicid");
                e.Property(x => x.Author).HasColumnName("author");
                e.Property(x => x.Message).HasColumnName("message");
                e.Property(x => x.PostedAt).HasColumnName("postedat");

                e.HasOne(x => x.ForumTopic)
                 .WithMany(x => x.Replies)
                 .HasForeignKey(x => x.ForumTopicId);
            });

            // =========================
            // Announcements
            // =========================
            modelBuilder.Entity<Announcement>(e =>
            {
                e.ToTable("announcements");
                e.HasKey(x => x.AnnouncementID);

                e.Property(x => x.AnnouncementID).HasColumnName("announcementid");
                e.Property(x => x.Topic)
                    .HasColumnName("topic")
                    .IsRequired()
                    .HasMaxLength(255);

                e.Property(x => x.Discussion)
                    .HasColumnName("discussion")
                    .IsRequired();

                e.Property(x => x.Progress)
                    .HasColumnName("progress")
                    .HasMaxLength(20);

                e.Property(x => x.CreatedAt)
                    .HasColumnName("createdat")
                    .HasDefaultValueSql("now()");

                e.Property(x => x.AdminID).HasColumnName("adminid");

                // Only keep this if your entity has `public ICollection<ReplyAnnouncement> Replies { get; set; }`
                e.HasMany(a => a.Replies)
                 .WithOne(r => r.Announcement)
                 .HasForeignKey(r => r.AnnouncementID)
                 .OnDelete(DeleteBehavior.NoAction); // matches existing FK (no cascade)
            });

            // ReplyAnnouncements
            modelBuilder.Entity<ReplyAnnouncement>(e =>
            {
                e.ToTable("replyannouncements");
                e.HasKey(x => x.ReplyID);

                e.Property(x => x.ReplyID).HasColumnName("replyid");
                e.Property(x => x.AnnouncementID).HasColumnName("announcementid");

                e.Property(x => x.ReplyText)
                    .HasColumnName("replytext")
                    .IsRequired();

                e.Property(x => x.PostedAt)
                    .HasColumnName("postedat")
                    .HasDefaultValueSql("now()");

                // Only keep this if your entity has `public Announcement? Announcement { get; set; }`
                e.HasOne(x => x.Announcement)
                 .WithMany(x => x.Replies)
                 .HasForeignKey(x => x.AnnouncementID)
                 .OnDelete(DeleteBehavior.NoAction); // match DB
            });

            // =========================
            // Sessions & Ratings
            // =========================
            modelBuilder.Entity<Session>(e =>
            {
                e.ToTable("session");
                e.HasKey(x => x.SessionID);
                e.Property(x => x.SessionID).HasColumnName("sessionid");
                e.Property(x => x.SessionTopic).HasColumnName("sessiontopic");
            });

            modelBuilder.Entity<Rating>(e =>
            {
                e.ToTable("rating");
                e.HasKey(x => x.RatingID);
                e.Property(x => x.RatingID).HasColumnName("ratingid");
                e.Property(x => x.RatingValue).HasColumnName("ratingvalue");
            });

            modelBuilder.Entity<SessionRating>(e =>
            {
                e.ToTable("sessionrating");
                e.HasKey(x => new { x.SessionID, x.RatingID });
                e.Property(x => x.SessionID).HasColumnName("sessionid");
                e.Property(x => x.RatingID).HasColumnName("ratingid");

                e.HasOne(x => x.Session)
                 .WithMany(x => x.SessionRatings)
                 .HasForeignKey(x => x.SessionID);

                e.HasOne(x => x.Rating)
                 .WithMany(x => x.SessionRatings)
                 .HasForeignKey(x => x.RatingID);
            });

            modelBuilder.Entity<SessionTutor>(e =>
            {
                e.ToTable("sessiontutor");
                e.HasKey(x => new { x.SessionID, x.TutorID });
                e.Property(x => x.SessionID).HasColumnName("sessionid");
                e.Property(x => x.TutorID).HasColumnName("tutorid");

                e.HasOne(x => x.Session)
                 .WithMany(x => x.SessionTutors)
                 .HasForeignKey(x => x.SessionID);

                e.HasOne(x => x.Tutor)
                 .WithMany(x => x.SessionTutors)
                 .HasForeignKey(x => x.TutorID);
            });

            modelBuilder.Entity<SessionStudent>(e =>
            {
                e.ToTable("sessionstudent");
                e.HasKey(x => new { x.SessionID, x.StudentID });
                e.Property(x => x.SessionID).HasColumnName("sessionid");
                e.Property(x => x.StudentID).HasColumnName("studentid");

                e.HasOne(x => x.Session)
                 .WithMany(x => x.SessionStudents)
                 .HasForeignKey(x => x.SessionID);

                e.HasOne(x => x.Student)
                 .WithMany(x => x.SessionStudents)
                 .HasForeignKey(x => x.StudentID);
            });

            modelBuilder.Entity<StudentTutor>(e =>
            {
                e.ToTable("studenttutor");
                e.HasKey(x => new { x.StudentID, x.TutorID });
                e.Property(x => x.StudentID).HasColumnName("studentid");
                e.Property(x => x.TutorID).HasColumnName("tutorid");

                e.HasOne(x => x.Student)
                 .WithMany(x => x.StudentTutors)
                 .HasForeignKey(x => x.StudentID);

                e.HasOne(x => x.Tutor)
                 .WithMany(x => x.StudentTutors)
                 .HasForeignKey(x => x.TutorID);
            });

            // =========================
            // Chat
            // =========================
            modelBuilder.Entity<ChatSession>(e =>
            {
                e.ToTable("chatsession");
                e.HasKey(x => x.ChatSessionID);
                e.Property(x => x.ChatSessionID).HasColumnName("chatsessionid");
                e.Property(x => x.StudentID).HasColumnName("studentid");
                e.Property(x => x.Topic).HasColumnName("topic");
                e.Property(x => x.StartedAt).HasColumnName("startedat");
                e.Property(x => x.EndedAt).HasColumnName("endedat");

                e.HasOne(x => x.Student)
                 .WithMany(x => x.ChatSessions)
                 .HasForeignKey(x => x.StudentID);
            });

            modelBuilder.Entity<ChatMessage>(e =>
            {
                e.ToTable("chatmessages");
                e.HasKey(x => x.ChatMessageID);

                e.Property(x => x.ChatMessageID).HasColumnName("chatmessageid");
                e.Property(x => x.ChatSessionID).HasColumnName("chatsessionid");
                e.Property(x => x.MessageText).HasColumnName("messagetext");
                e.Property(x => x.SentAt).HasColumnName("sentat");

                // Nullable FKs (match DB NULLs)
                e.Property(x => x.SenderStudentID).HasColumnName("senderstudentid");
                e.Property(x => x.SenderTutorID).HasColumnName("sendertutorid");
                e.Property(x => x.ReceiverStudentID).HasColumnName("receiverstudentid");
                e.Property(x => x.ReceiverTutorID).HasColumnName("receivertutorid");

                // ChatSession → ChatMessages (required)
                e.HasOne(x => x.ChatSession)
                 .WithMany(x => x.Messages)        // or .WithMany() if ChatSession has no nav
                 .HasForeignKey(x => x.ChatSessionID)
                 .OnDelete(DeleteBehavior.Cascade);

                // Sender: Student (optional)
                e.HasOne(x => x.SenderStudent)
                 .WithMany()                        // no back-collection; avoids ambiguity
                 .HasForeignKey(x => x.SenderStudentID)
                 .OnDelete(DeleteBehavior.Restrict) // prevent cascade cycles
                 .IsRequired(false);

                // Sender: Tutor (optional)
                e.HasOne(x => x.SenderTutor)
                 .WithMany()
                 .HasForeignKey(x => x.SenderTutorID)
                 .OnDelete(DeleteBehavior.Restrict)
                 .IsRequired(false);

                // Receiver: Student (optional)
                e.HasOne(x => x.ReceiverStudent)
                 .WithMany()
                 .HasForeignKey(x => x.ReceiverStudentID)
                 .OnDelete(DeleteBehavior.Restrict)
                 .IsRequired(false);

                // Receiver: Tutor (optional)
                e.HasOne(x => x.ReceiverTutor)
                 .WithMany()
                 .HasForeignKey(x => x.ReceiverTutorID)
                 .OnDelete(DeleteBehavior.Restrict)
                 .IsRequired(false);
            });


            // =========================
            // Module planning
            // =========================
            modelBuilder.Entity<ModulePlan>(e =>
            {
                e.ToTable("moduleplan");
                e.HasKey(x => x.ModuleID);
                e.Property(x => x.ModuleID).HasColumnName("moduleid");
                e.Property(x => x.TotalWeeks).HasColumnName("totalweeks");
                e.Property(x => x.TestsAllowed).HasColumnName("testsallowed");
                e.Property(x => x.AssignmentsRequired).HasColumnName("assignmentsrequired");
                e.Property(x => x.ProjectsRequired).HasColumnName("projectsrequired");

                e.HasOne<TopicModule>(p => p.Module)
                 .WithOne(m => m.ModulePlan)
                 .HasForeignKey<ModulePlan>(p=> p.ModuleID)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ModuleWeek>(e =>
            {
                e.ToTable("moduleweek");
                e.HasKey(x => x.WeekID);
                e.Property(x => x.WeekID).HasColumnName("weekid");
                e.Property(x => x.ModuleID).HasColumnName("moduleid");
                e.Property(x => x.WeekNumber).HasColumnName("weeknumber");

                e.HasOne(x => x.Module)
                 .WithMany(x => x.ModuleWeeks)
                 .HasForeignKey(x => x.ModuleID);

                e.HasIndex(x => new { x.ModuleID, x.WeekNumber }).IsUnique();
            });

            modelBuilder.Entity<WeekContent>(e =>
            {
                e.ToTable("weekcontent");
                e.HasKey(x => x.ContentID);
                e.Property(x => x.ContentID).HasColumnName("contentid");
                e.Property(x => x.WeekID).HasColumnName("weekid");
                e.Property(x => x.ContentTitle).HasColumnName("contenttitle");
                e.Property(x => x.PdfURL).HasColumnName("pdfurl");
                e.Property(x => x.PdfFile).HasColumnName("pdffile");
                e.Property(x => x.PdfMime).HasColumnName("pdfmime");
                e.Property(x => x.PdfSizeBytes).HasColumnName("pdfsizebytes");

                e.HasOne(x => x.ModuleWeek)
                 .WithMany(x => x.Contents)
                 .HasForeignKey(x => x.WeekID);
            });

            modelBuilder.Entity<ModuleAssignment>(e =>
            {
                e.ToTable("moduleassignments");
                e.HasKey(x => x.AssignmentID);
                e.Property(x => x.AssignmentID).HasColumnName("assignmentid");
                e.Property(x => x.ModuleID).HasColumnName("moduleid");
                e.Property(x => x.AssignmentTitle).HasColumnName("assignmenttitle");
                e.Property(x => x.AssignmentPdfURL).HasColumnName("assignmentpdfurl");
                e.Property(x => x.AssignmentPdf).HasColumnName("assignmentpdf");
                e.Property(x => x.AssignmentPdfMime).HasColumnName("assignmentpdfmime");
                e.Property(x => x.AssignmentPdfSizeBytes).HasColumnName("assignmentpdfsizebytes");
                e.Property(x => x.DateDue).HasColumnName("datedue");
                e.Property(x => x.AssignmentUploadPdfURL).HasColumnName("assignmentuploadpdfurl");
                e.Property(x => x.AssignmentUploadPdf).HasColumnName("assignmentuploadpdf");
                e.Property(x => x.AssignmentUploadPdfMime).HasColumnName("assignmentuploadpdfmime");
                e.Property(x => x.AssignmentUploadPdfSizeBytes).HasColumnName("assignmentuploadpdfsizebytes");

                e.HasOne(x => x.Module)
                 .WithMany(x => x.ModuleAssignments)
                 .HasForeignKey(x => x.ModuleID);
            });

            modelBuilder.Entity<ModuleProject>(e =>
            {
                e.ToTable("moduleprojects");
                e.HasKey(x => x.ProjectID);
                e.Property(x => x.ProjectID).HasColumnName("projectid");
                e.Property(x => x.ModuleID).HasColumnName("moduleid");
                e.Property(x => x.ProjectTitle).HasColumnName("projecttitle");
                e.Property(x => x.ProjectPdfURL).HasColumnName("projectpdfurl");
                e.Property(x => x.ProjectPdf).HasColumnName("projectpdf");
                e.Property(x => x.ProjectPdfMime).HasColumnName("projectpdfmime");
                e.Property(x => x.ProjectPdfSizeBytes).HasColumnName("projectpdfsizebytes");
                e.Property(x => x.DateDue).HasColumnName("datedue");
                e.Property(x => x.UploadPdfURL).HasColumnName("uploadpdfurl");
                e.Property(x => x.UploadPdf).HasColumnName("uploadpdf");
                e.Property(x => x.UploadPdfMime).HasColumnName("uploadpdfmime");
                e.Property(x => x.UploadPdfSizeBytes).HasColumnName("uploadpdfsizebytes");

                e.HasOne(x => x.Module)
                 .WithMany(x => x.ModuleProjects)
                 .HasForeignKey(x => x.ModuleID);
            });

            modelBuilder.Entity<ModuleTest>(e =>
            {
                e.ToTable("moduletests");
                e.HasKey(x => x.TestID);
                e.Property(x => x.TestID).HasColumnName("testid");
                e.Property(x => x.ModuleID).HasColumnName("moduleid");
                e.Property(x => x.TestTitle).HasColumnName("testtitle");
                e.Property(x => x.TestWeek).HasColumnName("testweek");
                e.Property(x => x.TestDate).HasColumnName("testdate");

                e.HasOne(x => x.Module)
                 .WithMany(x => x.ModuleTests)
                 .HasForeignKey(x => x.ModuleID);

                e.HasIndex(x => new { x.ModuleID, x.TestWeek }).IsUnique();
            });

            // =========================
            // Degree ↔ Module (join)
            // =========================
            modelBuilder.Entity<DegreeModule>(e =>
            {
                e.ToTable("degreemodule");
                e.HasKey(x => new { x.DegreeID, x.ModuleID });
                e.Property(x => x.DegreeID).HasColumnName("degreeid");
                e.Property(x => x.ModuleID).HasColumnName("moduleid");

                e.HasOne(x => x.Degree)
                 .WithMany(x => x.DegreeModules)
                 .HasForeignKey(x => x.DegreeID);

                e.HasOne(x => x.Module)
                 .WithMany(x => x.DegreeModules)
                 .HasForeignKey(x => x.ModuleID);
            });

            modelBuilder.Entity<MessageUser>(e =>
            {
                e.ToTable("messageuser");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
            });

            modelBuilder.Entity<CampusLearn.ViewModels.ModuleItemCountDto>().HasNoKey();
            modelBuilder.Entity<CampusLearn.ViewModels.DashboardStatsDto>().HasNoKey();
            modelBuilder.Entity<CampusLearn.ViewModels.AnnouncementItemDto>().HasNoKey();
            modelBuilder.Entity<CampusLearn.ViewModels.CalendarEventDto>().HasNoKey();
            modelBuilder.Entity<CampusLearn.ViewModels.StudentOptionDto>().HasNoKey();


    }
}
}
