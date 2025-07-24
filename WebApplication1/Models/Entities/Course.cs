namespace WebApplication1.Models.Entities
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public ICollection<Assignment> Assignments { get; set; }
        public ICollection<StudentCourse> StudentsCourse { get; set; }
    }
}
