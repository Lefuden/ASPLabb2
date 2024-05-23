using ASPLabb2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASPLabb2.Data;
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentCourse>().HasKey(sc => new { sc.StudentId, sc.CourseId });
        modelBuilder.Entity<TeacherCourse>().HasKey(tc => new { tc.TeacherId, tc.CourseId });
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<StudentCourse> StudentCourses { get; set; }
    public DbSet<TeacherCourse> TeachersCourses { get; set; }
}
