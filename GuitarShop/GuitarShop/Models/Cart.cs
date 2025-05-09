using System.ComponentModel.DataAnnotations;

namespace GuitarShop.Models
{
    public class Cart
    {
        [Key] 
        public int Id { get; set; }
        public Users User { get; set; }
        public List<Purchase> Purchases { get; set; }
    }
}
