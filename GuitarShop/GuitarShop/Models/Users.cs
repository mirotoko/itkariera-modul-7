using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GuitarShop.Models
{
    public class Users : IdentityUser
    {
        //molq vi za boga slagaite primary key na neshtoto koeto shte se pozlva za identifikaciq 👨‍🦯
        //sushto modelite trqbva da sa v edinstveno chilso, please
        [Key]
        public string FullName { get; set; }
    }
}
