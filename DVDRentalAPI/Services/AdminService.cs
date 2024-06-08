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

        public List<Admin> FindAll(int? page)
        {
            var query = _context.Admins.AsQueryable();

            int itensPerPage = 10;

            if (page != null)
                query = query.Skip((int)(page - 1) * itensPerPage).Take(itensPerPage);


            return [.. query];
        }

        public Admin? FindById(int id)
        {
            return _context.Admins.Where(a => a.Id == id).FirstOrDefault();
        }

        public Admin Create(Admin admin)
        {
            _context.Admins.Add(admin);
            _context.SaveChanges();

            return admin;
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
