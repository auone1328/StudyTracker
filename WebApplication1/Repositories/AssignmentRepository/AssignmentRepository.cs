using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebApplication1.Models.Contexts;
using WebApplication1.Models.Entities;
using static WebApplication1.Models.Entities.StudentAssignment;

namespace WebApplication1.Repositories.AssignmentRepository
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly StudyTrackerDbContext _context;
        private readonly IMapper _mapper;

        public AssignmentRepository(StudyTrackerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Assignment> GetAssignmentById(int assignmentId) 
        {
            var assignment = await _context.Assignments.FirstOrDefaultAsync(a => a.AssignmentId == assignmentId);

            return assignment;
        }

        public async Task AddAssignment(Assignment assignment) 
        {
            await _context.Assignments.AddAsync(assignment);
            await _context.SaveChangesAsync();

            var students = await _context.StudentsCourse
                .Where(sc => sc.CourseId == assignment.CourseId)
                .Include(sc => sc.Student)
                .Select(sc => sc.Student)
                .ToListAsync();


            foreach (var student in students)
            {
                await _context.StudentsAssignment.AddAsync(new StudentAssignment
                {
                    Status = AssignmentStatus.NotStarted,
                    StudentId = student.UserId,
                    AssignmentId = assignment.AssignmentId
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<StudentAssignment>> GetAssignmentsByStudentId(int studentId, int courseId) 
        {
            var assignments = await _context.StudentsAssignment
                        .Where(sa => sa.StudentId == studentId && sa.Assignment.CourseId == courseId)
                        .Include(sa => sa.Assignment)
                        .ToListAsync();

            return assignments;
        }

        public async Task<List<Assignment>> GetAssignmentsByCourseId(int courseId) 
        {
            var assignments = await _context.Assignments
                        .Where(a => a.CourseId == courseId)
                        .ToListAsync();

            return assignments;
        }

        public async Task UpdateStatus(int studentId, int assignmentId, AssignmentStatus status) 
        {
            
            var assignment = await _context.StudentsAssignment
                .Include(sa => sa.Assignment)
                .FirstOrDefaultAsync(sa =>
                    sa.AssignmentId == assignmentId &&
                    sa.StudentId == studentId);

            assignment.Status = status;
            await _context.SaveChangesAsync();
        }

        public async Task Remove(int assignmentId)
        {
            var assignment = await _context.Assignments.FirstOrDefaultAsync(a => a.AssignmentId == assignmentId);

            if (assignment != null)
            {
                _context.Assignments.Remove(assignment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(Assignment assignment)
        {
            var assignmentToUpdate = await _context.Assignments.FirstOrDefaultAsync(a => a.AssignmentId == assignment.AssignmentId);

            if (assignmentToUpdate != null)
            {
                assignmentToUpdate.Title = assignment.Title;
                assignmentToUpdate.Description = assignment.Description;
                assignmentToUpdate.Deadline = assignment.Deadline.ToUniversalTime();
            }

            await _context.SaveChangesAsync();
        }
    }
}
