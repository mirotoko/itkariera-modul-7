//s koi akaunt pushvam?
using System.ComponentModel.DataAnnotations;

namespace GuitarShop.Models
{
    public class Courier
    {
        [Key]
        public int Id { get; set; }
        public string Company {  get; set; }
        public Shop Shop { get; set; } = null!;
        public Guitar Guitar { get; set; } = null!;
        public int Quantity { get; set; }
        public string Status { get; set; }
    }
}
