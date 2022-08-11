using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IncidentsWebApi.Model;

namespace IncidentsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public AccountsController(ApplicationContext context)
        {
            _context = context;
        }
        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAccounts()
        {
            List<AccountDTO> accountsDTO = await _context.Accounts.Select(c => new AccountDTO(c)).ToListAsync();
            return Ok(accountsDTO);
        }
        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDTO>> GetAccount(int id)
        {
            Account? account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound(new { errorMessage = $"Can't find account with id = {id}" });
            }

            return new AccountDTO(account);
        }
        // PUT: api/Accounts
        [HttpPut]
        public async Task<IActionResult> PutAccount(AccountDTO account)
        {
            Account? foundAccount = await _context.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.Name == account.Name);
            if(foundAccount != null)
            {
                if(foundAccount.Id != account.Id)
                {
                    return BadRequest(new { errorMessage = $"Account name '{account.Name}' is busy by another user" });
                }
            }
            Account accountFromDTO = new Account(account);
            _context.Accounts.Update(accountFromDTO);
            _context.SaveChanges();
            return NoContent();
        }
        // POST: api/Accounts/5
        [HttpPost("{initialContactId:int}")]
        public async Task<ActionResult<AccountDTO>> PostAccount(AccountDTO account, int initialContactId)
        {
            Account? foundAccount = await _context.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.Name == account.Name);
            if (foundAccount != null)
            {
                if (foundAccount.Id != account.Id)
                {
                    return BadRequest(new { errorMessage = $"Account name '{account.Name}' is busy by another user" });
                }
            }
            Contact? initialContact = await _context.Contacts.FindAsync(initialContactId);
            if(initialContact == null) { return NotFound(new { errorMessage = $"Contact with id =  '{initialContactId}' does not exist" }); }
            Account accountFromDTO = new Account(account);
            accountFromDTO.Contacts.Add(initialContact);
            _context.Accounts.Add(accountFromDTO);
            _context.SaveChanges();
            account.Id = accountFromDTO.Id;
            return Ok(account);
        }
        [HttpPost("addContactToAccount/{accountId:int}/{contactId:int}")]
        public async Task<ActionResult> AddContactToAccount(int accountId, int contactId)
        {
            Account? account = await _context.Accounts.FindAsync(accountId);
            if(account == null) { return NotFound(new {errorText=$"Account with id = {accountId} does not exist"}); }
            Contact? contact = _context.Contacts.Find(contactId);
            if (contact == null) { return NotFound(new { errorText = $"Contact with id = {contactId} does not exist" }); }
            contact.AccountId = accountId;
            _context.SaveChanges();
            return NoContent();
        }
        [HttpPost("removeContactFromAccount/{accountId:int}/{contactId:int}")]
        public async Task<ActionResult> RemoveContactFromAccount(int accountId, int contactId)
        {
            Account? account = await _context.Accounts.FindAsync(accountId);
            if (account == null) { return NotFound(new { errorText = $"Account with id = {accountId} does not exist" }); }
            Contact? contact = _context.Contacts.Find(contactId);
            if (contact == null) { return NotFound(new { errorText = $"Contact with id = {accountId} does not exist" }); }
            contact.AccountId = null;
            _context.SaveChanges();
            return NoContent();
        }
        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            Account? account = await _context.Accounts.Include(a=>a.Contacts).FirstOrDefaultAsync(a=>a.Id==id);

            if (account == null)
            {
                return NotFound(new { errorMessage = $"Can't find account with id = {id}" });
            }
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
