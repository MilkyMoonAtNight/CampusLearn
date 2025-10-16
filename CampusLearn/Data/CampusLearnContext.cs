using CampusLearn.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CampusLearn.Data
{
    public class CampusLearnContext : DbContext
    {
        public CampusLearnContext(DbContextOptions<CampusLearnContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Tutors> Tutor { get; set; }
        public DbSet<TopicMod> Modules { get; set; }
        
    }
}
