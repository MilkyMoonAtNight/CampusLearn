using CampusLearn.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusLearn.Data
{
    public class CampusLearnContext : DbContext
    {
        public CampusLearnContext(DbContextOptions<CampusLearnContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Tutor> Tutors { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<TopicModule> Modules { get; set; }
        public DbSet<ModuleCluster> ModuleClusters { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<ChatSession> ChatSessions { get; set; }
        public DbSet<ChatMessages> ChatMessages { get; set; }
        public DbSet<ForumTopic> ForumTopics { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<ModuleResource> ModuleResources { get; set; }

        // Join tables
        public DbSet<DegreeModule> DegreeModules { get; set; }
        public DbSet<StudentTutor> StudentTutors { get; set; }
        public DbSet<SessionTutor> SessionTutors { get; set; }
        public DbSet<SessionStudent> SessionStudents { get; set; }
        public DbSet<EnrollmentDegree> EnrollmentDegrees { get; set; }
        public DbSet<SessionRating> SessionRatings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // Table & Column Mappings
            // =========================

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("student");
                entity.HasKey(e => e.StudentID);
                entity.Property(e => e.StudentID).HasColumnName("studentid");
                entity.Property(e => e.FirstName).HasColumnName("firstname");
                entity.Property(e => e.LastName).HasColumnName("lastname");
                entity.Property(e => e.PersonalEmail).HasColumnName("personalemail");
                entity.Property(e => e.Phone).HasColumnName("phone");
                entity.Property(e => e.PasswordHash).HasColumnName("passwordhash");
            });

            modelBuilder.Entity<Tutor>(entity =>
            {
                entity.ToTable("tutors");
                entity.HasKey(e => e.TutorID);
                entity.Property(e => e.TutorID).HasColumnName("tutorid");
                entity.Property(e => e.TutorName).HasColumnName("tutorname");
                entity.Property(e => e.TutorSurname).HasColumnName("tutorsurname");
                entity.Property(e => e.SpecialityID).HasColumnName("specialityid");

                entity.HasOne(t => t.Speciality)
                      .WithMany(s => s.Tutors)
                      .HasForeignKey(t => t.SpecialityID);
            });

            modelBuilder.Entity<Degree>(entity =>
            {
                entity.ToTable("degree");
                entity.HasKey(e => e.DegreeID);
                entity.Property(e => e.DegreeID).HasColumnName("degreeid");
                entity.Property(e => e.DegreeName).HasColumnName("degreename");
                entity.Property(e => e.FacultyID).HasColumnName("facultyid");
            });

            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.ToTable("faculty");
                entity.HasKey(e => e.FacultyID);
                entity.Property(e => e.FacultyID).HasColumnName("facultyid");
                entity.Property(e => e.FacultyName).HasColumnName("facultyname");
            });

            modelBuilder.Entity<TopicModule>(entity =>
            {
                entity.ToTable("topicmodule");
                entity.HasKey(e => e.ModuleID);
                entity.Property(e => e.ModuleID).HasColumnName("moduleid");
                entity.Property(e => e.ModuleName).HasColumnName("modulename");
                entity.Property(e => e.ClusterID).HasColumnName("clusterid");
                entity.Property(e => e.ModuleHeadID).HasColumnName("moduleheadid");

                entity.HasOne(tm => tm.ModuleCluster)
                      .WithMany(mc => mc.TopicModules)
                      .HasForeignKey(tm => tm.ClusterID);

                entity.HasOne(tm => tm.ModuleHead)
                      .WithMany(t => t.TopicModules)
                      .HasForeignKey(tm => tm.ModuleHeadID)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<ModuleCluster>(entity =>
            {
                entity.ToTable("modulecluster");
                entity.HasKey(e => e.ClusterID);
                entity.Property(e => e.ClusterID).HasColumnName("clusterid");
                entity.Property(e => e.ClusterName).HasColumnName("clustername");
            });

            modelBuilder.Entity<Speciality>(entity =>
            {
                entity.ToTable("speciality");
                entity.HasKey(e => e.SpecialityID);
                entity.Property(e => e.SpecialityID).HasColumnName("specialityid");
                entity.Property(e => e.SpecialityName).HasColumnName("specialityname");
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.ToTable("enrollment");
                entity.HasKey(e => e.EnrollmentID);
                entity.Property(e => e.EnrollmentID).HasColumnName("enrollmentid");
                entity.Property(e => e.StudentID).HasColumnName("studentid");
                entity.Property(e => e.EnrollmentDate).HasColumnName("enrollmentdate");

                entity.HasOne(e => e.Student)
                      .WithMany(s => s.Enrollments)
                      .HasForeignKey(e => e.StudentID);
            });

            modelBuilder.Entity<ModuleResource>(entity =>
            {
                entity.ToTable("moduleresource");
                entity.HasKey(e => e.ResourceID);
                entity.Property(e => e.ResourceID).HasColumnName("resourceid");
                entity.Property(e => e.ModuleID).HasColumnName("moduleid");
                entity.Property(e => e.ResourceType).HasColumnName("resourcetype");
                entity.Property(e => e.ResourceURL).HasColumnName("resourceurl");

                entity.HasOne(mr => mr.Module)
                      .WithMany(m => m.ModuleResources)
                      .HasForeignKey(mr => mr.ModuleID);
            });

            modelBuilder.Entity<ChatSession>(entity =>
            {
                entity.ToTable("chatsession");
                entity.HasKey(e => e.ChatSessionID);
                entity.Property(e => e.ChatSessionID).HasColumnName("chatsessionid");
                entity.Property(e => e.StudentID).HasColumnName("studentid");
                entity.Property(e => e.Topic).HasColumnName("topic");
                entity.Property(e => e.StartedAt).HasColumnName("startedat");
                entity.Property(e => e.EndedAt).HasColumnName("endedat");

                entity.HasOne(cs => cs.Student)
                      .WithMany(s => s.ChatSessions)
                      .HasForeignKey(cs => cs.StudentID);
            });

            modelBuilder.Entity<ChatMessages>(entity =>
            {
                entity.ToTable("chatmessages");
                entity.HasKey(e => e.ChatMessageID);
                entity.Property(e => e.ChatMessageID).HasColumnName("chatmessageid");
                entity.Property(e => e.ChatSessionID).HasColumnName("chatsessionid");
                entity.Property(e => e.IsFromStudent).HasColumnName("isfromstudent");
                entity.Property(e => e.MessageText).HasColumnName("messagetext");
                entity.Property(e => e.SentAt).HasColumnName("sentat");

                entity.HasOne(cm => cm.ChatSession)
                      .WithMany(cs => cs.Messages)
                      .HasForeignKey(cm => cm.ChatSessionID);
            });

            modelBuilder.Entity<ForumTopic>(entity =>
            {
                entity.ToTable("forumtopic");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Title).HasColumnName("title");
                entity.Property(e => e.Subject).HasColumnName("subject");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Contributions).HasColumnName("contributions");
                entity.Property(e => e.Progress).HasColumnName("progress");
                entity.Property(e => e.CreatedAt).HasColumnName("createdat");
            });

            modelBuilder.Entity<Reply>(entity =>
            {
                entity.ToTable("reply");
                entity.HasKey(e => e.ReplyID);
                entity.Property(e => e.ReplyID).HasColumnName("replyid");
                entity.Property(e => e.ForumTopicId).HasColumnName("forumtopicid");
                entity.Property(e => e.Author).HasColumnName("author");
                entity.Property(e => e.Message).HasColumnName("message");
                entity.Property(e => e.PostedAt).HasColumnName("postedat");

                entity.HasOne(r => r.ForumTopic)
                      .WithMany(ft => ft.Replies)
                      .HasForeignKey(r => r.ForumTopicId);
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("session");
                entity.HasKey(e => e.SessionID);
                entity.Property(e => e.SessionID).HasColumnName("sessionid");
                entity.Property(e => e.SessionTopic).HasColumnName("sessiontopic");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.ToTable("rating");
                entity.HasKey(e => e.RatingID);
                entity.Property(e => e.RatingID).HasColumnName("ratingid");
                entity.Property(e => e.RatingValue).HasColumnName("ratingvalue");
            });

            // =========================
            // Join Tables
            // =========================

            modelBuilder.Entity<DegreeModule>(entity =>
            {
                entity.ToTable("degreemodule");
                entity.HasKey(dm => new { dm.DegreeID, dm.ModuleID });
                entity.Property(dm => dm.DegreeID).HasColumnName("degreeid");
                entity.Property(dm => dm.ModuleID).HasColumnName("moduleid");

                entity.HasOne(dm => dm.Degree)
                      .WithMany(d => d.DegreeModules)
                      .HasForeignKey(dm => dm.DegreeID);

                entity.HasOne(dm => dm.Module)
                      .WithMany(m => m.DegreeModules)
                      .HasForeignKey(dm => dm.ModuleID);
            });

            modelBuilder.Entity<StudentTutor>(entity =>
            {
                entity.ToTable("studenttutor");
                entity.HasKey(st => new { st.StudentID, st.TutorID });
                entity.Property(st => st.StudentID).HasColumnName("studentid");
                entity.Property(st => st.TutorID).HasColumnName("tutorid");

                entity.HasOne(st => st.Student)
                      .WithMany(s => s.StudentTutors)
                      .HasForeignKey(st => st.StudentID);

                entity.HasOne(st => st.Tutor)
                      .WithMany(t => t.StudentTutors)
                      .HasForeignKey(st => st.TutorID);
            });

            modelBuilder.Entity<SessionTutor>(entity =>
            {
                entity.ToTable("sessiontutor");
                entity.HasKey(st => new { st.SessionID, st.TutorID });
                entity.Property(st => st.SessionID).HasColumnName("sessionid");
                entity.Property(st => st.TutorID).HasColumnName("tutorid");

                entity.HasOne(st => st.Session)
                      .WithMany(s => s.SessionTutors)
                      .HasForeignKey(st => st.SessionID);

                entity.HasOne(st => st.Tutor)
                      .WithMany(t => t.SessionTutors)
                      .HasForeignKey(st => st.TutorID);
            });

            modelBuilder.Entity<SessionStudent>(entity =>
            {
                entity.ToTable("sessionstudent");
                entity.HasKey(ss => new { ss.SessionID, ss.StudentID });
                entity.Property(ss => ss.SessionID).HasColumnName("sessionid");
                entity.Property(ss => ss.StudentID).HasColumnName("studentid");

                entity.HasOne(ss => ss.Session)
                      .WithMany(s => s.SessionStudents)
                      .HasForeignKey(ss => ss.SessionID);

                entity.HasOne(ss => ss.Student)
                      .WithMany(s => s.SessionStudents)
                      .HasForeignKey(ss => ss.StudentID);
            });

            modelBuilder.Entity<EnrollmentDegree>(entity =>
            {
                entity.ToTable("enrollmentdegree");
                entity.HasKey(ed => new { ed.EnrollmentID, ed.DegreeID });
                entity.Property(ed => ed.EnrollmentID).HasColumnName("enrollmentid");
                entity.Property(ed => ed.DegreeID).HasColumnName("degreeid");

                entity.HasOne(ed => ed.Enrollment)
                      .WithMany(e => e.EnrollmentDegrees)
                      .HasForeignKey(ed => ed.EnrollmentID);

                entity.HasOne(ed => ed.Degree)
                      .WithMany(d => d.EnrollmentDegrees)
                      .HasForeignKey(ed => ed.DegreeID);
            });

            modelBuilder.Entity<SessionRating>(entity =>
            {
                entity.ToTable("sessionrating");
                entity.HasKey(sr => new { sr.SessionID, sr.RatingID });
                entity.Property(sr => sr.SessionID).HasColumnName("sessionid");
                entity.Property(sr => sr.RatingID).HasColumnName("ratingid");

                entity.HasOne(sr => sr.Session)
                      .WithMany(s => s.SessionRatings)
                      .HasForeignKey(sr => sr.SessionID);

                entity.HasOne(sr => sr.Rating)
                      .WithMany(r => r.SessionRatings)
                      .HasForeignKey(sr => sr.RatingID);
            });
        }
    }
}