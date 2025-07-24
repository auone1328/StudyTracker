namespace WebApplication1.Models.Entities
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
        public IEnumerable<Course> Courses { get; set; }
    }
}
