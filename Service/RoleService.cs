using backend.Models;
using backend.Repository;

namespace backend.Service
{
    public class RoleService 
    {
        private readonly IRoleRepo roleRepo;

        public RoleService(IRoleRepo roleRepo)
        {
            this.roleRepo = roleRepo;
        }

        public async Task<List<Role>> GetAllRoles()
        {
            return await roleRepo.GetAll();
        }

        public async Task<Role?> GetRoleById(int id)
        {
            return await roleRepo.GetById(id);
        }

        public async Task<Role> CreateRole(
            string name)
        {
            var existingRole =
                await roleRepo.GetByName(name);

            if (existingRole != null)
            {
                throw new Exception(
                    "Role already exists"
                );
            }

            var role = new Role
            {
                Name = name
            };

            return await roleRepo.Create(role);
        }

        public async Task<bool> AddPermission(
            int roleId,
            int permissionId)
        {
            return await roleRepo.AddPermissionToRole(
                roleId,
                permissionId
            );
        }

        public async Task<bool> RemovePermission(
            int roleId,
            int permissionId)
        {
            return await roleRepo.RemovePermissionFromRole(
                roleId,
                permissionId
            );
        }

        public async Task<List<Permission>> GetPermissions(
            int roleId)
        {
            return await roleRepo.GetRolePermissions(
                roleId
            );
        }
    }
}