
-- =========================================================
-- CORE LOOKUPS
-- =========================================================
CREATE TABLE Faculty (
    FacultyID SERIAL PRIMARY KEY,
    FacultyName VARCHAR(120) NOT NULL
);

CREATE TABLE Degree (
    DegreeID SERIAL PRIMARY KEY,
    DegreeName VARCHAR(255) NOT NULL,
    FacultyID INT NOT NULL REFERENCES Faculty(FacultyID)
);

CREATE TABLE ModuleCluster (
    ClusterID SERIAL PRIMARY KEY,
    ClusterName VARCHAR(100) NOT NULL
);

CREATE TABLE Speciality (
    SpecialityID SERIAL PRIMARY KEY,
    SpecialityName VARCHAR(100) NOT NULL
);

CREATE TABLE Tutors (
    TutorID BIGSERIAL PRIMARY KEY,
    TutorName VARCHAR(255) NOT NULL,
    TutorSurname VARCHAR(255) NOT NULL,
    SpecialityID INT REFERENCES Speciality(SpecialityID)
);

CREATE TABLE TopicModule (
    ModuleID SERIAL PRIMARY KEY,
    ModuleName VARCHAR(255) NOT NULL,
    ClusterID INT REFERENCES ModuleCluster(ClusterID),
    ModuleHeadID BIGINT REFERENCES Tutors(TutorID)
);

CREATE TABLE DegreeModule (
    DegreeID INT NOT NULL,
    ModuleID INT NOT NULL,
    PRIMARY KEY (DegreeID, ModuleID),
    FOREIGN KEY (DegreeID) REFERENCES Degree(DegreeID),
    FOREIGN KEY (ModuleID) REFERENCES TopicModule(ModuleID)
);

CREATE TABLE ModuleResource (
    ResourceID SERIAL PRIMARY KEY,
    ModuleID INT NOT NULL REFERENCES TopicModule(ModuleID),
    ResourceType VARCHAR(40),
    ResourceURL TEXT NOT NULL
);

-- =========================================================
-- USERS & ENROLMENT
-- =========================================================
CREATE TABLE Student (
    StudentID BIGSERIAL PRIMARY KEY,
    FirstName VARCHAR(255) NOT NULL,
    LastName  VARCHAR(255) NOT NULL,
    PersonalEmail VARCHAR(255),
    Phone VARCHAR(50),
    PasswordHash VARCHAR(255) NOT NULL
);

CREATE TABLE Enrollment (
    EnrollmentID BIGSERIAL PRIMARY KEY,
    StudentID BIGINT NOT NULL REFERENCES Student(StudentID),
    EnrollmentDate TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE TABLE EnrollmentDegree (
    EnrollmentID BIGINT NOT NULL,
    DegreeID INT NOT NULL,
    PRIMARY KEY (EnrollmentID, DegreeID),
    FOREIGN KEY (EnrollmentID) REFERENCES Enrollment(EnrollmentID),
    FOREIGN KEY (DegreeID) REFERENCES Degree(DegreeID)
);

-- =========================================================
-- ADMINS
-- =========================================================
CREATE TABLE Admin (
    AdminID SERIAL PRIMARY KEY,
    AdminName VARCHAR(255) NOT NULL,
    AdminSurname VARCHAR(255) NOT NULL,
    AdminPhone VARCHAR(50),
    AdminEmail VARCHAR(255) UNIQUE NOT NULL,
    AdminPasswordHash VARCHAR(255) NOT NULL,
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    IsSuperAdmin BOOLEAN NOT NULL DEFAULT FALSE
);

-- =========================================================
-- SESSIONS & RATINGS
-- =========================================================
CREATE TABLE Session (
    SessionID BIGSERIAL PRIMARY KEY,
    SessionTopic VARCHAR(255) NOT NULL
);

CREATE TABLE Rating (
    RatingID BIGSERIAL PRIMARY KEY,
    RatingValue SMALLINT NOT NULL
);

CREATE TABLE SessionRating (
    SessionID BIGINT NOT NULL,
    RatingID BIGINT NOT NULL,
    PRIMARY KEY (SessionID, RatingID),
    FOREIGN KEY (SessionID) REFERENCES Session(SessionID),
    FOREIGN KEY (RatingID) REFERENCES Rating(RatingID)
);

CREATE TABLE SessionStudent (
    SessionID BIGINT NOT NULL,
    StudentID BIGINT NOT NULL,
    PRIMARY KEY (SessionID, StudentID),
    FOREIGN KEY (SessionID) REFERENCES Session(SessionID),
    FOREIGN KEY (StudentID) REFERENCES Student(StudentID)
);

CREATE TABLE SessionTutor (
    SessionID BIGINT NOT NULL,
    TutorID BIGINT NOT NULL,
    PRIMARY KEY (SessionID, TutorID),
    FOREIGN KEY (SessionID) REFERENCES Session(SessionID),
    FOREIGN KEY (TutorID) REFERENCES Tutors(TutorID)
);

CREATE TABLE StudentTutor (
    StudentID BIGINT NOT NULL,
    TutorID BIGINT NOT NULL,
    PRIMARY KEY (StudentID, TutorID),
    FOREIGN KEY (StudentID) REFERENCES Student(StudentID),
    FOREIGN KEY (TutorID) REFERENCES Tutors(TutorID)
);

-- =========================================================
-- FORUMS
-- =========================================================
CREATE TABLE ForumTopic (
    Id SERIAL PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Subject VARCHAR(255) NOT NULL,
    Description TEXT NOT NULL,
    Contributions INT NOT NULL DEFAULT 0,
    Progress VARCHAR(20) NOT NULL DEFAULT 'Fresh'
        CHECK (Progress IN ('Fresh','Pending','Solved','Important','General')),
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE TABLE Reply (
    ReplyID SERIAL PRIMARY KEY,
    ForumTopicId INT NOT NULL REFERENCES ForumTopic(Id),
    Author VARCHAR(255) NOT NULL,
    Message TEXT NOT NULL,
    PostedAt TIMESTAMP NOT NULL DEFAULT NOW()
);

-- =========================================================
-- ANNOUNCEMENTS (ENUM + anonymous replies)
-- =========================================================
CREATE TABLE Announcements (
    AnnouncementID SERIAL PRIMARY KEY,
    Topic VARCHAR(255) NOT NULL,
    Discussion TEXT NOT NULL,
    Progress VARCHAR(20) NOT NULL DEFAULT 'Fresh'
        CHECK (Progress IN ('Fresh','Pending','Solved','Important','General')),
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    AdminID INT REFERENCES Admin(AdminID)
);


CREATE TABLE ReplyAnnouncements (
    ReplyID SERIAL PRIMARY KEY,
    AnnouncementID INT NOT NULL REFERENCES Announcements(AnnouncementID),
    ReplyText TEXT NOT NULL,
    PostedAt TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE INDEX IX_ReplyAnnouncements_Announcement ON ReplyAnnouncements(AnnouncementID);

-- =========================================================
-- CHAT SYSTEM (student ↔ tutor)
-- =========================================================
CREATE TABLE ChatSession (
    ChatSessionID BIGSERIAL PRIMARY KEY,
    StudentID BIGINT NOT NULL REFERENCES Student(StudentID),
    StartedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    EndedAt TIMESTAMP NULL,
    Topic VARCHAR(100)
);

CREATE TABLE ChatMessages (
    ChatMessageID BIGSERIAL PRIMARY KEY,
    ChatSessionID BIGINT NOT NULL REFERENCES ChatSession(ChatSessionID),
    MessageText TEXT NOT NULL,
    SentAt TIMESTAMP NOT NULL DEFAULT NOW(),

    -- Exactly one sender (student or tutor)
    SenderStudentID BIGINT NULL REFERENCES Student(StudentID),
    SenderTutorID   BIGINT NULL REFERENCES Tutors(TutorID),

    -- Optional direct recipient (at most one)
    ReceiverStudentID BIGINT NULL REFERENCES Student(StudentID),
    ReceiverTutorID   BIGINT NULL REFERENCES Tutors(TutorID),

    CONSTRAINT CK_ChatMessages_ExactlyOneSender
      CHECK (
        (SenderStudentID IS NOT NULL AND SenderTutorID IS NULL) OR
        (SenderStudentID IS NULL AND SenderTutorID IS NOT NULL)
      ),
    CONSTRAINT CK_ChatMessages_AtMostOneReceiver
      CHECK (ReceiverStudentID IS NULL OR ReceiverTutorID IS NULL)
);

CREATE INDEX IX_ChatMessages_Session       ON ChatMessages(ChatSessionID);
CREATE INDEX IX_ChatMessages_SentAt        ON ChatMessages(SentAt);
CREATE INDEX IX_ChatMessages_SenderStudent ON ChatMessages(SenderStudentID);
CREATE INDEX IX_ChatMessages_SenderTutor   ON ChatMessages(SenderTutorID);

-- =========================================================
-- MODULE PLANNING: weeks, content, assignments, projects, tests
-- =========================================================
CREATE TABLE ModulePlan (
    ModuleID INT PRIMARY KEY REFERENCES TopicModule(ModuleID),
    TotalWeeks INT NOT NULL CHECK (TotalWeeks >= 1),
    TestsAllowed INT NOT NULL DEFAULT 0 CHECK (TestsAllowed >= 0),
    AssignmentsRequired INT NOT NULL DEFAULT 0 CHECK (AssignmentsRequired >= 0),
    ProjectsRequired INT NOT NULL DEFAULT 0 CHECK (ProjectsRequired >= 0),
    CONSTRAINT CK_ModulePlan_AssignmentsOrProjects
      CHECK (AssignmentsRequired > 0 OR ProjectsRequired > 0)
);

CREATE TABLE ModuleWeek (
    WeekID SERIAL PRIMARY KEY,
    ModuleID INT NOT NULL REFERENCES TopicModule(ModuleID),
    WeekNumber INT NOT NULL CHECK (WeekNumber >= 1)
);

CREATE UNIQUE INDEX UQ_ModuleWeek_Module_WeekNumber
    ON ModuleWeek(ModuleID, WeekNumber);

CREATE INDEX IX_ModuleWeek_Module ON ModuleWeek(ModuleID);

CREATE TABLE WeekContent (
    ContentID SERIAL PRIMARY KEY,
    WeekID INT NOT NULL REFERENCES ModuleWeek(WeekID),
    ContentTitle VARCHAR(255) NOT NULL,
    PdfURL TEXT NULL,         -- preferred for real deployments
    PdfFile BYTEA NULL,       -- optional: store file bytes
    PdfMime VARCHAR(100) NULL,
    PdfSizeBytes BIGINT NULL,
    CONSTRAINT CK_WeekContent_HasFile CHECK (
        (PdfURL IS NOT NULL) OR (PdfFile IS NOT NULL)
    )
);

CREATE INDEX IX_WeekContent_Week ON WeekContent(WeekID);

CREATE TABLE ModuleAssignments (
    AssignmentID SERIAL PRIMARY KEY,
    ModuleID INT NOT NULL REFERENCES TopicModule(ModuleID),
    AssignmentTitle VARCHAR(255) NOT NULL,
    AssignmentPdfURL TEXT NULL,
    AssignmentPdf BYTEA NULL,
    AssignmentPdfMime VARCHAR(100) NULL,
    AssignmentPdfSizeBytes BIGINT NULL,
    DateDue DATE NOT NULL,
    AssignmentUploadPdfURL TEXT NULL,
    AssignmentUploadPdf BYTEA NULL,
    AssignmentUploadPdfMime VARCHAR(100) NULL,
    AssignmentUploadPdfSizeBytes BIGINT NULL,
    CONSTRAINT CK_Assignment_HasSpec CHECK (
        (AssignmentPdfURL IS NOT NULL) OR (AssignmentPdf IS NOT NULL)
    ),
    CONSTRAINT CK_Assignment_UploadEither CHECK (
        (AssignmentUploadPdfURL IS NULL AND AssignmentUploadPdf IS NULL) OR
        (AssignmentUploadPdfURL IS NOT NULL OR AssignmentUploadPdf IS NOT NULL)
    )
);

CREATE INDEX IX_ModuleAssignments_Module ON ModuleAssignments(ModuleID);

CREATE TABLE ModuleProjects (
    ProjectID SERIAL PRIMARY KEY,
    ModuleID INT NOT NULL REFERENCES TopicModule(ModuleID),
    ProjectTitle VARCHAR(255) NOT NULL,
    ProjectPdfURL TEXT NULL,
    ProjectPdf BYTEA NULL,
    ProjectPdfMime VARCHAR(100) NULL,
    ProjectPdfSizeBytes BIGINT NULL,
    DateDue DATE NOT NULL,
    UploadPdfURL TEXT NULL,
    UploadPdf BYTEA NULL,
    UploadPdfMime VARCHAR(100) NULL,
    UploadPdfSizeBytes BIGINT NULL,
    CONSTRAINT CK_Project_HasSpec CHECK (
        (ProjectPdfURL IS NOT NULL) OR (ProjectPdf IS NOT NULL)
    ),
    CONSTRAINT CK_Project_UploadEither CHECK (
        (UploadPdfURL IS NULL AND UploadPdf IS NULL) OR
        (UploadPdfURL IS NOT NULL OR UploadPdf IS NOT NULL)
    )
);

CREATE INDEX IX_ModuleProjects_Module ON ModuleProjects(ModuleID);

CREATE TABLE ModuleTests (
    TestID SERIAL PRIMARY KEY,
    ModuleID INT NOT NULL REFERENCES TopicModule(ModuleID),
    TestTitle VARCHAR(255) NOT NULL,
    TestWeek INT NOT NULL,
    TestDate DATE NOT NULL,
    CONSTRAINT UQ_Module_TestWeek UNIQUE (ModuleID, TestWeek)
);
-- Helpful indexes
CREATE INDEX IX_Degree_Faculty           ON Degree(FacultyID);
CREATE INDEX IX_TopicModule_Cluster      ON TopicModule(ClusterID);
CREATE INDEX IX_TopicModule_ModuleHead   ON TopicModule(ModuleHeadID);
CREATE INDEX IX_ModuleResource_Module    ON ModuleResource(ModuleID);
CREATE INDEX IX_Enrollment_Student       ON Enrollment(StudentID);
CREATE INDEX IX_EnrollmentDegree_Degree  ON EnrollmentDegree(DegreeID);
CREATE INDEX IX_ForumTopic_CreatedAt     ON ForumTopic(CreatedAt);
CREATE INDEX IX_Announcements_CreatedAt  ON Announcements(CreatedAt);
