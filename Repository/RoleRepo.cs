using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class RoleRepo : IRoleRepo
    {
        private readonly ApplicationDbContext dbContext;

        public RoleRepo(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Role>> GetAll()
        {
            return await dbContext.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task<Role?> GetById(int id)
        {
            return await dbContext.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Role?> GetByName(string name)
        {
            return await dbContext.Roles
                .FirstOrDefaultAsync(r => r.Name == name);
        }

        public async Task<Role> Create(Role role)
        {
            await dbContext.Roles.AddAsync(role);

            await dbContext.SaveChangesAsync();

            return role;
        }

        public async Task<bool> AddPermissionToRole(
            int roleId,
            int permissionId)
        {
            var role = await dbContext.Roles
                .FindAsync(roleId);

            if (role == null)
                return false;

            var permission = await dbContext.Permissions
                .FindAsync(permissionId);

            if (permission == null)
                return false;

            var existing =
                await dbContext.RolePermissions
                    .FirstOrDefaultAsync(rp =>
                        rp.RoleId == roleId &&
                        rp.PermissionId == permissionId);

            if (existing != null)
                return false;

            var rolePermission = new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            };

            await dbContext.RolePermissions
                .AddAsync(rolePermission);

            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemovePermissionFromRole(
            int roleId,
            int permissionId)
        {
            var rolePermission =
                await dbContext.RolePermissions
                    .FirstOrDefaultAsync(rp =>
                        rp.RoleId == roleId &&
                        rp.PermissionId == permissionId);

            if (rolePermission == null)
                return false;

            dbContext.RolePermissions
                .Remove(rolePermission);

            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<Permission>> GetRolePermissions(
            int roleId)
        {
            return await dbContext.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.Permission)
                .ToListAsync();
        }
    }
}