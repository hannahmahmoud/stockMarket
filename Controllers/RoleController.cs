using backend.DTOs;
using backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/v1/roles")]
    [Authorize(Roles = "Admin")]
    public class RoleController : ControllerBase
    {
        private readonly RoleService roleService;

        public RoleController(
            RoleService roleService)
        {
            this.roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles =
                await roleService.GetAllRoles();

            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(
            int id)
        {
            var role =
                await roleService.GetRoleById(id);

            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(
            CreateRoleDto dto)
        {
            try
            {
                var role =
                    await roleService.CreateRole(
                        dto.Name
                    );

                return Ok(role);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("{roleId}/permissions/{permissionId}")]
        public async Task<IActionResult> AddPermission(
            int roleId,
            int permissionId)
        {
            var result =
                await roleService.AddPermission(
                    roleId,
                    permissionId
                );

            if (!result)
            {
                return BadRequest(
                    "Role or permission does not exist, or permission already assigned."
                );
            }

            return Ok(new
            {
                message =
                    "Permission added to role"
            });
        }

        [HttpDelete("{roleId}/permissions/{permissionId}")]
        public async Task<IActionResult> RemovePermission(
            int roleId,
            int permissionId)
        {
            var result =
                await roleService.RemovePermission(
                    roleId,
                    permissionId
                );

            if (!result)
            {
                return NotFound();
            }

            return Ok(new
            {
                message =
                    "Permission removed from role"
            });
        }

        [HttpGet("{roleId}/permissions")]
        public async Task<IActionResult> GetPermissions(
            int roleId)
        {
            var permissions =
                await roleService.GetPermissions(
                    roleId
                );

            return Ok(permissions);
        }
    }
}