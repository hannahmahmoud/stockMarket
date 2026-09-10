using backend.Models;

namespace backend.Repository
{
    public interface IUserRepo
    {
        Task<User?> GetById(int id);

        Task<User?> GetByEmail(string email);

        Task<List<User>> GetAll();

        Task<User> Create(User user);

        Task<bool> AddRoleToUser(int userId, int roleId);

        Task<bool> RemoveRoleFromUser(int userId, int roleId);

        Task<List<Role>> GetUserRoles(int userId);

        Task<List<Permission>> GetUserPermissions(int userId);
    }
}