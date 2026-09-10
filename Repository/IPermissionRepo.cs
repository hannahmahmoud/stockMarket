using backend.Models;

namespace backend.Repository
{
    public interface IPermissionRepo
    {
        Task<List<Permission>> GetAll();

        Task<Permission?> GetById(int id);

        Task<Permission?> GetByName(string name);

        Task<Permission> Create(Permission permission);
    }
}