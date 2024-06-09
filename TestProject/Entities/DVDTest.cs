using DVDRentalAPI.Domain.Entities;
using System.Runtime.Intrinsics.Arm;

namespace TestProject.Entities
{
    [TestClass]
    public class DVDTest
    {

        [TestMethod]
        public void GetSetPropertiesTest()
        {

            //Arrange
            var dvd = new DVD();

            //Act
            dvd.Id = 1;
            dvd.Title = "Title";
            dvd.Genre = "Action";
            dvd.Duration = 120;
            dvd.Year = 2000;

            //Assert
            Assert.AreEqual(1, dvd.Id);
            Assert.AreEqual("Title", dvd.Title);
            Assert.AreEqual("Action", dvd.Genre);
            Assert.AreEqual(120, dvd.Duration);
            Assert.AreEqual(2000, dvd.Year);
        }
    }
}
