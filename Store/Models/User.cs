using Microsoft.AspNetCore.Identity;

namespace Store.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public double? Balance { get; set; }

    }
}
