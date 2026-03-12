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