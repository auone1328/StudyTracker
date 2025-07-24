using WebApplication1.Models.Entities;

namespace WebApplication1.Repositories.CourseRepository
{
    public interface ICourseRepository
    {
        public Task<Course> GetCourseByCourseId(int courseId);
        public Task<List<Course>> GetCoursesByStudentId(int studentId);
        public Task<List<Course>> GetAllCourses();
        public Task AddCourse(Course course);
        public Task<Course> GetCourseWithStudents(int courseId);
        public Task AddStudentToCourse(int courseId, List<int> studentIds);
        public Task Remove(int courseId);
        public Task Update(Course course);
    }
}
