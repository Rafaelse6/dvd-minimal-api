using DVDRentalAPI.Data;
using DVDRentalAPI.Domain.Entities;
using DVDRentalAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace TestProject.Services
{
    [TestClass]
    public class DVDServiceTest
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
        public void CreateDVDTest()
        {
            // Arrange
            var context = CreateTestContext();
            var dvdService = new DVDService(context);
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE dvds");

            var dvd = new DVD
            {
                Title = "Test DVD",
                Genre = "Action",
                Duration = 120,
                Year = 2020
            };

            // Act
            dvdService.Create(dvd);

            // Assert
            Assert.AreEqual(1, dvdService.GetAllDVDs().Count());
        }

        [TestMethod]
        public void UpdateDVDTest()
        {
            // Arrange
            var context = CreateTestContext();
            var dvdService = new DVDService(context);
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE dvds");

            var dvd = new DVD
            {
                Title = "Test DVD",
                Genre = "Action",
                Duration = 120,
                Year = 2020
            };

            dvdService.Create(dvd);

            var updatedDvd = new DVD
            {
                Id = dvd.Id,
                Title = "Updated DVD",
                Genre = "Updated Genre",
                Duration = 150,
                Year = 2021
            };

            // Act
            dvdService.Update(updatedDvd);

            // Assert
            var retrievedDvd = dvdService.FindById(updatedDvd.Id);
            Assert.IsNotNull(retrievedDvd);
            Assert.AreEqual(updatedDvd.Title, retrievedDvd.Title);
            Assert.AreEqual(updatedDvd.Genre, retrievedDvd.Genre);
            Assert.AreEqual(updatedDvd.Duration, retrievedDvd.Duration);
            Assert.AreEqual(updatedDvd.Year, retrievedDvd.Year);
        }

        [TestMethod]
        public void DeleteDVDTest()
        {
            // Arrange
            var context = CreateTestContext();
            var dvdService = new DVDService(context);
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE dvds");

            var dvd = new DVD
            {
                Title = "Test DVD",
                Genre = "Action",
                Duration = 120,
                Year = 2020
            };

            dvdService.Create(dvd);

            // Act
            dvdService.Delete(dvd);

            // Assert
            Assert.AreEqual(0, dvdService.GetAllDVDs().Count());
        }

        [TestMethod]
        public void FindDVDByIdTest()
        {
            // Arrange
            var context = CreateTestContext();
            var dvdService = new DVDService(context);
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE dvds");

            var dvd = new DVD
            {
                Title = "Test DVD",
                Genre = "Action",
                Duration = 120,
                Year = 2020
            };

            dvdService.Create(dvd);

            // Act
            var retrievedDvd = dvdService.FindById(dvd.Id);

            // Assert
            Assert.IsNotNull(retrievedDvd);
            Assert.AreEqual(dvd.Title, retrievedDvd.Title);
            Assert.AreEqual(dvd.Genre, retrievedDvd.Genre);
            Assert.AreEqual(dvd.Duration, retrievedDvd.Duration);
            Assert.AreEqual(dvd.Year, retrievedDvd.Year);
        }

        [TestMethod]
        public void GetAllDVDsTest()
        {
            // Arrange
            var context = CreateTestContext();
            var dvdService = new DVDService(context);
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE dvds");

            var dvd1 = new DVD
            {
                Title = "Test DVD 1",
                Genre = "Action",
                Duration = 120,
                Year = 2020
            };

            var dvd2 = new DVD
            {
                Title = "Test DVD 2",
                Genre = "Comedy",
                Duration = 90,
                Year = 2019
            };

            dvdService.Create(dvd1);
            dvdService.Create(dvd2);

            // Act
            var allDVDs = dvdService.GetAllDVDs();

            // Assert
            Assert.AreEqual(2, allDVDs.Count);
        }
    }
}
