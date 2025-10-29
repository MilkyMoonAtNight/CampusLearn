BEGIN;

-- =========================================================
-- CORE LOOKUPS
-- =========================================================
INSERT INTO Faculty (FacultyID, FacultyName) VALUES
  (1, 'Faculty of Engineering & Information Technology'),
  (2, 'Faculty of Humanities');

INSERT INTO Degree (DegreeID, DegreeName, FacultyID) VALUES
  (1, 'BSc Information Technology', 1),
  (2, 'BCom Information Systems',   1);

INSERT INTO ModuleCluster (ClusterID, ClusterName) VALUES
  (1, 'First-Year Core'),
  (2, 'Networking & Infrastructure'),
  (3, 'Data & Information Management');

INSERT INTO Speciality (SpecialityID, SpecialityName) VALUES
  (1, 'Computer Networks'),
  (2, 'Databases'),
  (3, 'Software Engineering');

INSERT INTO Tutors (TutorID, TutorName, TutorSurname, SpecialityID) VALUES
  (501, 'John',  'Smith',  1),
  (502, 'Jane',  'Doe',    2),
  (503, 'Ravi',  'Naidoo', 3);
-- Two modules: PRG381 and ENG281
INSERT INTO TopicModule (ModuleID, ModuleName, ClusterID, ModuleHeadID) VALUES
  (101, 'PRG381 - Programming III',            3, 503),
  (102, 'ENG281 - Computer Networks Fundamentals', 2, 501);

INSERT INTO DegreeModule (DegreeID, ModuleID) VALUES
  (1, 101),
  (1, 102),
  (2, 101);

INSERT INTO ModuleResource (ResourceID, ModuleID, ResourceType, ResourceURL) VALUES
  (1, 101, 'Syllabus', '/PDFs/PRG381/Syllabus.pdf'),
  (2, 101, 'Reading',  '/PDFs/PRG381/Week01/Reading-Pack.pdf'),
  (3, 102, 'Syllabus', '/PDFs/ENG281/Syllabus.pdf');

-- =========================================================
-- USERS & ENROLMENT
-- =========================================================
INSERT INTO Student (StudentID, FirstName, LastName, PersonalEmail, Phone, PasswordHash) VALUES
  (1001, 'Iwan',   'Botha', 'iwan@example.com',   '+27 71 000 0001', 'hash:iwan'),
  (1002, 'Pieter', 'Nel',   'pieter@example.com', '+27 72 000 0002', 'hash:pieter'),
  (1003, 'Aisha',  'Khan',  'aisha@example.com',  '+27 73 000 0003', 'hash:aisha');

-- Enroll (auto IDs) and link to degrees without guessing IDs
WITH e AS (
  INSERT INTO Enrollment (StudentID, EnrollmentDate)
  VALUES
    (1001, NOW() - INTERVAL '30 days'),
    (1002, NOW() - INTERVAL '25 days')
  RETURNING enrollmentid, studentid
)
INSERT INTO EnrollmentDegree (EnrollmentID, DegreeID)
SELECT e.enrollmentid,
       CASE e.studentid
         WHEN 1001 THEN 1
         WHEN 1002 THEN 1
       END AS degreeid
FROM e;

-- =========================================================
-- ADMINS
-- =========================================================
INSERT INTO Admin (AdminID, AdminName, AdminSurname, AdminPhone, AdminEmail, AdminPasswordHash, IsSuperAdmin)
VALUES
  (1, 'Campus', 'Admin', '+27 11 555 0000', 'admin@campuslearn.example', 'hash:admin', TRUE);

-- =========================================================
-- SESSIONS & RATINGS
-- =========================================================
INSERT INTO Session (SessionID, SessionTopic) VALUES
  (3001, 'Intro to University Systems'),
  (3002, 'Exam Prep Workshop');

INSERT INTO Rating (RatingID, RatingValue) VALUES
  (1, 1),(2, 2),(3, 3),(4, 4),(5, 5);

INSERT INTO SessionRating (SessionID, RatingID) VALUES
  (3001, 5),
  (3002, 4);

INSERT INTO SessionStudent (SessionID, StudentID) VALUES
  (3001, 1001),
  (3001, 1002),
  (3002, 1001);

INSERT INTO SessionTutor (SessionID, TutorID) VALUES
  (3001, 502), -- Jane
  (3002, 501); -- John

INSERT INTO StudentTutor (StudentID, TutorID) VALUES
  (1001, 502),
  (1002, 501);

-- =========================================================
-- FORUMS
-- =========================================================
INSERT INTO ForumTopic (Id, Title, Subject, Description, Contributions, Progress, CreatedAt) VALUES
  (1, 'How to access PDFs', 'PRG381', 'Where are weekly PDFs located?', 0, 'Fresh', NOW()),
  (2, 'Subnetting tips',    'ENG281', 'Share good resources for subnetting.', 0, 'Fresh', NOW());

INSERT INTO Reply (ReplyID, ForumTopicId, Author, Message, PostedAt) VALUES
  (1, 1, 'Jane Doe',  'They''re under /PDFs/PRG381/WeekXX on the site.', NOW()),
  (2, 2, 'John Smith','Use VLSM and practice with online calculators.', NOW());

-- =========================================================
-- ANNOUNCEMENTS
-- =========================================================
INSERT INTO Announcements (AnnouncementID, Topic, Discussion, Progress, CreatedAt, AdminID) VALUES
  (1, 'Welcome to Semester', 'Please read your course outlines.', 'Important', NOW(), 1),
  (2, 'Lab Downtime', 'Networking lab will be offline Saturday 10:00–12:00.', 'Pending', NOW(), 1);

INSERT INTO ReplyAnnouncements (ReplyID, AnnouncementID, ReplyText, PostedAt) VALUES
  (1, 1, 'Thanks for the heads-up!', NOW()),
  (2, 2, 'Will the Wi-Fi be affected?', NOW());

-- =========================================================
-- CHAT SYSTEM
-- =========================================================
INSERT INTO ChatSession (ChatSessionID, StudentID, StartedAt, Topic)
VALUES (4001, 1001, NOW() - INTERVAL '1 day', 'Help with PRG381 Week 1');

INSERT INTO ChatMessages (ChatMessageID, ChatSessionID, MessageText, SentAt, SenderStudentID, ReceiverTutorID)
VALUES (1, 4001, 'Hi, where do I find Week 1 slides?', NOW() - INTERVAL '23 hours', 1001, 502);

INSERT INTO ChatMessages (ChatMessageID, ChatSessionID, MessageText, SentAt, SenderTutorID, ReceiverStudentID)
VALUES (2, 4001, 'Check /PDFs/PRG381/Week01/Intro.pdf 👍', NOW() - INTERVAL '22 hours', 502, 1001);

-- =========================================================
-- MODULE PLANNING, WEEKS & CONTENT
-- =========================================================
INSERT INTO ModulePlan (ModuleID, TotalWeeks, TestsAllowed, AssignmentsRequired, ProjectsRequired) VALUES
  (101, 12, 3, 2, 1),
  (102, 12, 2, 1, 1);

-- Weeks (first 3 weeks for each)
INSERT INTO ModuleWeek (WeekID, ModuleID, WeekNumber) VALUES
  (10101, 101, 1),
  (10102, 101, 2),
  (10103, 101, 3),
  (10201, 102, 1),
  (10202, 102, 2),
  (10203, 102, 3);

-- Weekly content uses /PDFs/... paths served by your app
INSERT INTO WeekContent (ContentID, WeekID, ContentTitle, PdfURL, PdfMime, PdfSizeBytes) VALUES
  (1, 10101, 'PRG381 Week 1 Intro',      '/PDFs/PRG381/Week01/Intro.pdf',        'application/pdf', 420000),
  (2, 10101, 'PRG381 Week 1 Reading',    '/PDFs/PRG381/Week01/Reading-Pack.pdf', 'application/pdf', 850000),
  (3, 10102, 'PRG381 Week 2 OOP Basics', '/PDFs/PRG381/Week02/OOP-Basics.pdf',   'application/pdf', 620000),
  (4, 10103, 'PRG381 Week 3 Patterns',   '/PDFs/PRG381/Week03/Patterns.pdf',     'application/pdf', 540000),
  (5, 10201, 'ENG281 Week 1 OSI',        '/PDFs/ENG281/Week01/OSI-Overview.pdf', 'application/pdf', 380000),
  (6, 10202, 'ENG281 Week 2 Subnetting', '/PDFs/ENG281/Week02/Subnetting-1.pdf', 'application/pdf', 500000),
  (7, 10203, 'ENG281 Week 3 VLANs',      '/PDFs/ENG281/Week03/VLANs.pdf',        'application/pdf', 460000);

-- Assignments (specs via URL; uploads left NULL)
INSERT INTO ModuleAssignments (ModuleID, AssignmentTitle, AssignmentPdfURL, DateDue) VALUES
  (101, 'PRG381 Assignment 1: ERD',           '/PDFs/PRG381/Assignments/ERD_Assignment.pdf',            CURRENT_DATE + INTERVAL '14 days'),
  (101, 'PRG381 Assignment 2: Normalization', '/PDFs/PRG381/Assignments/Normalization_Assignment.pdf',  CURRENT_DATE + INTERVAL '28 days'),
  (102, 'ENG281 Assignment 1: Subnetting',    '/PDFs/ENG281/Assignments/Subnetting_Assignment.pdf',     CURRENT_DATE + INTERVAL '21 days');

-- Projects
INSERT INTO ModuleProjects (ModuleID, ProjectTitle, ProjectPdfURL, DateDue) VALUES
  (101, 'PRG381 Project: Mini-IS Case Study', '/PDFs/PRG381/Projects/IS_Case_Study.pdf',          CURRENT_DATE + INTERVAL '45 days'),
  (102, 'ENG281 Project: Small Office Network','/PDFs/ENG281/Projects/Small_Office_Network.pdf',  CURRENT_DATE + INTERVAL '50 days');

-- Tests (ensure weeks exist and within TotalWeeks)
INSERT INTO ModuleTests (ModuleID, TestTitle, TestWeek, TestDate) VALUES
  (101, 'PRG381 Quiz 1', 4, CURRENT_DATE + INTERVAL '20 days'),
  (101, 'PRG381 Midterm', 8, CURRENT_DATE + INTERVAL '50 days'),
  (102, 'ENG281 Quiz 1', 3, CURRENT_DATE + INTERVAL '15 days');

-- =========================================================
-- SEQUENCE BUMPS (lowercase identifiers; skip non-serial tables)
-- =========================================================
SELECT setval(pg_get_serial_sequence('faculty','facultyid'),
              COALESCE((SELECT MAX(facultyid) FROM faculty), 1), true);
SELECT setval(pg_get_serial_sequence('degree','degreeid'),
              COALESCE((SELECT MAX(degreeid) FROM degree), 1), true);
SELECT setval(pg_get_serial_sequence('modulecluster','clusterid'),
              COALESCE((SELECT MAX(clusterid) FROM modulecluster), 1), true);
SELECT setval(pg_get_serial_sequence('speciality','specialityid'),
              COALESCE((SELECT MAX(specialityid) FROM speciality), 1), true);
SELECT setval(pg_get_serial_sequence('tutors','tutorid'),
              COALESCE((SELECT MAX(tutorid) FROM tutors), 1), true);
SELECT setval(pg_get_serial_sequence('topicmodule','moduleid'),
              COALESCE((SELECT MAX(moduleid) FROM topicmodule), 1), true);
SELECT setval(pg_get_serial_sequence('moduleresource','resourceid'),
              COALESCE((SELECT MAX(resourceid) FROM moduleresource), 1), true);

SELECT setval(pg_get_serial_sequence('student','studentid'),
              COALESCE((SELECT MAX(studentid) FROM student), 1), true);
SELECT setval(pg_get_serial_sequence('enrollment','enrollmentid'),
              COALESCE((SELECT MAX(enrollmentid) FROM enrollment), 1), true);

SELECT setval(pg_get_serial_sequence('admin','adminid'),
              COALESCE((SELECT MAX(adminid) FROM admin), 1), true);

SELECT setval(pg_get_serial_sequence('session','sessionid'),
              COALESCE((SELECT MAX(sessionid) FROM session), 1), true);
SELECT setval(pg_get_serial_sequence('rating','ratingid'),
              COALESCE((SELECT MAX(ratingid) FROM rating), 1), true);

SELECT setval(pg_get_serial_sequence('forumtopic','id'),
              COALESCE((SELECT MAX(id) FROM forumtopic), 1), true);
SELECT setval(pg_get_serial_sequence('reply','replyid'),
              COALESCE((SELECT MAX(replyid) FROM reply), 1), true);

SELECT setval(pg_get_serial_sequence('announcements','announcementid'),
              COALESCE((SELECT MAX(announcementid) FROM announcements), 1), true);
SELECT setval(pg_get_serial_sequence('replyannouncements','replyid'),
              COALESCE((SELECT MAX(replyid) FROM replyannouncements), 1), true);

SELECT setval(pg_get_serial_sequence('chatsession','chatsessionid'),
              COALESCE((SELECT MAX(chatsessionid) FROM chatsession), 1), true);
SELECT setval(pg_get_serial_sequence('chatmessages','chatmessageid'),
              COALESCE((SELECT MAX(chatmessageid) FROM chatmessages), 1), true);

SELECT setval(pg_get_serial_sequence('moduleweek','weekid'),
              COALESCE((SELECT MAX(weekid) FROM moduleweek), 1), true);
SELECT setval(pg_get_serial_sequence('weekcontent','contentid'),
              COALESCE((SELECT MAX(contentid) FROM weekcontent), 1), true);
SELECT setval(pg_get_serial_sequence('moduleassignments','assignmentid'),
              COALESCE((SELECT MAX(assignmentid) FROM moduleassignments), 1), true);
SELECT setval(pg_get_serial_sequence('moduleprojects','projectid'),
              COALESCE((SELECT MAX(projectid) FROM moduleprojects), 1), true);
SELECT setval(pg_get_serial_sequence('moduletests','testid'),
              COALESCE((SELECT MAX(testid) FROM moduletests), 1), true);

COMMIT;
