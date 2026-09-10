using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public record CreatePermissionDto
    {
        [Required(ErrorMessage = "Permission name is required")]
        [StringLength(
            100,
            MinimumLength = 2,
            ErrorMessage = "Permission name must be between 2 and 100 characters"
        )]
        public string Name { get; set; } = string.Empty;
    }
}