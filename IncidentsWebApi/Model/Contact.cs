using Microsoft.EntityFrameworkCore;
namespace IncidentsWebApi.Model
{
    [Index("Email", IsUnique = true)]
    public class Contact
    {
        public Contact() { }
        public Contact(ContactDTO contactDTO)
        {
            Id = contactDTO.Id;
            FirstName = contactDTO.FirstName;
            LastName = contactDTO.LastName;
            Email = contactDTO.Email;
            AccountId = contactDTO.AccountId;
        }
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";

        public Account? Account { get; set; }
        public int? AccountId { get; set; }
    }
    public class ContactDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public int? AccountId { get; set; }

        public ContactDTO() { }
        public ContactDTO(Contact contact)
        {
            Id = contact.Id;
            FirstName = contact.FirstName;
            LastName = contact.LastName;
            Email = contact.Email;
            AccountId = contact.AccountId;
        }
    }
}
