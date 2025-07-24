using WebApplication1.Models.Entities;
using static WebApplication1.Models.Entities.StudentAssignment;

namespace WebApplication1.Repositories.AssignmentRepository
{
    public interface IAssignmentRepository
    {
        public Task<Assignment> GetAssignmentById(int assignmentId);
        public Task AddAssignment(Assignment assignment);
        public Task<List<StudentAssignment>> GetAssignmentsByStudentId(int studentId, int courseId);
        public Task UpdateStatus(int studentId, int assignmentId, AssignmentStatus status);
        public Task<List<Assignment>> GetAssignmentsByCourseId(int courseId);
        public Task Remove(int assignmentId);
        public Task Update(Assignment assignment);
    }
}
