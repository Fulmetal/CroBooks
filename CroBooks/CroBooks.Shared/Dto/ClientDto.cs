﻿using System.ComponentModel.DataAnnotations;

namespace CroBooks.Shared.Dto
{
    public class ClientDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Phone { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public string PostalCode { get; set; } = string.Empty;
        [Required]
        public string Country { get; set; } = string.Empty;
        [Required]
        public string TaxNumber { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
    }
}
