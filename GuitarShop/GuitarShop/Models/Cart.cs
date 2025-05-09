using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GuitarShop.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; } = null!;
        [ForeignKey("UserId")]
        public Users User { get; set; } = null!;

        public string GuitarName { get; set; } = null!;
        [ForeignKey("GuitarName")]
        public Guitar Guitar { get; set; } = null!;

        public int Quantity { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}
