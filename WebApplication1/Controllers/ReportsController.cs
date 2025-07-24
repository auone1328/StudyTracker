using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static WebApplication1.Models.Entities.StudentAssignment;
using System.Security.Claims;
using System;
using WebApplication1.Models.Entities;
using WebApplication1.Models.Contexts;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Services;

[Authorize]
public class ReportsController : Controller
{
    private readonly StudyTrackerDbContext _context;
    private readonly IReportService _reportService;

    public ReportsController(StudyTrackerDbContext context, IReportService reportService)
    {
        _context = context;
        _reportService = reportService;
    }

    [Authorize(Roles = "student")]
    public async Task<IActionResult> DownloadCourseAssignments(int courseId, string format)
    {
        var studentId = Convert.ToInt32(User.FindFirst("userId")?.Value);

        var data = await _context.StudentsAssignment
            .Include(sa => sa.Assignment).ThenInclude(a => a.Course)
            .Where(sa => sa.StudentId == studentId && sa.Assignment.CourseId == courseId)     
            .ToListAsync();

        return format.ToLower() switch
        {
            "docx" => _reportService.GenerateDocxReport(data, "AssignmentsReport.docx"),
            "xlsx" => _reportService.GenerateXlsxReport(data, "AssignmentsReport.xlsx"),
            _ => BadRequest("Unsupported format")
        };
    }

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DownloadOverdueAssignments(string format)
    {
        var data = await _context.StudentsAssignment
            .Where(sa => sa.Assignment.Deadline.ToUniversalTime() < DateTime.UtcNow.Date && sa.Status != AssignmentStatus.Completed)
            .Include(sa => sa.Student)
            .Include(sa => sa.Assignment)
            .ThenInclude(a => a.Course)
            .ToListAsync();

        return format.ToLower() switch
        {
            "docx" => _reportService.GenerateOverdueDocxReport(data, "OverdueAssignments.docx"),
            "xlsx" => _reportService.GenerateOverdueXlsxReport(data, "OverdueAssignments.xlsx"),
            _ => BadRequest("Unsupported format")
        };
    }
}