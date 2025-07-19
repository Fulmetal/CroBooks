using System.ComponentModel.DataAnnotations;
using CroBooks.Domain.Base;
using CroBooks.Shared.Dto;

namespace CroBooks.Domain.Companies
{
    public sealed class Company : AuditEntity<int>
    {
        [StringLength(300)]
        public string Name { get; set; } = string.Empty;
        [StringLength(300)]
        public string Address { get; set; } = string.Empty;
        [StringLength(100)]
        public string PostalCode { get; set; } = string.Empty;
        [StringLength(300)]
        public string City { get; set; } = string.Empty;
        [StringLength(300)]
        public string Country { get; set; } = string.Empty;
        [StringLength(100)]
        public string TaxNumber { get; set; } = string.Empty;
        [StringLength(100)]
        public string Iban { get; set; } = string.Empty;
        [StringLength(300)]
        public string RegisteredActivity { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public bool IsDefault { get; set; }
        
        public Company()
        {
        }

        public Company(CompanyDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            Address = dto.Address;
            PostalCode = dto.PostalCode;
            City = dto.City;
            Country = dto.Country;
            TaxNumber = dto.TaxNumber;
            Iban = dto.Iban;
            RegisteredActivity = dto.RegisteredActivity;
            if (dto.RegistrationDate != null) RegistrationDate = dto.RegistrationDate.Value.ToUniversalTime();
            IsDefault = dto.IsDefault;
        }


        public CompanyDto ToDto()
        {
            return new CompanyDto(id: this.Id, name: this.Name, address: this.Address, postalCode: this.PostalCode,
                city: this.City, country: this.Country, taxNumber: this.TaxNumber, iban: this.Iban,
                registeredActivity: this.RegisteredActivity, registrationDate: this.RegistrationDate,
                isDefault: this.IsDefault);
        }

        public static void UpdateFromDto(Company company, CompanyDto dto)
        {
            company.Name = dto.Name;
            company.Address = dto.Address;
            company.PostalCode = dto.PostalCode;
            company.City = dto.City;
            company.Country = dto.Country;
            company.TaxNumber = dto.TaxNumber;
            company.Iban = dto.Iban;
            company.RegisteredActivity = dto.RegisteredActivity;
            company.RegistrationDate = dto.RegistrationDate.HasValue ? dto.RegistrationDate.Value.ToUniversalTime() : company.RegistrationDate;
            company.IsDefault = dto.IsDefault;
        }
    }
}
