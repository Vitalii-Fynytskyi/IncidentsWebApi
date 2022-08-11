using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace IncidentsWebApi.Model
{
    [Index("Name", IsUnique = true)]
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [ForeignKey("IncidentName")]
        public virtual Incident? Incident { get; set; }
        public Guid? IncidentName { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();
        public Account() { }
        public Account(AccountDTO accountDTO)
        {
            Id = accountDTO.Id;
            Name = accountDTO.Name;
            if(accountDTO.IncidentName == null)
            {
                IncidentName = null;
            }
            else
            {
                IncidentName = Guid.Parse(accountDTO.IncidentName);
            }
        }

    }
    public class AccountDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? IncidentName { get; set; }
        public AccountDTO() { }
        public AccountDTO(Account account)
        {
            Id = account.Id;
            Name = account.Name;
            IncidentName = account.IncidentName?.ToString();
            if(account.IncidentName == null)
            {
                IncidentName = null;
            }
            else
            {
                IncidentName = account.IncidentName.ToString();
            }
        }
    }
}
