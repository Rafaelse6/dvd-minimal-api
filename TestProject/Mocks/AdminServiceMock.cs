using DVDRentalAPI.Domain.Entities;
using DVDRentalAPI.Domain.Entities.DTO;
using DVDRentalAPI.Domain.Interfaces;

namespace TestProject.Mocks
{
    public class AdminServiceMock : IAdminService
    {
        private readonly static List<Admin> admins = [
            new Admin{
                Id = 1,
                Email = "adm@test.com",
                Password = "123456",
                Profile = "Adm"
            }
        ];

        public List<Admin> FindAll(int? page)
        {
            return admins;
        }

        public Admin? FindById(int id)
        {
            return admins.Find(a => a.Id == id);
        }

        public Admin Create(Admin admin)
        {
            admin.Id = admins.Count() + 1;
            admins.Add(admin);

            return admin;
        }

        public Admin? Login(LoginDTO loginDTO)
        {
            return admins.Find(a => a.Email == loginDTO.Email && a.Password == loginDTO.Password);
        }
    }
}
