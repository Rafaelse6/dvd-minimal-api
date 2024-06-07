using Microsoft.EntityFrameworkCore;

namespace DVDRentalAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    }
}
