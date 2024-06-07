using DVDRentalAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace DVDRentalAPI.Data
{
    public class DataContext : DbContext
    {

        public DbSet<Admin> Admins { get; set; } = default!;

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

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    }
}
