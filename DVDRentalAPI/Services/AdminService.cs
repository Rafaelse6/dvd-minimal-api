using DVDRentalAPI.Domain.Entities;
using DVDRentalAPI.Domain.Entities.DTO;
using DVDRentalAPI.Domain.Interfaces;

namespace DVDRentalAPI.Services
{
    public class AdminService : IAdminService
    {
        private readonly Data.SQLContext _context;

        public AdminService(Data.SQLContext context)
        {
            _context = context;
        }

        public Admin? Login(LoginDTO loginDTO)
        {
            var adm = _context.Admins.Where
                (a => a.Email == loginDTO.Email && a.Password == loginDTO.Password)
                .FirstOrDefault();

            return adm;
        }
    }
}
