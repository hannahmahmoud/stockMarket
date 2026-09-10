using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public record CreateRoleDto
    {
        [Required(ErrorMessage = "Role name is required")]
        [StringLength(
            50,
            MinimumLength = 2,
            ErrorMessage = "Role name must be between 2 and 50 characters"
        )]
        public string Name { get; set; } = string.Empty;
    }
}