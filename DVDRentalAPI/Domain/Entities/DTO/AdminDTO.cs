using DVDRentalAPI.Domain.Enums;

namespace DVDRentalAPI.Domain.Entities.DTO
{
    public class AdminDTO
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public Profile? Profile { get; set; }
    }
}
