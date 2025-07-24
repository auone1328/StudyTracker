using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using WebApplication1.Models.Contexts;
using WebApplication1.Models.Entities;
using static WebApplication1.Models.Entities.StudentAssignment;

namespace WebApplication1.Repositories.CourseRepository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly StudyTrackerDbContext _context;
        private readonly IMapper _mapper;

        public CourseRepository(StudyTrackerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Course> GetCourseByCourseId(int courseId) 
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == courseId);

            return course;
        }

        public async Task<List<Course>> GetAllCourses() 
        {
            var courses = await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Assignments)
                .Include(c => c.StudentsCourse)
                .ToListAsync();

            return courses;
        }

        public async Task<List<Course>> GetCoursesByStudentId(int studentId)
        {
            var courses = await _context.StudentsCourse
                        .Where(sc => sc.StudentId == studentId)
                        .Include(sc => sc.Course)
                        .ThenInclude(c => c.Teacher)  
                        .Include(sc => sc.Course)
                        .ThenInclude(c => c.Assignments)  
                        .Select(sc => sc.Course)
                        .ToListAsync();

            return courses;
        }

        public async Task AddCourse(Course course) 
        {
           await _context.Courses.AddAsync(course);
           await _context.SaveChangesAsync();
        }      

        public async Task<Course> GetCourseWithStudents(int courseId) 
        {
            var course = await _context.Courses
                        .Include(c => c.StudentsCourse)
                        .ThenInclude(sc => sc.Student)
                        .FirstOrDefaultAsync(c => c.CourseId == courseId);

            return course;
        }
        public async Task AddStudentToCourse(int courseId, List<int> studentIds)
        {

            var courseAssignments = await _context.Assignments
                                        .Where(a => a.CourseId == courseId)
                                        .ToListAsync();

            foreach (var studentId in studentIds)
            {
                var isStudentAsigned = await _context.StudentsCourse
                       .FirstOrDefaultAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);

                if (isStudentAsigned == null) 
                {
                    await _context.StudentsCourse.AddAsync(new StudentCourse
                    {
                        CourseId = courseId,
                        StudentId = studentId
                    });

                    foreach (var assignment in courseAssignments)
                    {
                        await _context.StudentsAssignment.AddAsync(new StudentAssignment
                        {
                            Status = AssignmentStatus.NotStarted,
                            StudentId = studentId,
                            AssignmentId = assignment.AssignmentId
                        });
                    }
                }                
            }
               
            await _context.SaveChangesAsync();
        }

        public async Task Remove(int courseId) 
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == courseId);

            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(Course course) 
        {
            var courseToUpdate = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == course.CourseId);

            if (courseToUpdate != null) 
            {
                courseToUpdate.Title = course.Title;
                courseToUpdate.Description = course.Description;
                courseToUpdate.TeacherId = course.TeacherId;
            }

            await _context.SaveChangesAsync();
        }
    }
}
