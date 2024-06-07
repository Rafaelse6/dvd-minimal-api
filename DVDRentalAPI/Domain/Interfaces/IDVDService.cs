using DVDRentalAPI.Domain.Entities;

namespace DVDRentalAPI.Domain.Interfaces
{
    public interface IDVDService
    {
        List<DVD> GetAllDVDs(int? page = 1, string? title = null, 
                            string? genre = null, int? duration = null, 
                            int? year = null, DateTime? releaseDate = null);

        DVD? FindById(int id);
        void Create(DVD dvd);
        void Update(DVD dvd);
        void Delete(int id);
    }
}
