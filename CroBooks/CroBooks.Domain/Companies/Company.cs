using CroBooks.Domain.Base;
using CroBooks.Shared.Dto;
using System.Runtime.CompilerServices;

namespace CroBooks.Domain.Companies
{
    public class Company : AuditEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string TaxNumber { get; set; } = string.Empty;
        public string IBAN { get; set; } = string.Empty;
        public string RegisteredActivity { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public bool IsDefault { get; set; }
        
        public Company()
        {
        }

        public Company(CompanyDto dto)
        {
            this.Id = dto.Id;
            this.Name = dto.Name;
            this.Address = dto.Address;
            this.PostalCode = dto.PostalCode;
            this.City = dto.City;
            this.Country = dto.Country;
            this.TaxNumber = dto.TaxNumber;
            this.IBAN = dto.IBAN;
            this.RegisteredActivity = dto.RegisteredActivity;
            this.RegistrationDate = dto.RegistrationDate;
            this.IsDefault = dto.IsDefault;
        }


        public CompanyDto ToDto()
        {
            return new CompanyDto
            {
                Id = this.Id,
                Name = this.Name,
                Address = this.Address,
                PostalCode = this.PostalCode,
                City = this.City,
                Country = this.Country,
                TaxNumber = this.TaxNumber,
                IBAN = this.IBAN,
                RegisteredActivity = this.RegisteredActivity,
                RegistrationDate = this.RegistrationDate,
                IsDefault = this.IsDefault
            };
        }
    }
}
