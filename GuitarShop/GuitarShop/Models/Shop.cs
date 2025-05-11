using System.ComponentModel.DataAnnotations;

namespace GuitarShop.Models
{
    public class Shop
    {
        public string Town { get; set; }
        [Key]
        public string Name { get; set; }
    }
}
