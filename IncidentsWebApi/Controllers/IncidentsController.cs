using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IncidentsWebApi.Model;

namespace IncidentsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public IncidentsController(ApplicationContext context)
        {
            _context = context;
        }
        // GET: api/Incidents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncidentDTO>>> GetIncidents()
        {
            List<IncidentDTO> incidentsDTO = await _context.Incidents.Select(c => new IncidentDTO(c)).ToListAsync();
            return Ok(incidentsDTO);
        }
        // GET: api/Incidents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IncidentDTO>> GetIncident(int id)
        {
            Incident? incident = await _context.Incidents.FindAsync(id);

            if (incident == null)
            {
                return NotFound(new { errorMessage = $"Can't find incident with id = {id}" });
            }

            return new IncidentDTO(incident);
        }
        // PUT: api/Incidents
        [HttpPut]
        public async Task<IActionResult> PutIncident(IncidentDTO incident)
        {
            Incident incidentFromDTO = new Incident(incident);
            _context.Incidents.Update(incidentFromDTO);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        // POST: api/Incidents
        [HttpPost("{initialAccountId:int}")]
        public async Task<ActionResult<IncidentDTO>> PostIncident(IncidentDTO incident, int initialAccountId)
        {
            Account? initialAccount = await _context.Accounts.FindAsync(initialAccountId);
            if (initialAccount == null) { return BadRequest(new { errorMessage = $"Can't find account with id = {initialAccountId}" }); }
            Incident incidentFromDTO = new Incident(incident);
            incidentFromDTO.Accounts.Add(initialAccount);
            _context.Incidents.Add(incidentFromDTO);
            await _context.SaveChangesAsync();
            incident.IncidentName = incidentFromDTO.IncidentName.ToString();
            return Ok(incident);
        }
        [HttpPost]
        public async Task<ActionResult<IncidentDTO>> PostIncidentCustomRequest(IncidentCreateRequest incidentCreateRequest)
        {
            //find account
            Account? account = await _context.Accounts.FirstOrDefaultAsync(a => a.Name == incidentCreateRequest.AccountName);
            if (account == null) { return NotFound(new { errorText = $"Account \'{incidentCreateRequest.AccountName}\' does not exist" }); }

            //find contact
            Contact? contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Email == incidentCreateRequest.Email);
            if(contact != null) //update contact data
            {
                contact.FirstName = incidentCreateRequest.FirstName;
                contact.LastName = incidentCreateRequest.LastName;
                contact.AccountId = account.Id;
            }
            else //create new contact and link to account
            {
                Contact newContact = new Contact
                {
                    FirstName = incidentCreateRequest.FirstName,
                    LastName = incidentCreateRequest.LastName,
                    Email = incidentCreateRequest.Email,
                    AccountId=account.Id
                };
                _context.Contacts.Add(newContact);
            }
            //create incident for account
            Incident? incident = await _context.Incidents.FirstOrDefaultAsync(i => i.Description == incidentCreateRequest.IncidentDescription);
            if(incident != null)
            {
                incident.Accounts.Add(account);
            }
            else
            {
                Incident newIncident = new Incident();
                newIncident.Description = incidentCreateRequest.IncidentDescription;
                newIncident.Accounts.Add(account);
                _context.Incidents.Add(newIncident);
                await _context.SaveChangesAsync();
                return Ok(new IncidentDTO(newIncident));

            }
            await _context.SaveChangesAsync();
            return Ok(new IncidentDTO(incident));

        }
        [HttpDelete("{incidentName}")]
        public async Task<IActionResult> DeleteIncident(string incidentName)
        {
            Incident? incident = await _context.Incidents.Include(i=>i.Accounts).FirstOrDefaultAsync(i=>i.IncidentName.ToString() == incidentName);
            if (incident == null)
            {
                return NotFound(new { errorMessage = $"Can't find incident with incidentName = {incidentName}" });
            }
            _context.Incidents.Remove(incident);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
