using System.ComponentModel.DataAnnotations;

namespace GuitarShop.Models
{
    public class Purchase
    {
        /*CREATE TABLE `Purchases` (
  `Id` int(10) NOT NULL,
  `First name` varchar(20) NOT NULL,
  `Last name` varchar(20) NOT NULL,
  `Product ordered` varchar(50) NOT NULL,
  `Shop of purchase` varchar(50) NOT NULL,
  `Quantity of product` int(10) NOT NULL,
  PRIMARY KEY (`Id`),
  FOREIGN KEY (`Product ordered`) REFERENCES Guitars(`Name`),
  FOREIGN KEY (`Shop of purchase`) REFERENCES Shops(`Store name`)
);*/
        [Key]
        public int Id { get; set; }
        public string UserID { get; set; }
        public Guitar Guitar { get; set; } = null!;
        public Shop Shop { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
