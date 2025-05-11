using Microsoft.AspNetCore.Identity;

namespace GuitarShop.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        
        public List<string> Cart { get; set; } = new();
        
    }
}
