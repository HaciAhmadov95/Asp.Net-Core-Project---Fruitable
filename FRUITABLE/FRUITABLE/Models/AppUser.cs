using Microsoft.AspNetCore.Identity;

namespace FRUITABLE.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
