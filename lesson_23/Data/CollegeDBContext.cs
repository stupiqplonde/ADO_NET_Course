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
        public DbSet<Teacher> Teachers => Set<Teacher>();
        public DbSet<Subject> Subjects => Set<Subject>();
        public DbSet<Grade> Grades => Set<Grade>();

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

            modelBuilder.Entity<Teacher>(e =>
            {
                e.ToTable("Teachers");
                e.HasKey(t => t.TeacherId);
                e.Property(t => t.FirstName).HasMaxLength(50).IsRequired();
                e.Property(t => t.LastName).HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<Subject>(e =>
            {
                e.ToTable("Subjects");
                e.HasKey(s => s.SubjectId);
                e.Property(s => s.SubjectName).HasMaxLength(100).IsRequired();

                e.HasOne(s => s.Teacher)
                    .WithMany(t => t.Subjects)
                    .HasForeignKey(s => s.TeacherId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Grade>(e =>
            {
                e.ToTable("Grades");
                e.HasKey(g => g.GradeId);
                e.Property(g => g.Score).IsRequired();
                e.Property(g => g.GradeDate).HasColumnType("date").IsRequired();

                e.HasOne(g => g.Student)
                    .WithMany(s => s.Grades)
                    .HasForeignKey(g => g.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(g => g.Subject)
                    .WithMany(s => s.Grades)
                    .HasForeignKey(g => g.SubjectId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
