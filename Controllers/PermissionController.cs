using backend.DTOs;
using backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/v1/permissions")]
    [Authorize(Roles = "Admin")]
    public class PermissionController : ControllerBase
    {
        private readonly PermissionService permissionService;

        public PermissionController(
            PermissionService permissionService)
        {
            this.permissionService = permissionService;
        }

        [HttpGet]
        public async Task<IActionResult>
            GetPermissions()
        {
            var permissions =
                await permissionService
                    .GetAllPermissions();

            return Ok(permissions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>
            GetPermission(int id)
        {
            var permission =
                await permissionService
                    .GetPermissionById(id);

            if (permission == null)
            {
                return NotFound();
            }

            return Ok(permission);
        }

        [HttpPost]
        public async Task<IActionResult>
            CreatePermission(
                CreatePermissionDto dto)
        {
            try
            {
                var permission =
                    await permissionService
                        .CreatePermission(dto.Name);

                return Ok(permission);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }
}