using System.ComponentModel.DataAnnotations;

namespace CroBooks.Shared.Dto
{
    public class CompanyDto
    {
        public CompanyDto()
        {
        }

        public CompanyDto(bool isDefault)
        {
            IsDefault = isDefault;
        }

        public CompanyDto(int id, string name, string address, string postalCode, string city, string country, string taxNumber, string iban, string registeredActivity, DateTime? registrationDate, bool isDefault) : this()
        {
            Id = id;
            Name = name;
            Address = address;
            PostalCode = postalCode;
            City = city;
            Country = country;
            TaxNumber = taxNumber;
            Iban = iban;
            RegisteredActivity = registeredActivity;
            RegistrationDate = registrationDate;
            IsDefault = isDefault;
        }

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
        public string Iban { get; set; } = string.Empty;
        [Required]
        public string RegisteredActivity { get; set; } = string.Empty;
        [Required]
        public DateTime? RegistrationDate { get; set; } = DateTime.Now;
        public bool IsDefault { get; set; }
    }
}
