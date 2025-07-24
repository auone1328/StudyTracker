using Microsoft.AspNetCore.Mvc.Rendering;
using static WebApplication1.Models.Entities.StudentAssignment;
using WebApplication1.Models.Entities;

namespace WebApplication1.Views.ViewModels
{
    public class AssignmentIndexViewModel
    {
        public int CourseId { get; set; }
        public List<StudentAssignment> StudentAssignments { get; set; }
        public List<Assignment> Assignments { get; set; }
        public AssignmentStatus? SelectedStatus { get; set; }
        public string SortOrder { get; set; } // "asc", "desc", or ""
        public List<SelectListItem> StatusOptions { get; } = new List<SelectListItem>
        {
            new SelectListItem("Все", ""),
            new SelectListItem("Не начато", AssignmentStatus.NotStarted.ToString()),
            new SelectListItem("В работе", AssignmentStatus.InProgress.ToString()),
            new SelectListItem("Завершено", AssignmentStatus.Completed.ToString()),
        };
    }
}
