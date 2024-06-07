using DVDRentalAPI.Data;
using DVDRentalAPI.Domain.Entities;
using DVDRentalAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DVDRentalAPI.Services
{
    public class DVDService : IDVDService
    {
        private readonly SQLContext _context;

        public DVDService(SQLContext context)
        {
            _context = context;
        }

        public List<DVD> GetAllDVDs(int page = 1, string? title = null, string? genre = null, int? duration = null, int? year = null, DateTime? releaseDate = null)
        {
            var query = _context.Dvds.AsQueryable();
            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(d => EF.Functions.Like(d.Title.ToLower(), $"%{title}%"));
            }

            int itensPerPage = 10;

            query = query.Skip((page - 1) * itensPerPage).Take(itensPerPage);

            return [.. query];
        }

        public DVD? FindById(int id)
        {
            return _context.Dvds.Where(v => v.Id == id).FirstOrDefault();
        }

        public void Create(DVD dvd)
        {
            _context.Dvds.Add(dvd);
            _context.SaveChanges();
        }

        public void Update(DVD dvd)
        {
            _context.Dvds.Update(dvd);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.Remove(id);
            _context.SaveChanges();
        }
    }
}
