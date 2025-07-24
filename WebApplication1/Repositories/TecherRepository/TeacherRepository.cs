using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.Contexts;
using WebApplication1.Models.Entities;

namespace WebApplication1.Repositories.TecherRepository
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly StudyTrackerDbContext _context;
        private readonly IMapper _mapper;

        public TeacherRepository(StudyTrackerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Teacher>> GetAllTeachers() 
        {
            var teachers = await _context.Teachers.ToListAsync();

            return teachers;
        }
    }
}
