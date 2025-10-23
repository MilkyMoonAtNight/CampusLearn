using CampusLearn.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusLearn.Data
{
    public class CampusLearnContext : DbContext
    {
        public CampusLearnContext(DbContextOptions<CampusLearnContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Tutors> Tutors { get; set; }
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
            // Primary Keys (single)
            // =========================
            modelBuilder.Entity<Student>().HasKey(s => s.StudentID);
            modelBuilder.Entity<Tutors>().HasKey(t => t.TutorID);
            modelBuilder.Entity<Degree>().HasKey(d => d.DegreeID);
            modelBuilder.Entity<Faculty>().HasKey(f => f.FacultyID);
            modelBuilder.Entity<TopicModule>().HasKey(tm => tm.ModuleID);
            modelBuilder.Entity<ModuleCluster>().HasKey(mc => mc.ClusterID);
            modelBuilder.Entity<Speciality>().HasKey(sp => sp.SpecialityID);
            modelBuilder.Entity<Enrollment>().HasKey(e => e.EnrollmentID);
            modelBuilder.Entity<ChatSession>().HasKey(cs => cs.ChatSessionID);
            modelBuilder.Entity<ChatMessages>().HasKey(cm => cm.ChatMessageID);
            modelBuilder.Entity<ForumTopic>().HasKey(ft => ft.Id);
            modelBuilder.Entity<Session>().HasKey(s => s.SessionID);
            modelBuilder.Entity<Rating>().HasKey(r => r.RatingID);
            modelBuilder.Entity<ModuleResource>().HasKey(mr => mr.ResourceID);

            // =========================
            // Composite Keys (join tables)
            // =========================
            modelBuilder.Entity<DegreeModule>().HasKey(dm => new { dm.DegreeID, dm.ModuleID });
            modelBuilder.Entity<StudentTutor>().HasKey(st => new { st.StudentID, st.TutorID });
            modelBuilder.Entity<SessionTutor>().HasKey(st => new { st.SessionID, st.TutorID });
            modelBuilder.Entity<SessionStudent>().HasKey(ss => new { ss.SessionID, ss.StudentID });
            modelBuilder.Entity<EnrollmentDegree>().HasKey(ed => new { ed.EnrollmentID, ed.DegreeID });
            modelBuilder.Entity<SessionRating>().HasKey(sr => new { sr.SessionID, sr.RatingID });

            // =========================
            // Relationships
            // =========================

            // DegreeModule
            modelBuilder.Entity<DegreeModule>()
                .HasOne(dm => dm.Degree)
                .WithMany(d => d.DegreeModules)
                .HasForeignKey(dm => dm.DegreeID);

            modelBuilder.Entity<DegreeModule>()
                .HasOne(dm => dm.Module)
                .WithMany(m => m.DegreeModules)
                .HasForeignKey(dm => dm.ModuleID);

            // StudentTutor
            modelBuilder.Entity<StudentTutor>()
                .HasOne(st => st.Student)
                .WithMany(s => s.StudentTutors)
                .HasForeignKey(st => st.StudentID);

            modelBuilder.Entity<StudentTutor>()
                .HasOne(st => st.Tutor)
                .WithMany(t => t.StudentTutors)
                .HasForeignKey(st => st.TutorID);

            // SessionTutor
            modelBuilder.Entity<SessionTutor>()
                .HasOne(st => st.Session)
                .WithMany(s => s.SessionTutors)
                .HasForeignKey(st => st.SessionID);

            modelBuilder.Entity<SessionTutor>()
                .HasOne(st => st.Tutor)
                .WithMany(t => t.SessionTutors)
                .HasForeignKey(st => st.TutorID);

            // SessionStudent
            modelBuilder.Entity<SessionStudent>()
                .HasOne(ss => ss.Session)
                .WithMany(s => s.SessionStudents)
                .HasForeignKey(ss => ss.SessionID);

            modelBuilder.Entity<SessionStudent>()
                .HasOne(ss => ss.Student)
                .WithMany(s => s.SessionStudents)
                .HasForeignKey(ss => ss.StudentID);

            // EnrollmentDegree
            modelBuilder.Entity<EnrollmentDegree>()
                .HasOne(ed => ed.Enrollment)
                .WithMany(e => e.EnrollmentDegrees)
                .HasForeignKey(ed => ed.EnrollmentID);

            modelBuilder.Entity<EnrollmentDegree>()
                .HasOne(ed => ed.Degree)
                .WithMany(d => d.EnrollmentDegrees)
                .HasForeignKey(ed => ed.DegreeID);

            // SessionRating
            modelBuilder.Entity<SessionRating>()
                .HasOne(sr => sr.Session)
                .WithMany(s => s.SessionRatings)
                .HasForeignKey(sr => sr.SessionID);

            modelBuilder.Entity<SessionRating>()
                .HasOne(sr => sr.Rating)
                .WithMany(r => r.SessionRatings)
                .HasForeignKey(sr => sr.RatingID);

            // ✅ TopicModule -> ModuleCluster
            modelBuilder.Entity<TopicModule>()
                .HasOne(tm => tm.ModuleCluster)
                .WithMany(mc => mc.TopicModules)
                .HasForeignKey(tm => tm.ClusterID);

            modelBuilder.Entity<TopicModule>().ToTable("TopicModule");

            // ✅ TopicModule -> Tutors (ModuleHead)
            modelBuilder.Entity<TopicModule>()
                .HasOne(tm => tm.ModuleHead)
                .WithMany(t => t.TopicModules)
                .HasForeignKey(tm => tm.ModuleHeadID);

            // Tutors -> Speciality
            modelBuilder.Entity<Tutors>()
                .HasOne(t => t.Speciality)
                .WithMany(s => s.Tutors)
                .HasForeignKey(t => t.SpecialityID);
        }
    }
}