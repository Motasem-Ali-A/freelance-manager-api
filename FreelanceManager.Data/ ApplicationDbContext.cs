using FreelanceManager.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace FreelanceManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Seed Data for testing
            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    Id = 1,
                    CreatedAt = new DateTime(2026, 1, 1),
                    Name = "John Smith",
                    Email = "john@acme.com",
                    Phone = "123-456-7890",
                    CompanyName = "Acme Corp",
                    Address = "123 Main St, New York",
                    Notes = "Long term client",
                    Status = "Active"
                },
                new Client
                {
                    Id = 2,
                    CreatedAt = new DateTime(2026, 1, 15),
                    Name = "Sara Jones",
                    Email = "sara@globex.com",
                    Phone = "987-654-3210",
                    CompanyName = "Globex",
                    Address = "456 Market St, London",
                    Notes = "",
                    Status = "Active"
                }
            );
            modelBuilder.Entity<Project>().HasData(
                new Project
                {
                    Id = 1,
                    CreatedAt = new DateTime(2026, 1, 5),
                    Title = "E-commerce Website",
                    Description = "Build a full e-commerce website",
                    Status = "InProgress",
                    StartDate = new DateTime(2026, 1, 5),
                    EndDate = new DateTime(2026, 3, 5),
                    HourlyRate = 50,
                    FixedPrice = 0,
                    BillingType = "Hourly",
                    ClientId = 1
                },
                new Project
                {
                    Id = 2,
                    CreatedAt = new DateTime(2026, 1, 20),
                    Title = "Website Redesign",
                    Description = "Modern website redesign",
                    Status = "NotStarted",
                    StartDate = new DateTime(2026, 2, 1),
                    EndDate = new DateTime(2026, 4, 1),
                    HourlyRate = 0,
                    FixedPrice = 1500,
                    BillingType = "Fixed",
                    ClientId = 2
                }
            );


            modelBuilder.Entity<TimeEntry>().HasData(
                new TimeEntry
                {
                    Id = 1,
                    CreatedAt = new DateTime(2026, 1, 10),
                    Date = new DateTime(2026, 1, 10),
                    HoursWorked = 5,
                    Description = "Built login page",
                    ProjectId = 1
                },
                new TimeEntry
                {
                    Id = 2,
                    CreatedAt = new DateTime(2026, 1, 11),
                    Date = new DateTime(2026, 1, 11),
                    HoursWorked = 3,
                    Description = "Built navigation bar",
                    ProjectId = 1
                }
            );

            modelBuilder.Entity<Invoice>().HasData(
                new Invoice
                {
                    Id = 1,
                    CreatedAt = new DateTime(2026, 2, 1),
                    InvoiceNumber = "INV-0001",
                    IssueDate = new DateTime(2026, 2, 1),
                    DueDate = new DateTime(2026, 2, 15),
                    Status = "Sent",
                    Notes = "Payment via bank transfer",
                    Subtotal = 400,
                    TaxRate = 0.15m,
                    TaxAmount = 60,
                    TotalAmount = 460,
                    ClientId = 1
                },
                new Invoice
                {
                    Id = 2,
                    CreatedAt = new DateTime(2026, 2, 10),
                    InvoiceNumber = "INV-0002",
                    IssueDate = new DateTime(2026, 2, 10),
                    DueDate = new DateTime(2026, 2, 24),
                    Status = "Draft",
                    Notes = "",
                    Subtotal = 1500,
                    TaxRate = 0.15m,
                    TaxAmount = 225,
                    TotalAmount = 1725,
                    ClientId = 2
                }
            );

            modelBuilder.Entity<InvoiceItem>().HasData(
                new InvoiceItem
                {
                    Id = 1,
                    Description = "Frontend Development - 8hrs",
                    Quantity = 1,
                    UnitPrice = 400,
                    Total = 400,
                    InvoiceId = 1
                },
                new InvoiceItem
                {
                    Id = 2,
                    Description = "Website Redesign - Fixed Price",
                    Quantity = 1,
                    UnitPrice = 1500,
                    Total = 1500,
                    InvoiceId = 2
                }
            );

            //Entity relationship configureation
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Projects)
                .WithOne(p => p.Client)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Invoices)
                .WithOne(i => i.Client)
                .HasForeignKey(i => i.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.TimeEntries)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Invoice>()
                .HasMany(i => i.InvoiceItems)
                .WithOne(it => it.Invoice)
                .HasForeignKey(it => it.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);


            //decimal precisionconfigureation
            modelBuilder.Entity<Project>()
                .Property(p => p.HourlyRate)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Project>()
                .Property(p => p.FixedPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TimeEntry>()
                .Property(t => t.HoursWorked)
                .HasPrecision(4, 2);

            modelBuilder.Entity<Invoice>()
                .Property(i => i.Subtotal)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Invoice>()
                .Property(i => i.TaxAmount)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Invoice>()
                .Property(i => i.TotalAmount)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Invoice>()
                .Property(i => i.TaxRate)
                .HasPrecision(18, 2);

            modelBuilder.Entity<InvoiceItem>()
                .Property(ii => ii.UnitPrice)
                .HasPrecision(18, 2);
            modelBuilder.Entity<InvoiceItem>()
                .Property(ii => ii.Total)
                .HasPrecision(18, 2);


        }
    }
}