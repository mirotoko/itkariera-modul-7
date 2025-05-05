using System.ComponentModel.DataAnnotations;

namespace GuitarShop.Models
{
    public class Shop
    {
        /*CREATE TABLE `Shops` (
    `Town` varchar(50) NOT NULL,
    `Store name` varchar(50) NOT NULL,
    PRIMARY KEY (`Store name`)
);*/
        public string Town { get; set; }
        [Key]
        public string Name { get; set; }
    }
}
