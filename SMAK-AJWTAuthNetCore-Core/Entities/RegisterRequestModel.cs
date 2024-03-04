using Microsoft.AspNetCore.Identity;

namespace SMAK_AJWTAuthNetCore_Core.Entities
{
    public class RegisterRequestModel : IdentityUser
    {
        public string? Name { get; set; }
        public string? AccountType { get; set; }
        public string? PhoneNo { get; set; }
        public string? Password { get; set; }
        public string? UserRole { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
