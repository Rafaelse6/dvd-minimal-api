using DVDRentalAPI.Data;
using DVDRentalAPI.Domain.Entities;
using DVDRentalAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace TestProject.Services
{
    [TestClass]
    public class AdminServiceTest
    {
        private SQLContext CreateTestContext()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));

            var builder = new ConfigurationBuilder()
                .SetBasePath(path ?? Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<SQLContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new SQLContext(optionsBuilder.Options);
        }

        [TestMethod]
        public void CreateAdminTest()
        {

            // Arrange
            var context = CreateTestContext();
            var adminService = new AdminService(context);
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE admins");

            var adm = new Admin();
            adm.Email = "test@test.com";
            adm.Password = "password";
            adm.Profile = "Adm";

            // Act
            adminService.Create(adm);

            // Assert
            Assert.AreEqual(1, adminService.FindAll(1).Count());
        }
    }
}
