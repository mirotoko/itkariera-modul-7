using System.ComponentModel.DataAnnotations;

namespace GuitarShop.Models
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }
        public string? GuitarName { get; set; }
        public string? UserId { get; set; }
        public DateTime DateTime { get; set; }

        public bool IsProcessed {  get; set; }
        public bool IsAccepted { get; set; }

        public Purchase(string guitarName, string userId)
        {
            DateTime = DateTime.Now;
            GuitarName = guitarName;
            UserId = userId;
            IsProcessed = false;
            IsAccepted = false;
        }
        public Purchase() { }
    }
}
