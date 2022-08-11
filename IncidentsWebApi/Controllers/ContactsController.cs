using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IncidentsWebApi.Model;

namespace IncidentsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ContactsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactDTO>>> GetContacts()
        {
            List<ContactDTO> contactsDTO = await _context.Contacts.Select(c => new ContactDTO(c)).ToListAsync();
            return Ok(contactsDTO);
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactDTO>> GetContact(int id)
        {
            Contact? contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return NotFound(new { errorMessage = $"Can't find contact with id = {id}" });
            }

            return new ContactDTO(contact);
        }

        // PUT: api/Contacts
        [HttpPut]
        public async Task<IActionResult> PutContact(ContactDTO contact)
        {
            Contact? contactWithEmail = await _context.Contacts.AsNoTracking().FirstOrDefaultAsync(c => c.Email == contact.Email);
            if(contactWithEmail != null)
            {
                if(contactWithEmail.Id != contact.Id)
                {
                    return BadRequest(new {errorMessage=$"Email '{contact.Email}' is busy by another user"});
                }
            }
            Contact contactFromDTO = new Contact(contact);
            _context.Contacts.Update(contactFromDTO);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Contacts
        [HttpPost]
        public async Task<ActionResult<ContactDTO>> PostContact(ContactDTO contact)
        {
            Contact? contactWithEmail = await _context.Contacts.AsNoTracking().FirstOrDefaultAsync(c => c.Email == contact.Email);
            if (contactWithEmail != null)
            {
                if (contactWithEmail.Id != contact.Id)
                {
                    return BadRequest(new { errorMessage = $"Email '{contact.Email}' is busy by another user" });
                }
            }
            Contact contactFromDTO = new Contact(contact);
            _context.Contacts.Add(contactFromDTO);
            await _context.SaveChangesAsync();
            contact.Id = contactFromDTO.Id;
            return Ok(contact);
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            Contact? contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound(new {errorMessage=$"Can't find contact with id = {id}" });
            }
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
