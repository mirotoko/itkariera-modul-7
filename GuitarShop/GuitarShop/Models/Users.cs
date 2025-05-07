using Microsoft.AspNetCore.Identity;

namespace GuitarShop.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }    
    }
}
