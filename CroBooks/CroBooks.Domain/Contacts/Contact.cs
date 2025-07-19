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
            Id = dto.Id;
            Name = dto.Name;
            Email = dto.Email;
            Address = dto.Address;
            City = dto.City;
            PostalCode = dto.PostalCode;
            Country = dto.Country;
            Note = dto.Note;
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

        public static void UpdateFromDto(Contact contact, ContactDto dto)
        {
            contact.Name = dto.Name;
            contact.Email = dto.Email;
            contact.Address = dto.Address;
            contact.PostalCode = dto.PostalCode;
            contact.City = dto.City;
            contact.Country = dto.Country;
            contact.Note = dto.Note;
        }
    }
}
