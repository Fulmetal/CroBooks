﻿using System.ComponentModel.DataAnnotations;

namespace CroBooks.Shared.Dto.Request
{
    public class CreateUserRequestDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
