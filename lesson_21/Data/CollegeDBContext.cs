using Microsoft.EntityFrameworkCore;
using College_DB.Models;

namespace WebApplication1.Data
{
    public class CollegeDBContext : DbContext
    {
        public CollegeDBContext(DbContextOptions<CollegeDBContext> options)
            :base(options){ }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<Group> Groups => Set<Group>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(e =>
            {
                e.ToTable("Students");
                e.HasKey(s => s.StudentId);
                e.Property(s => s.FirstName).HasMaxLength(50).IsRequired();
                e.Property(s => s.LastName).HasMaxLength(50).IsRequired();

                e.HasOne(s => s.Group)
                    .WithMany(g => g.Students)
                    .HasForeignKey(s => s.GroupId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Group>(e =>
            {
                e.ToTable("Groups");
                e.HasKey(g => g.GroupId);
                e.Property(s => s.GroupName).HasMaxLength(20).IsRequired();
            });
        }
    }
}
