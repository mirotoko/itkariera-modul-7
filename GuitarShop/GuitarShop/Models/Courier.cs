//s koi akaunt pushvam?
using System.ComponentModel.DataAnnotations;

namespace GuitarShop.Models
{
    public class Courier
    {
        /*CREATE TABLE `Couriers` (
  `Order Id` int(10) NOT NULL,
  `Company` varchar(20) NOT NULL,
  `Shop of delivery` varchar(50) NOT NULL,
  `Product of delivery` varchar(50) NOT NULL,
  `Quantity of item` int(10) NOT NULL,
  `Status` varchar(50) NOT NULL,
  PRIMARY KEY (`Order Id`),
  FOREIGN KEY (`Shop of delivery`) REFERENCES Shops(`Store name`),
  FOREIGN KEY (`Product of delivery`) REFERENCES Guitars(`Name`)
);*/
        [Key]
        public int Id { get; set; }
        public string Company {  get; set; }
        public Shop Shop { get; set; } = null!;
        public Guitar Guitar { get; set; } = null!;
        public int Quantity { get; set; }
        public string Status { get; set; }
    }
}
