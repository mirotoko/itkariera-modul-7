using System.ComponentModel.DataAnnotations;
namespace GuitarShop.Models
{
    public class Guitar
    {
        /*`Type` varchar(20) NOT NULL,
  `Body style` varchar(50) NOT NULL,
  `Brand` varchar(20) NOT NULL,
  `Name` varchar(50) NOT NULL,
  `Price (EURO)` decimal(10, 2) NOT NULL,
  `Availability` int(10),
  `Level of interest` int (10)*/
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
