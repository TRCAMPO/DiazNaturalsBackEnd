﻿namespace BACK_END_DIAZNATURALS.Jwt
{
    public class JwtData
    {
        public string Key { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string Subject { get; set; } = null!;

    }
}
