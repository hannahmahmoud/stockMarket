using backend.Models;
using backend.Repository;

namespace backend.Service
{
    public class PermissionService 
    {
        private readonly IPermissionRepo permissionRepo;

        public PermissionService(
            IPermissionRepo permissionRepo)
        {
            this.permissionRepo = permissionRepo;
        }

        public async Task<List<Permission>>
            GetAllPermissions()
        {
            return await permissionRepo.GetAll();
        }

        public async Task<Permission?>
            GetPermissionById(int id)
        {
            return await permissionRepo.GetById(id);
        }

        public async Task<Permission>
            CreatePermission(string name)
        {
            var existing =
                await permissionRepo.GetByName(name);

            if (existing != null)
            {
                throw new Exception(
                    "Permission already exists"
                );
            }

            var permission = new Permission
            {
                Name = name
            };

            return await permissionRepo.Create(
                permission
            );
        }
    }
}