using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class PermissionRepo : IPermissionRepo
    {
        private readonly ApplicationDbContext dbContext;

        public PermissionRepo(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Permission>> GetAll()
        {
            return await dbContext.Permissions
                .ToListAsync();
        }

        public async Task<Permission?> GetById(int id)
        {
            return await dbContext.Permissions
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Permission?> GetByName(
            string name)
        {
            return await dbContext.Permissions
                .FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<Permission> Create(
            Permission permission)
        {
            await dbContext.Permissions
                .AddAsync(permission);

            await dbContext.SaveChangesAsync();

            return permission;
        }
    }
}