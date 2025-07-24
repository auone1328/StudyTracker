using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.Contexts;
using AutoMapper;
using WebApplication1.Models.DTOs;
using WebApplication1.Models.Entities;

namespace WebApplication1.UserRepository.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly StudyTrackerDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(StudyTrackerDbContext context, IMapper mapper) 
        {
            _context = context; 
            _mapper = mapper;   
        }
        public async Task<List<User>> GetAllStudents()
        {
            var students = await _context.Users.Where(u => u.Role == "student").ToListAsync();

            return students;
        }

        public async Task<bool> CheckEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task Add(UserDTO user)
        {
            var userEntity = _mapper.Map<User>(user);

            await _context.Users.AddAsync(userEntity);  
            await _context.SaveChangesAsync();
        }

        public async Task<UserDTO> GetByEmail(string email)
        {
            var userEntity = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

            return _mapper.Map<UserDTO>(userEntity);
        }

        public async Task<UserDTO> GetUserById(int userId)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            return _mapper.Map<UserDTO>(userEntity);
        }

        public async Task SaveRefreshToken(int userId, string refreshToken, DateTime expiryTime) 
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = expiryTime;

            await _context.SaveChangesAsync();
        }
    }
}
