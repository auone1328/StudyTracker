namespace WebApplication1.Models.Entities
{
    public class Assignment
    {
        public int AssignmentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public int CourseId { get; set; }

        public Course Course { get; set; }
        public ICollection<StudentAssignment> StudentsAssignment { get; set; }
    }
}
