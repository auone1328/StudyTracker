using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.Entities;

namespace WebApplication1.Services
{
    public interface IReportService
    {
        FileResult GenerateDocxReport(List<StudentAssignment> data, string fileName);
        FileResult GenerateXlsxReport(List<StudentAssignment> data, string fileName);
        public FileResult GenerateOverdueDocxReport(List<StudentAssignment> data, string fileName);
        public FileResult GenerateOverdueXlsxReport(List<StudentAssignment> data, string fileName);
    }
}
