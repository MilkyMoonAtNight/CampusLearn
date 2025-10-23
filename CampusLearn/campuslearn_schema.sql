CREATE TABLE Faculty (
    FacultyID INT PRIMARY KEY IDENTITY(1,1),
    FacultyName NVARCHAR(120) NOT NULL
);

-- =========================
-- Degree
-- =========================
CREATE TABLE Degree (
    DegreeID INT PRIMARY KEY IDENTITY(1,1),
    DegreeName NVARCHAR(255) NOT NULL,
    FacultyID INT NOT NULL,
    FOREIGN KEY (FacultyID) REFERENCES Faculty(FacultyID)
);

-- =========================
-- TopicModule
-- =========================
CREATE TABLE TopicModule (
    ModuleID INT PRIMARY KEY IDENTITY(1,1),
    ModuleName NVARCHAR(255) NOT NULL,
    ClusterID INT NULL,
    ModuleHeadID BIGINT NULL
);

-- =========================
-- ModuleCluster
-- =========================
CREATE TABLE ModuleCluster (
    ClusterID INT PRIMARY KEY IDENTITY(1,1),
    ClusterName NVARCHAR(100) NOT NULL
);

ALTER TABLE TopicModule
ADD CONSTRAINT FK_TopicModule_Cluster FOREIGN KEY (ClusterID) REFERENCES ModuleCluster(ClusterID);

-- =========================
-- Tutors
-- =========================
CREATE TABLE Tutors (
    TutorID BIGINT PRIMARY KEY IDENTITY(1,1),
    TutorName NVARCHAR(255) NOT NULL,
    TutorSurname NVARCHAR(255) NOT NULL,
    SpecialityID INT NULL
);

-- =========================
-- Speciality
-- =========================
CREATE TABLE Speciality (
    SpecialityID INT PRIMARY KEY IDENTITY(1,1),
    SpecialityName NVARCHAR(100) NOT NULL
);

ALTER TABLE Tutors
ADD CONSTRAINT FK_Tutors_Speciality FOREIGN KEY (SpecialityID) REFERENCES Speciality(SpecialityID);

ALTER TABLE TopicModule
ADD CONSTRAINT FK_TopicModule_Tutor FOREIGN KEY (ModuleHeadID) REFERENCES Tutors(TutorID);

-- =========================
-- DegreeModule (Join Table)
-- =========================
CREATE TABLE DegreeModule (
    DegreeID INT NOT NULL,
    ModuleID INT NOT NULL,
    PRIMARY KEY (DegreeID, ModuleID),
    FOREIGN KEY (DegreeID) REFERENCES Degree(DegreeID),
    FOREIGN KEY (ModuleID) REFERENCES TopicModule(ModuleID)
);

-- =========================
-- ModuleResource
-- =========================
CREATE TABLE ModuleResource (
    ResourceID INT PRIMARY KEY IDENTITY(1,1),
    ModuleID INT NOT NULL,
    ResourceType NVARCHAR(40),
    ResourceURL NVARCHAR(MAX) NOT NULL,
    FOREIGN KEY (ModuleID) REFERENCES TopicModule(ModuleID)
);

-- =========================
-- Student
-- =========================
CREATE TABLE Student (
    StudentID BIGINT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(255) NOT NULL,
    LastName NVARCHAR(255) NOT NULL,
    PersonalEmail NVARCHAR(255),
    Phone NVARCHAR(50),
    PasswordHash NVARCHAR(255) NOT NULL
);

-- =========================
-- Enrollment
-- =========================
CREATE TABLE Enrollment (
    EnrollmentID BIGINT PRIMARY KEY IDENTITY(1,1),
    StudentID BIGINT NOT NULL,
    EnrollmentDate DATETIME NOT NULL,
    FOREIGN KEY (StudentID) REFERENCES Student(StudentID)
);

-- =========================
-- EnrollmentDegree (Join Table)
-- =========================
CREATE TABLE EnrollmentDegree (
    EnrollmentID BIGINT NOT NULL,
    DegreeID INT NOT NULL,
    PRIMARY KEY (EnrollmentID, DegreeID),
    FOREIGN KEY (EnrollmentID) REFERENCES Enrollment(EnrollmentID),
    FOREIGN KEY (DegreeID) REFERENCES Degree(DegreeID)
);

-- =========================
-- ChatSession
-- =========================
CREATE TABLE ChatSession (
    ChatSessionID BIGINT PRIMARY KEY IDENTITY(1,1),
    StudentID BIGINT NOT NULL,
    StartedAt DATETIME NOT NULL DEFAULT GETDATE(),
    EndedAt DATETIME NULL,
    Topic NVARCHAR(100),
    FOREIGN KEY (StudentID) REFERENCES Student(StudentID)
);

-- =========================
-- ChatMessages
-- =========================
CREATE TABLE ChatMessages (
    ChatMessageID BIGINT PRIMARY KEY IDENTITY(1,1),
    ChatSessionID BIGINT NOT NULL,
    IsFromStudent BIT NOT NULL,
    MessageText NVARCHAR(MAX) NOT NULL,
    SentAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (ChatSessionID) REFERENCES ChatSession(ChatSessionID)
);

-- =========================
-- ForumTopic + Reply (embedded class)
-- =========================
CREATE TABLE ForumTopic (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(255) NOT NULL,
    Subject NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Contributions INT NOT NULL DEFAULT 0,
    Progress NVARCHAR(50) NOT NULL DEFAULT 'Fresh',
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Reply (
    ReplyID INT PRIMARY KEY IDENTITY(1,1),
    ForumTopicId INT NOT NULL,
    Author NVARCHAR(255) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    PostedAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (ForumTopicId) REFERENCES ForumTopic(Id)
);

-- =========================
-- Session
-- =========================
CREATE TABLE Session (
    SessionID BIGINT PRIMARY KEY IDENTITY(1,1),
    SessionTopic NVARCHAR(255) NOT NULL
);

-- =========================
-- Rating
-- =========================
CREATE TABLE Rating (
    RatingID BIGINT PRIMARY KEY IDENTITY(1,1),
    RatingValue SMALLINT NOT NULL
);

-- =========================
-- SessionRating (Join Table)
-- =========================
CREATE TABLE SessionRating (
    SessionID BIGINT NOT NULL,
    RatingID BIGINT NOT NULL,
    PRIMARY KEY (SessionID, RatingID),
    FOREIGN KEY (SessionID) REFERENCES Session(SessionID),
    FOREIGN KEY (RatingID) REFERENCES Rating(RatingID)
);

-- =========================
-- SessionStudent (Join Table)
-- =========================
CREATE TABLE SessionStudent (
    SessionID BIGINT NOT NULL,
    StudentID BIGINT NOT NULL,
    PRIMARY KEY (SessionID, StudentID),
    FOREIGN KEY (SessionID) REFERENCES Session(SessionID),
    FOREIGN KEY (StudentID) REFERENCES Student(StudentID)
);

-- =========================
-- SessionTutor (Join Table)
-- =========================
CREATE TABLE SessionTutor (
    SessionID BIGINT NOT NULL,
    TutorID BIGINT NOT NULL,
    PRIMARY KEY (SessionID, TutorID),
    FOREIGN KEY (SessionID) REFERENCES Session(SessionID),
    FOREIGN KEY (TutorID) REFERENCES Tutors(TutorID)
);

-- =========================
-- StudentTutor (Join Table)
-- =========================
CREATE TABLE StudentTutor (
    StudentID BIGINT NOT NULL,
    TutorID BIGINT NOT NULL,
    PRIMARY KEY (StudentID, TutorID),
    FOREIGN KEY (StudentID) REFERENCES Student(StudentID),
    FOREIGN KEY (TutorID) REFERENCES Tutors(TutorID)
);






