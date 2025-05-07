using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GuitarShop.ViewModels
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
