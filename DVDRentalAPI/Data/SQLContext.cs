using DVDRentalAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DVDRentalAPI.Data
{
    public class SQLContext : DbContext
    {
        public SQLContext(DbContextOptions<SQLContext> options) : base(options) { }

        public DbSet<Admin> Admins { get; set; } = default!;

        public DbSet<DVD> Dvds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = 1,
                    Email = "admin@test.com",
                    Password = "123456",
                    Profile = "Adm"
                }
            );
        }
    }
}
