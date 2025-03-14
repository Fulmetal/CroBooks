using CroBooks.Domain.Base;
using CroBooks.Domain.Clients;
using CroBooks.Shared.Dto;

namespace CroBooks.Domain.Contacts
{
    public class Contact : AuditEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public int? ClientId { get; set; }
        public Client? Client { get; set; }

        public Contact()
        {
        }

        public Contact(ContactDto dto)
        {
            this.Id = dto.Id;
            this.Name = dto.Name;
            this.Email = dto.Email;
            this.Address = dto.Address;
            this.City = dto.City;
            this.PostalCode = dto.PostalCode;
            this.Country = dto.Country;
            this.Note = dto.Note;
        }


        public ContactDto ToDto()
        {
            return new ContactDto
            {
                Id = this.Id,
                Name = this.Name,
                Email = this.Email,
                Address = this.Address,
                PostalCode = this.PostalCode,
                City = this.City,
                Country = this.Country,
                Note = this.Note
            };
        }

        public void UpdateFromDto(Contact contact, ContactDto dto)
        {
            this.Name = dto.Name;
            this.Email = dto.Email;
            this.Address = dto.Address;
            this.PostalCode = dto.PostalCode;
            this.City = dto.City;
            this.Country = dto.Country;
            this.Note = dto.Note;
        }
    }
}
