using Microsoft.EntityFrameworkCore;
namespace IncidentsWebApi.Model
{
    public class ApplicationContext :DbContext
    {
        public DbSet<Contact> Contacts { get; set; } = null!;
        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Incident> Incidents { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options) :base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ///populate contacts
            Contact contact1 = new Contact
            {
                Id = 1,
                FirstName = "Tom",
                LastName = "Nadal",
                Email = "example1@mail.com"
            };
            Contact contact2 = new Contact
            {
                Id = 2,
                FirstName = "Stefanos",
                LastName = "Alcaraz",
                Email = "example2@mail.com"
            };
            Contact contact3 = new Contact
            {
                Id = 3,
                FirstName = "Carlos",
                LastName = "Cilic",
                Email = "example3@mail.com"
            };
            Contact contact4 = new Contact
            {
                Id = 4,
                FirstName = "Maria",
                LastName = "Collins",
                Email = "example4@mail.com"
            };

            ///populate accounts
            Account account1 = new Account
            {
                Id = 1,
                Name = "Account1"
            };
            Account account2 = new Account
            {
                Id = 2,
                Name = "Account2"
            };
            Account account3 = new Account
            {
                Id = 3,
                Name = "Account3"
            };
            contact1.AccountId = 1;
            contact2.AccountId = 2;
            contact3.AccountId = 3;
            Incident incident1 = new Incident
            {
                IncidentName = Guid.NewGuid(),
                Description = "Incident #1"
            };
            Incident incident2 = new Incident
            {
                IncidentName = Guid.NewGuid(),
                Description = "Incident #2"
            };
            account1.IncidentName = incident1.IncidentName;
            account2.IncidentName = incident2.IncidentName;


            modelBuilder.Entity<Contact>().HasData(contact1, contact2, contact3, contact4);
            modelBuilder.Entity<Account>().HasData(account1, account2, account3);
            modelBuilder.Entity<Incident>().HasData(incident1, incident2);
            base.OnModelCreating(modelBuilder);


        }
    }
}
