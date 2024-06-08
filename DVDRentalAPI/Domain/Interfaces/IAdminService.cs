using DVDRentalAPI.Domain.Entities;
using DVDRentalAPI.Domain.Entities.DTO;

namespace DVDRentalAPI.Domain.Interfaces
{
    public interface IAdminService
    {
        List<Admin> FindAll(int? page);
        Admin? FindById(int id);
        Admin? Login(LoginDTO loginDTO);
        Admin Create(Admin admin);
    }
}
