using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System.Diagnostics;
using System.Security.Claims;
using WebApplication1.Models;
using WebApplication1.Models.Entities;
using WebApplication1.Repositories.AssignmentRepository;
using WebApplication1.Repositories.CourseRepository;
using WebApplication1.Repositories.TecherRepository;
using WebApplication1.Views.ViewModels;
using static WebApplication1.Models.Entities.StudentAssignment;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class AssignmentsController : Controller
    {
        private readonly IAssignmentRepository _assignmentRepository;

        public AssignmentsController(IAssignmentRepository assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AdminAssignments(int courseId, AssignmentStatus? status, string sortOrder) 
        {
            var assignments = await _assignmentRepository.GetAssignmentsByCourseId(courseId);

            switch (sortOrder)
            {
                case "asc":
                    assignments = assignments.OrderBy(a => a.Deadline).ToList();
                    break;
                case "desc":
                    assignments = assignments.OrderByDescending(a => a.Deadline).ToList();
                    break;
                default:
                    // Без сортировки
                    break;
            }

            var viewModel = new AssignmentIndexViewModel
            {
                CourseId = courseId,
                Assignments = assignments,
                SelectedStatus = status,
                SortOrder = sortOrder
            };

            return View(viewModel);
        }

        [Authorize(Roles = "student")]
        public async Task<IActionResult> Index(int courseId, AssignmentStatus? status, string sortOrder)
        {
            var studentAssignments = await _assignmentRepository.GetAssignmentsByStudentId(Convert.ToInt32(User.FindFirst("userId")?.Value), courseId);
            if (status.HasValue)
            {
                studentAssignments = studentAssignments.Where(a => a.Status == status.Value).ToList();
            }

            switch (sortOrder)
            {
                case "asc":
                    studentAssignments = studentAssignments.OrderBy(sa => sa.Assignment.Deadline).ToList();
                    break;
                case "desc":
                    studentAssignments = studentAssignments.OrderByDescending(sa => sa.Assignment.Deadline).ToList();
                    break;
                default:
                    // Без сортировки
                    break;
            }

            var viewModel = new AssignmentIndexViewModel
            {
                CourseId = courseId,
                StudentAssignments = studentAssignments,
                SelectedStatus = status,
                SortOrder = sortOrder
            };

            return View(viewModel);
        }

        [Authorize(Roles = "student")]
        [HttpPost]
        public async Task<IActionResult> Index(int assignmentId, AssignmentStatus status, int courseId)
        {
            var studentId = Convert.ToInt32(User.FindFirst("userId")?.Value);

            await _assignmentRepository.UpdateStatus(studentId, assignmentId, status);

            var assignments = await _assignmentRepository.GetAssignmentsByStudentId(Convert.ToInt32(User.FindFirst("userId")?.Value), courseId);
            var viewModel = new AssignmentIndexViewModel
            {
                CourseId = courseId,
                StudentAssignments = assignments
            };

            return View(viewModel);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Create(int courseId)
        {
            var model = new Assignment { CourseId = courseId };
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Assignment assignment)
        {
            assignment.Deadline = assignment.Deadline.ToUniversalTime();
            await _assignmentRepository.AddAssignment(assignment);
            return RedirectToAction("Index", "Courses");
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Remove(int assignmentId, int courseId)
        {
            await _assignmentRepository.Remove(assignmentId);

            return RedirectToAction("AdminAssignments", new { courseId });
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Update(int assignmentId, int courseId)
        {
            var assignment = await _assignmentRepository.GetAssignmentById(assignmentId);

            return View(assignment);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Update(Assignment assignment)
        {
            await _assignmentRepository.Update(assignment);

            return RedirectToAction("AdminAssignments", new { assignment.CourseId });
        }
    }
}
