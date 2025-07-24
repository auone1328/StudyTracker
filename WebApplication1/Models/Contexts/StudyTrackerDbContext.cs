using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApplication1.Models.Entities;
using WebApplication1.Models.Entities.Configurations;

namespace WebApplication1.Models.Contexts
{
    //dotnet ef migrations add Initial --project Persistence --startup-project CarRental
    // dotnet ef database update --project Persistence --startup-project CarRental
    public class StudyTrackerDbContext : DbContext
    {

        public StudyTrackerDbContext(DbContextOptions<StudyTrackerDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentAssignment> StudentsAssignment { get; set; }
        public DbSet<StudentCourse> StudentsCourse { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new AssignmentConfiguration());
            modelBuilder.ApplyConfiguration(new CourseConfiguration());
            modelBuilder.ApplyConfiguration(new StudentAssignmentConfiguration());
            modelBuilder.ApplyConfiguration(new StudentCourseConfiguration());
            modelBuilder.ApplyConfiguration(new TeacherConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
