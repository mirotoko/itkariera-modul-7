using System.ComponentModel.DataAnnotations;

namespace GuitarShop.Models
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }
        public Guitar? Guitar { get; set; }
        public User? User { get; set; }
        public DateTime DateTime { get; set; }

        public Purchase(Guitar? guitar, User? user)
        {
            Guitar = guitar;
            User = user;
            DateTime = DateTime.Now;
        }
        public Purchase() { }
    }
}
