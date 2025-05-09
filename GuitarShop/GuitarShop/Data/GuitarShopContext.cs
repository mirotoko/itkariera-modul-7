using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GuitarShop.Models;

namespace GuitarShop.Data
{
    public class GuitarShopContext : DbContext
    {
        public GuitarShopContext (DbContextOptions<GuitarShopContext> options)
            : base(options)
        {
        }

        public DbSet<GuitarShop.Models.Guitar> Guitar { get; set; } = default!;
        public DbSet<GuitarShop.Models.Shop> Shop { get; set; } = default!;
        public DbSet<GuitarShop.Models.Purchase> Purchase { get; set; } = default!;
        public DbSet<GuitarShop.Models.Courier> Courier { get; set; } = default!;
        public DbSet<Cart> Carts { get; set; }
    }
}
