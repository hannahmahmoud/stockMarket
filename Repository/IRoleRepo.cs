using backend.Models;

namespace backend.Repository
{
    public interface IRoleRepo
    {
        Task<List<Role>> GetAll();

        Task<Role?> GetById(int id);

        Task<Role?> GetByName(string name);

        Task<Role> Create(Role role);

        Task<bool> AddPermissionToRole(
            int roleId,
            int permissionId);

        Task<bool> RemovePermissionFromRole(
            int roleId,
            int permissionId);

        Task<List<Permission>> GetRolePermissions(int roleId);
    }
}