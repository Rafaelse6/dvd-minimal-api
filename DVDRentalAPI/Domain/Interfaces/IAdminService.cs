using DVDRentalAPI.Domain.Entities;
using DVDRentalAPI.Domain.Entities.DTO;

namespace DVDRentalAPI.Domain.Interfaces
{
    public interface IAdminService
    {
        Admin? Login(LoginDTO loginDTO);
    }
}
