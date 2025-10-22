-- ============ CORE PEOPLE ============

CREATE TABLE Student (
  StudentID       BIGSERIAL PRIMARY KEY,
  FirstName       VARCHAR(60) NOT NULL,
  MiddleName      VARCHAR(60),
  LastName        VARCHAR(60) NOT NULL,
  PersonalEmail   VARCHAR(255) UNIQUE,
  Phone           VARCHAR(30),
  PasswordHash    TEXT NOT NULL
);

CREATE TABLE Speciality (
  SpecialityID    SERIAL PRIMARY KEY,
  SpecialityName  VARCHAR(100) UNIQUE NOT NULL
);

CREATE TABLE Tutor (
  TutorID         BIGSERIAL PRIMARY KEY,
  TutorName       VARCHAR(60) NOT NULL,
  TutorMiddleInit VARCHAR(10),
  TutorSurname    VARCHAR(60) NOT NULL,
  SpecialityID    INT REFERENCES Speciality(SpecialityID)
);

CREATE TABLE StudentTutor (
  StudentID BIGINT REFERENCES Student(StudentID) ON DELETE CASCADE,
  TutorID   BIGINT REFERENCES Tutor(TutorID)     ON DELETE CASCADE,
  PRIMARY KEY (StudentID, TutorID)
);

-- ============ FACULTIES, DEGREES, MODULES ============

CREATE TABLE Faculty (
  FacultyID     SERIAL PRIMARY KEY,
  FacultyName   VARCHAR(120) UNIQUE NOT NULL
);

CREATE TABLE Degree (
  DegreeID       SERIAL PRIMARY KEY,
  DegreeName     VARCHAR(160) NOT NULL,
  FacultyID      INT REFERENCES Faculty(FacultyID) NOT NULL,
  DegreeDuration SMALLINT,
  DegreeNQF      SMALLINT,
  DegreeCredits  SMALLINT,
  UNIQUE (DegreeName, FacultyID)
);

CREATE TABLE ModuleCluster (
  ClusterID   SERIAL PRIMARY KEY,
  ClusterName VARCHAR(100) UNIQUE NOT NULL
);

CREATE TABLE Module (
  ModuleID      SERIAL PRIMARY KEY,
  ModuleName    VARCHAR(160) UNIQUE NOT NULL,
  ClusterID     INT REFERENCES ModuleCluster(ClusterID),
  ModuleHeadID  BIGINT REFERENCES Tutor(TutorID)
);

CREATE TABLE DegreeModule (
  DegreeID  INT    REFERENCES Degree(DegreeID) ON DELETE CASCADE,
  ModuleID  INT    REFERENCES Module(ModuleID) ON DELETE CASCADE,
  PRIMARY KEY (DegreeID, ModuleID)
);

CREATE TABLE ModuleResource (
  ResourceID   SERIAL PRIMARY KEY,
  ModuleID     INT REFERENCES Module(ModuleID) ON DELETE CASCADE,
  ResourceType VARCHAR(40),
  ResourceURL  TEXT NOT NULL
);

-- ============ ENROLLMENT ============

CREATE TABLE Enrollment (
  EnrollmentID   BIGSERIAL PRIMARY KEY,
  StudentID      BIGINT REFERENCES Student(StudentID) ON DELETE CASCADE,
  EnrollmentDate DATE NOT NULL
);

CREATE TABLE EnrollmentDegree (
  EnrollmentID BIGINT REFERENCES Enrollment(EnrollmentID) ON DELETE CASCADE,
  DegreeID     INT    REFERENCES Degree(DegreeID)        ON DELETE CASCADE,
  PRIMARY KEY (EnrollmentID, DegreeID)
);

-- ============ TUTORING SESSIONS ============

CREATE TABLE Session (
  SessionID     BIGSERIAL PRIMARY KEY,
  SessionTopic  VARCHAR(200) NOT NULL,
  SessionStart  TIMESTAMP NOT NULL,
  SessionEnd    TIMESTAMP
);

CREATE TABLE SessionTutor (
  SessionID BIGINT REFERENCES Session(SessionID) ON DELETE CASCADE,
  TutorID   BIGINT REFERENCES Tutor(TutorID)     ON DELETE CASCADE,
  PRIMARY KEY (SessionID, TutorID)
);

CREATE TABLE SessionStudent (
  SessionID BIGINT REFERENCES Session(SessionID) ON DELETE CASCADE,
  StudentID BIGINT REFERENCES Student(StudentID) ON DELETE CASCADE,
  PRIMARY KEY (SessionID, StudentID)
);

-- ============ RATINGS ============

CREATE TABLE Rating (
  RatingID       BIGSERIAL PRIMARY KEY,
  RatingValue    SMALLINT NOT NULL CHECK (RatingValue BETWEEN 1 AND 5),
  RatingComment  TEXT,
  RatingDate     DATE NOT NULL DEFAULT GETDATE()
);

CREATE TABLE SessionRating (
  SessionID BIGINT REFERENCES Session(SessionID) ON DELETE CASCADE,
  RatingID  BIGINT REFERENCES Rating(RatingID)   ON DELETE CASCADE,
  PRIMARY KEY (SessionID, RatingID)
);

-- ============ AI CHAT SYSTEM ============

CREATE TABLE ChatSession (
    ChatSessionID SERIAL PRIMARY KEY,
    StudentID BIGINT REFERENCES Student(StudentID) ON DELETE SET NULL,
    StartedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    EndedAt TIMESTAMP,
    Topic VARCHAR(255),
    IsActive BOOLEAN DEFAULT TRUE
);

CREATE TABLE ChatMessage (
    ChatMessageID SERIAL PRIMARY KEY,
    ChatSessionID INT REFERENCES ChatSession(ChatSessionID) ON DELETE CASCADE,
    SenderRole VARCHAR(20) CHECK (SenderRole IN ('User', 'AI', 'Tutor')),
    MessageText TEXT NOT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);



-- Indexes
CREATE INDEX ix_enrollment_student ON Enrollment(StudentID);
CREATE INDEX ix_session_start ON Session(SessionStart);
CREATE INDEX ix_degreemodule_mod ON DegreeModule(ModuleID);
CREATE INDEX ix_sessionstudent_stu ON SessionStudent(StudentID);
CREATE INDEX ix_sessiontutor_tut ON SessionTutor(TutorID);
CREATE INDEX idx_chatsession_student ON ChatSession(StudentID);
CREATE INDEX idx_chatmessage_session ON ChatMessage(ChatSessionID);





