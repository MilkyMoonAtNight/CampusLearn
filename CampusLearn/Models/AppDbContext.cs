using Microsoft.EntityFrameworkCore;
using CampusLearn.Models; // adjust if needed

namespace CampusLearn.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ChatSession> ChatSessions { get; set; }
        public DbSet<MessageUser> MessageUsers { get; set; }
    }
}