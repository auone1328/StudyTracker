using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebApplication1.Models.Entities;
using WebApplication1.Repositories.CourseRepository;
using WebApplication1.Repositories.TecherRepository;
using WebApplication1.UserRepository.Repositories;

namespace WebApplication1.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IUserRepository _userRepository;

        public CoursesController(ICourseRepository courseRepository, ITeacherRepository teacherRepository, IUserRepository userRepository)
        {
            _courseRepository = courseRepository;
            _teacherRepository = teacherRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            List<Course> courses;
            if (User.IsInRole("admin"))
            {
                courses = await _courseRepository.GetAllCourses();
                return View(courses);
            }

            courses = await _courseRepository.GetCoursesByStudentId(Convert.ToInt32(User.FindFirst("userId")?.Value));
            return View(courses);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Teachers = await _teacherRepository.GetAllTeachers();
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Course course)
        {
            await _courseRepository.AddCourse(course);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> AssignStudents(int courseId)
        {
            var course = await _courseRepository.GetCourseWithStudents(courseId);

            var students = await _userRepository.GetAllStudents();

            ViewBag.Course = course;
            ViewBag.Students = students;
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AssignStudents(int courseId, List<int> studentIds)
        {
            await _courseRepository.AddStudentToCourse(courseId, studentIds);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Remove(int courseId)   
        {
            await _courseRepository.Remove(courseId);

            var courses = await _courseRepository.GetAllCourses();
            return RedirectToAction("Index", courses);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Update(int courseId)
        {
            ViewBag.Teachers = await _teacherRepository.GetAllTeachers();
            var course = await _courseRepository.GetCourseByCourseId(courseId);

            return View(course);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Update(Course course)
        {
            await _courseRepository.Update(course);

            return RedirectToAction("Index");
        }
    }
}
