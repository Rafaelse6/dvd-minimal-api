﻿namespace DVDRentalAPI.Domain.Entities.DTO
{
    public class LoginDTO
    {
        public string Email { get; set; } = default!;

        public string Password { get; set; } = default!;
    }
}
