using CroBooks.Domain.Base;
using CroBooks.Domain.Roles;
using CroBooks.Shared.Dto;

namespace CroBooks.Domain.Users
{
    public class User : AuditEntity<int>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public int RoleId { get; set; }
        public Role? Role { get; set; }

        public User()
        {
            
        }

        public User(UserDto dto)
        {
            this.Id = dto.Id;
            this.FirstName = dto.FirstName;
            this.LastName = dto.LastName;
            this.Username = dto.Username;
            this.Email = dto.Email;
        }

        public UserDto ToDto()
        {
            return new UserDto
            {
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Username = this.Username,
                Email = this.Email
            };
        }
    }
}
