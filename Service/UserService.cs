using backend.Models;
using backend.Repository;

namespace backend.Service
{
    public class UserService 
    {
        private readonly IUserRepo userRepo;

        public UserService(IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await userRepo.GetAll();
        }

        public async Task<User?> GetUserById(int id)
        {
            return await userRepo.GetById(id);
        }

        public async Task<bool> AddRole(
            int userId,
            int roleId)
        {
            return await userRepo.AddRoleToUser(
                userId,
                roleId
            );
        }

        public async Task<bool> RemoveRole(
            int userId,
            int roleId)
        {
            return await userRepo.RemoveRoleFromUser(
                userId,
                roleId
            );
        }

        public async Task<List<Role>> GetUserRoles(
            int userId)
        {
            return await userRepo.GetUserRoles(userId);
        }

        public async Task<List<Permission>> GetUserPermissions(
            int userId)
        {
            return await userRepo.GetUserPermissions(
                userId
            );
        }
    }
}