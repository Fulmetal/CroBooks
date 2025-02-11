using System.ComponentModel.DataAnnotations;

namespace CroBooks.Shared.Dto
{
    public class CompanyDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public string PostalCode { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public string Country { get; set; } = string.Empty;
        [Required]
        public string TaxNumber { get; set; } = string.Empty;
        [Required]
        public string IBAN { get; set; } = string.Empty;
        [Required]
        public string RegisteredActivity { get; set; } = string.Empty;
        [Required]
        public DateTime? RegistrationDate { get; set; } = DateTime.Now;
        public bool IsDefault { get; set; }
    }
}
