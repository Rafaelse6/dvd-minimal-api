using System;
using System.Collections.Generic;
using System.Linq;
using DVDRentalAPI.Domain.Entities;
using DVDRentalAPI.Domain.Interfaces;

namespace TestProject.Mocks
{
    public class DVDServiceMock : IDVDService
    {
        private static readonly List<DVD> dvds = new List<DVD>
        {
            new DVD
            {
                Id = 1,
                Title = "Title",
                Genre = "Action",
                Duration = 120,
                Year = 2000
            },
            new DVD
            {
                Id = 2,
                Title = "Title2",
                Genre = "Action2",
                Duration = 160,
                Year = 2002
            }
        };

        public void Create(DVD dvd)
        {
            dvd.Id = dvds.Count + 1;
            dvds.Add(dvd);
        }

        public void Delete(DVD dvd)
        {
            var dvdToDelete = dvds.FirstOrDefault(d => d.Id == dvd.Id);
            if (dvdToDelete != null)
                dvds.Remove(dvdToDelete);
        }

        public DVD? FindById(int? id)
        {
            if (id.HasValue)
                return dvds.FirstOrDefault(d => d.Id == id);
            return null;
        }

        public List<DVD> GetAllDVDs(int? page = 1, string? title = null, string? genre = null, int? duration = null, int? year = null)
        {
            var result = dvds;

            if (!string.IsNullOrEmpty(title))
                result = result.Where(d => d.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrEmpty(genre))
                result = result.Where(d => d.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase)).ToList();

            if (duration.HasValue)
                result = result.Where(d => d.Duration == duration).ToList();

            if (year.HasValue)
                result = result.Where(d => d.Year == year).ToList();

            const int pageSize = 10;
            return result.Skip((page.Value - 1) * pageSize).Take(pageSize).ToList();
        }

        public void Update(DVD dvd)
        {
            var existingDVD = dvds.FirstOrDefault(d => d.Id == dvd.Id);
            if (existingDVD != null)
            {
                existingDVD.Title = dvd.Title;
                existingDVD.Genre = dvd.Genre;
                existingDVD.Duration = dvd.Duration;
                existingDVD.Year = dvd.Year;
            }
        }
    }
}
