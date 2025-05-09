using Microsoft.AspNetCore.Identity;

namespace GuitarShop.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        
        public List<string> Cart { get; set; } = new(); // nesigurno i nedobro no RABOTI za momenta i NE mi se zanimava s neshto po slojno <3
        
    }
}
