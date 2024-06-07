using DVDRentalAPI.Domain.Entities;

namespace DVDRentalAPI.Domain.Interfaces
{
    public interface IAdminService
    {
        Admin? Login(LoginDTO loginDTO);
    }
}
