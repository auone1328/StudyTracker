using WebApplication1.Models.Entities;

namespace WebApplication1.Repositories.TecherRepository
{
    public interface ITeacherRepository
    {
        public Task<List<Teacher>> GetAllTeachers();
    }
}
