﻿
namespace Infrastructure.Authentication.JWT.Validator
{
    public class JwtSettings
    {
        public required string Key { get; set; }

        public required string Issuer { get; set; }

        public required string Audience { get; set; }

        public required double Lifetime { get; set; }
    }
}
