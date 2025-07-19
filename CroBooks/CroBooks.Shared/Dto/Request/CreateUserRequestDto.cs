using System.ComponentModel.DataAnnotations;

namespace CroBooks.Shared.Dto.Request
{
    public class CreateUserRequestDto
    {
        public CreateUserRequestDto(int roleId)
        {
            RoleId = roleId;
        }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;        
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int RoleId { get; set; } = 2;
    }
}
