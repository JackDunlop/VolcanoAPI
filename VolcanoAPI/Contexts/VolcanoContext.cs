using Microsoft.EntityFrameworkCore;
using VolcanoAPI.Data;

namespace VolcanoAPI.Contexts
{
    public class VolcanoContext : DbContext
    {
        public DbSet<VolcanoData> Volcanoes { get; set; }

        public VolcanoContext(DbContextOptions<VolcanoContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VolcanoData>(entity =>
            {
                entity.ToTable("data");
                entity.HasKey(e => e.id);
            });
        }
    }
}
