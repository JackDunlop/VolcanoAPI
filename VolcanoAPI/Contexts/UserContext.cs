using Microsoft.EntityFrameworkCore;
using VolcanoAPI.Data;

namespace VolcanoAPI.Contexts
{
    public class UserContext : DbContext
    {
        public DbSet<UserData> Users { get; set; }

        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserData>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.id);
            });
        }
    }
}


