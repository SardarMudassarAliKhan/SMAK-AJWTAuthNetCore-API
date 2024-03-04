using Microsoft.Extensions.Configuration;

namespace SMAK_AJWTAuthNetCore_Core.Entities
{
    public class JsonWebTokenKeys
    {
        public bool ValidateIssuerSigningKey { get; set; }
        public string? IssuerSigningKey { get; set; }
        public string? securityKey { get; set; }
        public bool ValidateIssuer { get; set; }
        public string? ValidIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public string? ValidAudience { get; set; }
        public bool RequireExpirationTime { get; set; }
        public bool ValidateLifetime { get; set; }

        public JsonWebTokenKeys(IConfiguration configuration)
        {
            this.ValidateIssuerSigningKey = bool.Parse(configuration["JsonWebTokenKeys:ValidateIssuerSigningKey"]);
            this.IssuerSigningKey = configuration["JsonWebTokenKeys:IssuerSigningKey"];
            this.securityKey = configuration["JsonWebTokenKeys:securityKey"];
            this.ValidateIssuer = bool.Parse(configuration["JsonWebTokenKeys:ValidateIssuer"]);
            this.ValidIssuer = configuration["JsonWebTokenKeys:ValidIssuer"];
            this.ValidateAudience = bool.Parse(configuration["JsonWebTokenKeys:ValidateAudience"]);
            this.ValidAudience = configuration["JsonWebTokenKeys:ValidAudience"];
            this.RequireExpirationTime = bool.Parse(configuration["JsonWebTokenKeys:Require"]);
            this.ValidateLifetime = bool.Parse(configuration["JsonWebTokenKeys:ValidateLifetime"]);
        }
    }
}
