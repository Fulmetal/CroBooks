using System.ComponentModel.DataAnnotations;
using CroBooks.Domain.Base;
using CroBooks.Domain.Contacts;
using CroBooks.Shared.Dto;

namespace CroBooks.Domain.Clients
{
    public sealed class Client : AuditEntity<int>
    {
        [StringLength(300)]
        public string Name { get; set; } = string.Empty;
        [StringLength(300)]
        public string Email { get; set; } = string.Empty;
        [StringLength(50)]
        public string Phone { get; set; } = string.Empty;
        [StringLength(300)]
        public string Address { get; set; } = string.Empty;
        [StringLength(300)]
        public string City { get; set; } = string.Empty;
        [StringLength(100)]
        public string PostalCode { get; set; } = string.Empty;
        [StringLength(200)]
        public string Country { get; set; } = string.Empty;
        [StringLength(100)]
        public string TaxNumber { get; set; } = string.Empty;
        [StringLength(2000)]
        public string Note { get; set; } = string.Empty;
        public List<Contact> Contacts { get; set; } = new List<Contact>();

        public Client()
        {
        }

        public Client(ClientDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            Email = dto.Email;
            Phone = dto.Phone;
            Address = dto.Address;
            City = dto.City;
            PostalCode = dto.PostalCode;
            Country = dto.Country;
            TaxNumber = dto.TaxNumber;
            Note = dto.Note;
        }


        public ClientDto ToDto()
        {
            return new ClientDto
            {
                Id = Id,
                Name = Name,
                Email = Email,
                Phone = Phone,
                Address = Address,
                PostalCode = PostalCode,
                City = City,
                Country = Country,
                TaxNumber = TaxNumber,
                Note = Note
            };
        }

        public static void UpdateFromDto(Client client, ClientDto dto)
        {
            client.Name = dto.Name;
            client.Email = dto.Email;
            client.Phone = dto.Phone;
            client.Address = dto.Address;
            client.PostalCode = dto.PostalCode;
            client.City = dto.City;
            client.Country = dto.Country;
            client.TaxNumber = dto.TaxNumber;
            client.Note = dto.Note;
        }
    }
}
