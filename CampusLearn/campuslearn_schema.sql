-- =========================
-- Faculty
-- =========================
CREATE TABLE Faculty (
    FacultyID SERIAL PRIMARY KEY,
    FacultyName VARCHAR(120) NOT NULL
);

-- =========================
-- Degree
-- =========================
CREATE TABLE Degree (
    DegreeID SERIAL PRIMARY KEY,
    DegreeName VARCHAR(255) NOT NULL,
    FacultyID INT NOT NULL,
    FOREIGN KEY (FacultyID) REFERENCES Faculty(FacultyID)
);

-- =========================
-- ModuleCluster
-- =========================
CREATE TABLE ModuleCluster (
    ClusterID SERIAL PRIMARY KEY,
    ClusterName VARCHAR(100) NOT NULL
);

-- =========================
-- Tutors
-- =========================
CREATE TABLE Tutors (
    TutorID BIGSERIAL PRIMARY KEY,
    TutorName VARCHAR(255) NOT NULL,
    TutorSurname VARCHAR(255) NOT NULL,
    SpecialityID INT NULL
);

-- =========================
-- Speciality
-- =========================
CREATE TABLE Speciality (
    SpecialityID SERIAL PRIMARY KEY,
    SpecialityName VARCHAR(100) NOT NULL
);

ALTER TABLE Tutors
ADD CONSTRAINT FK_Tutors_Speciality FOREIGN KEY (SpecialityID)
REFERENCES Speciality(SpecialityID);

-- =========================
-- TopicModule
-- =========================
CREATE TABLE TopicModule (
    ModuleID SERIAL PRIMARY KEY,
    ModuleName VARCHAR(255) NOT NULL,
    ClusterID INT NULL,
    ModuleHeadID BIGINT NULL,
    FOREIGN KEY (ClusterID) REFERENCES ModuleCluster(ClusterID),
    FOREIGN KEY (ModuleHeadID) REFERENCES Tutors(TutorID)
);

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
    ResourceID SERIAL PRIMARY KEY,
    ModuleID INT NOT NULL,
    ResourceType VARCHAR(40),
    ResourceURL TEXT NOT NULL,
    FOREIGN KEY (ModuleID) REFERENCES TopicModule(ModuleID)
);

-- =========================
-- Student
-- =========================
CREATE TABLE Student (
    StudentID BIGSERIAL PRIMARY KEY,
    FirstName VARCHAR(255) NOT NULL,
    LastName VARCHAR(255) NOT NULL,
    PersonalEmail VARCHAR(255),
    Phone VARCHAR(50),
    PasswordHash VARCHAR(255) NOT NULL
);

-- =========================
-- Enrollment
-- =========================
CREATE TABLE Enrollment (
    EnrollmentID BIGSERIAL PRIMARY KEY,
    StudentID BIGINT NOT NULL,
    EnrollmentDate TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
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
    ChatSessionID BIGSERIAL PRIMARY KEY,
    StudentID BIGINT NOT NULL,
    StartedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    EndedAt TIMESTAMP NULL,
    Topic VARCHAR(100),
    FOREIGN KEY (StudentID) REFERENCES Student(StudentID)
);

-- =========================
-- ChatMessages
-- =========================
CREATE TABLE ChatMessages (
    ChatMessageID BIGSERIAL PRIMARY KEY,
    ChatSessionID BIGINT NOT NULL,
    IsFromStudent BOOLEAN NOT NULL,
    MessageText TEXT NOT NULL,
    SentAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ChatSessionID) REFERENCES ChatSession(ChatSessionID)
);

-- =========================
-- ForumTopic
-- =========================
CREATE TABLE ForumTopic (
    Id SERIAL PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Subject VARCHAR(255) NOT NULL,
    Description TEXT NOT NULL,
    Contributions INT NOT NULL DEFAULT 0,
    Progress VARCHAR(50) NOT NULL DEFAULT 'Fresh',
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- =========================
-- Reply
-- =========================
CREATE TABLE Reply (
    ReplyID SERIAL PRIMARY KEY,
    ForumTopicId INT NOT NULL,
    Author VARCHAR(255) NOT NULL,
    Message TEXT NOT NULL,
    PostedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ForumTopicId) REFERENCES ForumTopic(Id)
);

-- =========================
-- Session
-- =========================
CREATE TABLE Session (
    SessionID BIGSERIAL PRIMARY KEY,
    SessionTopic VARCHAR(255) NOT NULL
);

-- =========================
-- Rating
-- =========================
CREATE TABLE Rating (
    RatingID BIGSERIAL PRIMARY KEY,
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






