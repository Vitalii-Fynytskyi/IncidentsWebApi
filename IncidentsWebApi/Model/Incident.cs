using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace IncidentsWebApi.Model
{
    public class Incident
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid IncidentName { get; set; }
        public string Description { get; set; } = null!;
        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
        public Incident() { }
        public Incident(IncidentDTO incidentDTO)
        {
            if(incidentDTO.IncidentName != null)
            {
                IncidentName = Guid.Parse(incidentDTO.IncidentName);
            }
            Description = incidentDTO.Description;
        }

    }
    public class IncidentDTO
    {
        public string? IncidentName { get; set; }
        public string Description { get; set; } = "";
        public IncidentDTO() { }
        public IncidentDTO(Incident incident)
        {
            IncidentName = incident.IncidentName.ToString();
            Description = incident.Description;
        }
    }
    public class IncidentCreateRequest
    {
        public string? AccountName { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string IncidentDescription { get; set; } = "";
    }
}
