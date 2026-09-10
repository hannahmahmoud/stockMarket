using backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;

        public UserController(
            UserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users =
                await userService.GetAllUsers();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(
            int id)
        {
            var user =
                await userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("{userId}/roles/{roleId}")]
        public async Task<IActionResult> AddRole(
            int userId,
            int roleId)
        {
            var result =
                await userService.AddRole(
                    userId,
                    roleId
                );

            if (!result)
            {
                return BadRequest(
                    "User or role does not exist, or role already assigned."
                );
            }

            return Ok(new
            {
                message =
                    "Role added to user"
            });
        }

        [HttpDelete("{userId}/roles/{roleId}")]
        public async Task<IActionResult> RemoveRole(
            int userId,
            int roleId)
        {
            var result =
                await userService.RemoveRole(
                    userId,
                    roleId
                );

            if (!result)
            {
                return NotFound(
                    "User role not found."
                );
            }

            return Ok(new
            {
                message =
                    "Role removed from user"
            });
        }

        [HttpGet("{userId}/roles")]
        public async Task<IActionResult> GetRoles(
            int userId)
        {
            var roles =
                await userService.GetUserRoles(
                    userId
                );

            return Ok(roles);
        }

        [HttpGet("{userId}/permissions")]
        public async Task<IActionResult> GetPermissions(
            int userId)
        {
            var permissions =
                await userService.GetUserPermissions(
                    userId
                );

            return Ok(permissions);
        }
    }
}