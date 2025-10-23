-- =========================
-- Faculty
-- =========================
INSERT INTO Faculty (FacultyName) VALUES
('Faculty of IT'),
('Faculty of Business'),
('Faculty of Engineering');

-- =========================
-- Degree
-- =========================
INSERT INTO Degree (DegreeName, FacultyID) VALUES
('Bachelor of Computing', 1),
('Diploma in Business Management', 2),
('BEng Software Engineering', 3);

-- =========================
-- ModuleCluster
-- =========================
INSERT INTO ModuleCluster (ClusterName) VALUES
('Programming Fundamentals'),
('Data Science'),
('Business Studies');

-- =========================
-- Tutors
-- =========================
INSERT INTO Tutors (TutorName, TutorSurname, SpecialityID) VALUES
('Alice', 'Johnson', NULL),
('Bob', 'Smith', NULL),
('Catherine', 'Mokoena', NULL);

-- =========================
-- TopicModule
-- =========================
INSERT INTO TopicModule (ModuleName, ClusterID, ModuleHeadID) VALUES
('Programming 101', 1, 1),
('Database Development', 1, 2),
('Machine Learning', 2, 3),
('Business Communication', 3, 2);

-- =========================
-- DegreeModule
-- =========================
INSERT INTO DegreeModule (DegreeID, ModuleID) VALUES
(1, 1), (1, 2), (1, 3), -- Bachelor of Computing
(2, 4),                 -- Business Diploma
(3, 1), (3, 2), (3, 3); -- BEng Software Eng

-- =========================
-- ModuleResource
-- =========================
INSERT INTO ModuleResource (ModuleID, ResourceType, ResourceURL) VALUES
(1, 'PDF', 'https://campuslearn/resources/prog101.pdf'),
(2, 'Video', 'https://campuslearn/resources/dbd381.mp4'),
(3, 'Slides', 'https://campuslearn/resources/mlg381.pptx');

-- =========================
-- Students
-- =========================
INSERT INTO Student (FirstName, LastName, PersonalEmail, Phone, PasswordHash) VALUES
('Kevin', 'Venter', 'kevin@student.belgiumcampus.ac.za', '012-3456789', 'hashed_pw1'),
('Alice', 'Smith', 'alice@student.belgiumcampus.ac.za', '011-2233445', 'hashed_pw2'),
('Bob', 'Ngwenya', 'bob@student.belgiumcampus.ac.za', '010-9988776', 'hashed_pw3');

-- =========================
-- Enrollment
-- =========================
INSERT INTO Enrollment (StudentID, EnrollmentDate) VALUES
(1, CURRENT_TIMESTAMP),
(2, CURRENT_TIMESTAMP),
(3, CURRENT_TIMESTAMP);

-- =========================
-- EnrollmentDegree
-- =========================
INSERT INTO EnrollmentDegree (EnrollmentID, DegreeID) VALUES
(1, 1), -- Kevin → Bachelor of Computing
(2, 2), -- Alice → Business Diploma
(3, 3); -- Bob → BEng Software Eng

-- =========================
-- ChatSession
-- =========================
INSERT INTO ChatSession (StudentID, Topic) VALUES
(1, 'Help with Database Assignment'),
(2, 'Clarification on Business Project');

-- =========================
-- ChatMessages
-- =========================
INSERT INTO ChatMessages (ChatSessionID, IsFromStudent, MessageText) VALUES
(1, TRUE, 'Hi, I need help with SQL joins.'),
(1, FALSE, 'Sure, let’s go through an example.'),
(2, TRUE, 'Can you explain the project requirements?'),
(2, FALSE, 'Yes, I’ll send you the rubric.');

-- =========================
-- ForumTopic
-- =========================
INSERT INTO ForumTopic (Title, Subject, Description) VALUES
('Best Practices in Programming', 'Coding Standards', 'Share your coding tips here'),
('AI in Education', 'Machine Learning', 'Discuss how AI is shaping learning');

-- =========================
-- Reply
-- =========================
INSERT INTO Reply (ForumTopicId, Author, Message) VALUES
(1, 'Kevin', 'Always comment your code!'),
(1, 'Alice', 'Use meaningful variable names.'),
(2, 'Bob', 'AI tutors are the future.');

-- =========================
-- Sessions
-- =========================
INSERT INTO Session (SessionTopic) VALUES
('Database Workshop'),
('Machine Learning Lab');

-- =========================
-- Ratings
-- =========================
INSERT INTO Rating (RatingValue) VALUES
(1), (2), (3), (4), (5);

-- =========================
-- SessionTutor
-- =========================
INSERT INTO SessionTutor (SessionID, TutorID) VALUES
(1, 2), (2, 3);

-- =========================
-- SessionStudent
-- =========================
INSERT INTO SessionStudent (SessionID, StudentID) VALUES
(1, 1), (1, 2), (2, 3);

-- =========================
-- SessionRating
-- =========================
INSERT INTO SessionRating (SessionID, RatingID) VALUES
(1, 5), (2, 4);

-- =========================
-- StudentTutor
-- =========================
INSERT INTO StudentTutor (StudentID, TutorID) VALUES
(1, 1), (2, 2), (3, 3);