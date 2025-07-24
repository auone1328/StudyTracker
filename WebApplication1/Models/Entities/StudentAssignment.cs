namespace WebApplication1.Models.Entities
{
    public class StudentAssignment
    {
        public enum AssignmentStatus
        {
            NotStarted,
            InProgress,
            Completed
        }

        public int StudentAssignmentId { get; set; }
        public AssignmentStatus Status { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int StudentId { get; set; }
        public int AssignmentId { get; set; }

        public User Student { get; set; }
        public Assignment Assignment { get; set; }
    }

}
