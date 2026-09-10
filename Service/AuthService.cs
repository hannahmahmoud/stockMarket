using backend.DTOs;
using backend.Models;
using backend.Repository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Service
{
    public class AuthService 
    {
        private readonly IUserRepo userRepo;
        private readonly IRoleRepo roleRepo;
        private readonly IConfiguration configuration;

        public AuthService(
            IUserRepo userRepo,
            IRoleRepo roleRepo,
            IConfiguration configuration)
        {
            this.userRepo = userRepo;
            this.roleRepo = roleRepo;
            this.configuration = configuration;
        }

        public async Task<string> Register(
            RegisterDto registerDto)
        {
            var existingUser =
                await userRepo.GetByEmail(registerDto.Email);

            if (existingUser != null)
            {
                throw new Exception(
                    "Email already exists"
                );
            }

            var passwordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    registerDto.Password
                );

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = passwordHash
            };

            await userRepo.Create(user);

            // Every new user gets the User role
            var defaultRole =
                await roleRepo.GetByName("User");

            if (defaultRole != null)
            {
                await userRepo.AddRoleToUser(
                    user.Id,
                    defaultRole.Id
                );
            }

            return await GenerateJwtToken(user.Id);
        }

        public async Task<string?> Login(
            LoginDto loginDto)
        {
            var user =
                await userRepo.GetByEmail(loginDto.Email);

            if (user == null)
            {
                return null;
            }

            var passwordCorrect =
                BCrypt.Net.BCrypt.Verify(
                    loginDto.Password,
                    user.PasswordHash
                );

            if (!passwordCorrect)
            {
                return null;
            }

            return await GenerateJwtToken(user.Id);
        }

        private async Task<string> GenerateJwtToken(
            int userId)
        {
            var user =
                await userRepo.GetById(userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            var roles =
                await userRepo.GetUserRoles(userId);

            var permissions =
                await userRepo.GetUserPermissions(userId);

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.Id.ToString()
                ),

                new Claim(
                    ClaimTypes.Name,
                    user.Username
                ),

                new Claim(
                    ClaimTypes.Email,
                    user.Email
                )
            };

            // Add roles
            foreach (var role in roles)
            {
                claims.Add(
                    new Claim(
                        ClaimTypes.Role,
                        role.Name
                    )
                );
            }

            // Add permissions
            foreach (var permission in permissions)
            {
                claims.Add(
                    new Claim(
                        "Permission",
                        permission.Name
                    )
                );
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    configuration["Jwt:Key"]!
                )
            );

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256
                );

            var token = new JwtSecurityToken(
                issuer:
                    configuration["Jwt:Issuer"],

                audience:
                    configuration["Jwt:Audience"],

                claims: claims,

                expires:
                    DateTime.UtcNow.AddHours(2),

                signingCredentials:
                    credentials
            );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}