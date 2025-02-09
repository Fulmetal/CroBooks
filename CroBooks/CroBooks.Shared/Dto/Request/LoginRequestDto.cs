using System.ComponentModel.DataAnnotations;

namespace CroBooks.Shared.Dto.Request
{
    public class LoginRequestDto
    {
        [Required]
        public string UsernameOrEmail { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
