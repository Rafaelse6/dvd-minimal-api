namespace DVDRentalAPI.Domain.Entities.DTO
{
    public class DVDDTO
    {
        public string Title { get; set; } = default!;
        public string Genre { get; set; } = default!;
        public int Duration { get; set; } = default!;
        public int Year { get; set; } = default!;
        public DateTime ReleaseDate { get; set; } = default!;
    }
}
