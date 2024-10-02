using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Uppgift_3.Models;

namespace Uppgift_3.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Incident> Incidents { get; set; }



        // Kanske inte nödvändigt:
        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);

        //    // Configure relationships, if needed
        //    builder.Entity<Incident>()
        //        .HasOne(i => i.Driver)
        //        .WithMany(d => d.Incidents)
        //        .HasForeignKey(i => i.DriverID);
        //}
    }
}
