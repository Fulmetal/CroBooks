using CroBooks.Domain.Base;
using CroBooks.Domain.Contacts;
using CroBooks.Shared.Dto;

namespace CroBooks.Domain.Clients
{
    public class Client : AuditEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string TaxNumber { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public List<Contact> Contacts { get; set; } = new List<Contact>();

        public Client()
        {
        }

        public Client(ClientDto dto)
        {
            this.Id = dto.Id;
            this.Name = dto.Name;
            this.Email = dto.Email;
            this.Phone = dto.Phone;
            this.Address = dto.Address;
            this.City = dto.City;
            this.PostalCode = dto.PostalCode;
            this.Country = dto.Country;
            this.TaxNumber = dto.TaxNumber;
            this.Note = dto.Note;
        }


        public ClientDto ToDto()
        {
            return new ClientDto
            {
                Id = this.Id,
                Name = this.Name,
                Email = this.Email,
                Phone = this.Phone,
                Address = this.Address,
                PostalCode = this.PostalCode,
                City = this.City,
                Country = this.Country,
                TaxNumber = this.TaxNumber,
                Note = this.Note
            };
        }

        public void UpdateFromDto(Client client, ClientDto dto)
        {
            this.Name = dto.Name;
            this.Email = dto.Email;
            this.Phone = dto.Phone;
            this.Address = dto.Address;
            this.PostalCode = dto.PostalCode;
            this.City = dto.City;
            this.Country = dto.Country;
            this.TaxNumber = dto.TaxNumber;
            this.Note = dto.Note;
        }
    }
}
