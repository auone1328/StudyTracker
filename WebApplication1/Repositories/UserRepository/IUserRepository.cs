
using WebApplication1.Models.DTOs;
using WebApplication1.Models.Entities;

namespace WebApplication1.UserRepository.Repositories
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAllStudents();
        public Task<bool> CheckEmail(string email);
        public Task Add(UserDTO user);
        public Task<UserDTO> GetByEmail(string email);
        public Task SaveRefreshToken(int userId, string refreshToken, DateTime expiryTime);
        public Task<UserDTO> GetUserById(int userId);
    }
}