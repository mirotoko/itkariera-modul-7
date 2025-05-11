using System.ComponentModel.DataAnnotations;
namespace GuitarShop.Models
{
    public class Guitar
    {
        public string? Type { get; set; }
        public string? Body { get; set; }
        public string? Brand { get; set; }
        [Key]
        public string? Name { get; set; }
        public int Price { get; set; }
        public int Availability { get; set; }
        public int Interest {  get; set; }
    }
}
