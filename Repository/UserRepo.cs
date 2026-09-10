using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext dbContext;

        public UserRepo(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<User?> GetById(int id)
        {
            return await dbContext.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<User>> GetAll()
        {
            return await dbContext.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .ToListAsync();
        }

        public async Task<User> Create(User user)
        {
            await dbContext.Users.AddAsync(user);

            await dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<bool> AddRoleToUser(
            int userId,
            int roleId)
        {
            var user = await dbContext.Users
                .FindAsync(userId);

            if (user == null)
                return false;

            var role = await dbContext.Roles
                .FindAsync(roleId);

            if (role == null)
                return false;

            var existingUserRole =
                await dbContext.UserRoles
                    .FirstOrDefaultAsync(ur =>
                        ur.UserId == userId &&
                        ur.RoleId == roleId);

            if (existingUserRole != null)
                return false;

            var userRole = new UserRole
            {
                UserId = userId,
                RoleId = roleId
            };

            await dbContext.UserRoles.AddAsync(userRole);

            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveRoleFromUser(
            int userId,
            int roleId)
        {
            var userRole =
                await dbContext.UserRoles
                    .FirstOrDefaultAsync(ur =>
                        ur.UserId == userId &&
                        ur.RoleId == roleId);

            if (userRole == null)
                return false;

            dbContext.UserRoles.Remove(userRole);

            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<Role>> GetUserRoles(int userId)
        {
            return await dbContext.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role)
                .ToListAsync();
        }

        public async Task<List<Permission>> GetUserPermissions(
            int userId)
        {
            return await dbContext.UserRoles
                .Where(ur => ur.UserId == userId)
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => rp.Permission)
                .Distinct()
                .ToListAsync();
        }
    }
}