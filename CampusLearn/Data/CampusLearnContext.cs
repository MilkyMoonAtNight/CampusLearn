using CampusLearn.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusLearn.Data
{
    public class CampusLearnContext : DbContext
    {
        public CampusLearnContext(DbContextOptions<CampusLearnContext> options) : base(options) { }

        // People
        public DbSet<Student> Students { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<Tutors> Tutors { get; set; }
        public DbSet<StudentTutor> StudentTutors { get; set; }

        // Academic
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<ModuleCluster> ModuleClusters { get; set; }
        public DbSet<TopicModule> Modules { get; set; }
        public DbSet<DegreeModule> DegreeModules { get; set; }
        public DbSet<ModuleResource> ModuleResources { get; set; }

        // Enrollment
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<EnrollmentDegree> EnrollmentDegrees { get; set; }

        // Sessions
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SessionTutor> SessionTutors { get; set; }
        public DbSet<SessionStudent> SessionStudents { get; set; }

        // Ratings
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<SessionRating> SessionRatings { get; set; }

        // Chat
        public DbSet<ChatSession> ChatSessions { get; set; }
        public DbSet<ChatMessages> ChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Composite keys ---

            modelBuilder.Entity<DegreeModule>()
                .HasKey(dm => new { dm.DegreeID, dm.ModuleID });
            modelBuilder.Entity<StudentTutor>()
                .HasKey(st => new { st.StudentID, st.TutorID });
            modelBuilder.Entity<SessionTutor>()
                .HasKey(st => new { st.SessionID, st.TutorID });
            modelBuilder.Entity<SessionStudent>()
                .HasKey(ss => new { ss.SessionID, ss.StudentID });
            modelBuilder.Entity<EnrollmentDegree>()
                .HasKey(ed => new { ed.EnrollmentID, ed.DegreeID });
            modelBuilder.Entity<SessionRating>()
                .HasKey(sr => new { sr.SessionID, sr.RatingID });

            // --- Relationships (optional but recommended) ---

            modelBuilder.Entity<DegreeModule>()
                .HasOne(dm => dm.Degree)
                .WithMany(d => d.DegreeModules)
                .HasForeignKey(dm => dm.DegreeID);

            modelBuilder.Entity<DegreeModule>()
                .HasOne(dm => dm.Module)
                .WithMany(m => m.DegreeModules)
                .HasForeignKey(dm => dm.ModuleID);

            modelBuilder.Entity<StudentTutor>()
                .HasOne(st => st.Student)
                .WithMany(s => s.StudentTutors)
                .HasForeignKey(st => st.StudentID);

            modelBuilder.Entity<StudentTutor>()
                .HasOne(st => st.Tutor)
                .WithMany(t => t.StudentTutors)
                .HasForeignKey(st => st.TutorID);

            modelBuilder.Entity<SessionTutor>()
                .HasOne(st => st.Session)
                .WithMany(s => s.SessionTutors)
                .HasForeignKey(st => st.SessionID);

            modelBuilder.Entity<SessionTutor>()
                .HasOne(st => st.Tutor)
                .WithMany(t => t.SessionTutors)
                .HasForeignKey(st => st.TutorID);

            modelBuilder.Entity<SessionStudent>()
                .HasOne(ss => ss.Session)
                .WithMany(s => s.SessionStudents)
                .HasForeignKey(ss => ss.SessionID);

            modelBuilder.Entity<SessionStudent>()
                .HasOne(ss => ss.Student)
                .WithMany(s => s.SessionStudents)
                .HasForeignKey(ss => ss.StudentID);

            modelBuilder.Entity<EnrollmentDegree>()
                .HasOne(ed => ed.Enrollment)
                .WithMany(e => e.EnrollmentDegrees)
                .HasForeignKey(ed => ed.EnrollmentID);

            modelBuilder.Entity<EnrollmentDegree>()
                .HasOne(ed => ed.Degree)
                .WithMany(d => d.EnrollmentDegrees)
                .HasForeignKey(ed => ed.DegreeID);

            modelBuilder.Entity<SessionRating>()
                .HasOne(sr => sr.Session)
                .WithMany(s => s.SessionRatings)
                .HasForeignKey(sr => sr.SessionID);

            modelBuilder.Entity<SessionRating>()
                .HasOne(sr => sr.Rating)
                .WithMany(r => r.SessionRatings)
                .HasForeignKey(sr => sr.RatingID);
        }
    }
}