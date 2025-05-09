using Microsoft.EntityFrameworkCore;
using GuitarShop.Data;

namespace GuitarShop.Models
{
    public class SeedData
    {
        public static void InitializeGuitars(IServiceProvider serviceProvider)
        {
            using (var context = new GuitarShopContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<GuitarShopContext>>()))
            {
                if (context == null || context.Guitar == null)
                {
                    throw new ArgumentNullException("Null RazorPagesMovieContext");
                }

                // Look for any movies.
                if (context.Guitar.Any())
                {
                    return;   // DB has been seeded
                }

                context.Guitar.AddRange(
                    new Guitar { Type = "Classical", Body = "4/4", Brand = "Yamaha", Name = "C40", Price = 119, Availability = 25, Interest = 9 },
                    new Guitar { Type = "Classical", Body = "4/4", Brand = "Yamaha", Name = "C40 BL", Price = 135, Availability = 0, Interest = 8 },
                    new Guitar { Type = "Classical", Body = "4/4", Brand = "Yamaha", Name = "C70", Price = 205, Availability = 18, Interest = 7 },
                    new Guitar { Type = "Classical", Body = "4/4", Brand = "Cordoba", Name = "C7 CD Iberia", Price = 529, Availability = 0, Interest = 4 },
                    new Guitar { Type = "Classical", Body = "4/4", Brand = "Cordoba", Name = "C10", Price = 1249, Availability = 8, Interest = 2 },
                    new Guitar { Type = "Classical", Body = "4/4", Brand = "Taylor", Name = "314ce-N", Price = 2399, Availability = 3, Interest = 3 },
                    new Guitar { Type = "Classical", Body = "4/4", Brand = "Fender", Name = "CN-140SCE", Price = 349, Availability = 16, Interest = 5 },
                    new Guitar { Type = "Acoustic", Body = "Dreadnought", Brand = "Epiphone", Name = "Dove studio", Price = 389, Availability = 16, Interest = 9 },
                    new Guitar { Type = "Acoustic", Body = "Dreadnought", Brand = "Fender", Name = "CD-60SCE Nat WN", Price = 259, Availability = 69, Interest = 10 },
                    new Guitar { Type = "Acoustic", Body = "Dreadnought", Brand = "Fender", Name = "CD-60 NA V3", Price = 139, Availability = 82, Interest = 9 },
                    new Guitar { Type = "Acoustic", Body = "Dreadnought", Brand = "Harley Benton", Name = "D-120CE BK", Price = 89, Availability = 0, Interest = 7 },
                    new Guitar { Type = "Acoustic", Body = "Dreadnought", Brand = "Harley Benton", Name = "D-120CE", Price = 89, Availability = 82, Interest = 6 },
                    new Guitar { Type = "Acoustic", Body = "Dreadnought", Brand = "Martin Guitar", Name = "D-300", Price = 369000, Availability = 1, Interest = 1 },
                    new Guitar { Type = "Electric", Body = "ST-style", Brand = "Squier", Name = "Sonic Start MN 2TSB", Price = 179, Availability = 0, Interest = 5 },
                    new Guitar { Type = "Electric", Body = "ST-style", Brand = "Squier", Name = "CV 70s Strat Black", Price = 379, Availability = 61, Interest = 3 },
                    new Guitar { Type = "Electric", Body = "ST-style", Brand = "Squier", Name = "CV 60s Start CAR", Price = 389, Availability = 49, Interest = 4 },
                    new Guitar { Type = "Electric", Body = "ST-style", Brand = "Jackson", Name = "JS22 Dinky BLK AH", Price = 229, Availability = 72, Interest = 8 },
                    new Guitar { Type = "Electric", Body = "ST-style", Brand = "Jackson", Name = "JS12 Dinky MR AH", Price = 189, Availability = 52, Interest = 9 },
                    new Guitar { Type = "Electric", Body = "ST-style", Brand = "Fender", Name = "Standard strat MN WPG OWT", Price = 599, Availability = 24, Interest = 7 },
                    new Guitar { Type = "Electric", Body = "ST-style", Brand = "Fender", Name = "Am Ultra II Strat HSS EB TXT", Price = 2480, Availability = 13, Interest = 5 },
                    new Guitar { Type = "Electric", Body = "ST-style", Brand = "Fender", Name = "Player II Strat HSS MN BLK", Price = 759, Availability = 17, Interest = 5 },
                    new Guitar { Type = "Electric", Body = "ST-style", Brand = "Fender", Name = "Am Perf Strat MN Satin LPB", Price = 1359, Availability = 21, Interest = 4 },
                    new Guitar { Type = "Electric", Body = "T-style", Brand = "Fender", Name = "Player II Tele MN BTB", Price = 799, Availability = 14, Interest = 5 },
                    new Guitar { Type = "Electric", Body = "T-style", Brand = "Fender", Name = "Standard Tele MN BPG BTB", Price = 599, Availability = 3, Interest = 7 },
                    new Guitar { Type = "Electric", Body = "T-style", Brand = "Fender", Name = "Am Ultra II Tele MN UBST", Price = 2266, Availability = 5, Interest = 2 },
                    new Guitar { Type = "Electric", Body = "T-style", Brand = "Fender", Name = "AM Perf Tele HUM MN 3CSB", Price = 1479, Availability = 7, Interest = 3 },
                    new Guitar { Type = "Electric", Body = "T-style", Brand = "Squier", Name = "Sonic Tele MN ButterscotchB", Price = 169, Availability = 51, Interest = 7 },
                    new Guitar { Type = "Electric", Body = "T-style", Brand = "Squier", Name = "CV 60s Custom Tele 3-SB", Price = 409, Availability = 19, Interest = 4 },
                    new Guitar { Type = "Electric", Body = "T-style", Brand = "Squier", Name = "Sonic Tele LRL Torino Red", Price = 168, Availability = 41, Interest = 5 },
                    new Guitar { Type = "Electric", Body = "T-style", Brand = "Harley Benton", Name = "TE-52 NA Vintage Series", Price = 149, Availability = 42, Interest = 4 },
                    new Guitar { Type = "Electric", Body = "T-style", Brand = "Harley Benton", Name = "TE-62CC SFG", Price = 169, Availability = 29, Interest = 3 },
                    new Guitar { Type = "Electric", Body = "Single Cut", Brand = "Epiphone", Name = "Les Paul 59 Tobacco Burst VOS", Price = 1329, Availability = 14, Interest = 3 },
                    new Guitar { Type = "Electric", Body = "Single Cut", Brand = "Epiphone", Name = "Les Paul 59 Factory Burst VOS", Price = 1349, Availability = 0, Interest = 4 },
                    new Guitar { Type = "Electric", Body = "Single Cut", Brand = "Epiphone", Name = "SIGNATURE Kirk Hammett Greeny LP", Price = 1249, Availability = 6, Interest = 5 },
                    new Guitar { Type = "Electric", Body = "Single Cut", Brand = "Epiphone", Name = "Les Paul Standard 50s HCS", Price = 599, Availability = 21, Interest = 8 },
                    new Guitar { Type = "Electric", Body = "Single Cut", Brand = "Epiphone", Name = "Les Paul Standard 50s VSS", Price = 599, Availability = 1, Interest = 7 },
                    new Guitar { Type = "Electric", Body = "Single Cut", Brand = "Epiphone", Name = "Les Paul Classic Ebony", Price = 529, Availability = 15, Interest = 4 },
                    new Guitar { Type = "Electric", Body = "Single Cut", Brand = "Epiphone", Name = "Les Paul Standard 60s Ebony", Price = 563, Availability = 19, Interest = 3 },
                    new Guitar { Type = "Electric", Body = "Single Cut", Brand = "Gibson", Name = "Les Paul SIGNATURE Slash Standard NB", Price = 2789, Availability = 4, Interest = 2 },
                    new Guitar { Type = "Electric", Body = "Single Cut", Brand = "Gibson", Name = "Les Paul Standard 60s BB", Price = 2499, Availability = 9, Interest = 2 },
                    new Guitar { Type = "Electric", Body = "Single Cut", Brand = "Gibson", Name = "Les Paul Standard 50s HCSs", Price = 2499, Availability = 8, Interest = 2 },
                    new Guitar { Type = "Electric", Body = "Single Cut", Brand = "Gibson", Name = "Les Paul Modern Figured CB", Price = 2899, Availability = 6, Interest = 2 },
                    new Guitar { Type = "Electric", Body = "Single Cut", Brand = "Gibson", Name = "Les Paul Standard 50s GT", Price = 2539, Availability = 4, Interest = 1 },
                    new Guitar { Type = "Electric", Body = "Single Cut", Brand = "Gibson", Name = "Les Paul Studio BlueberryBurst", Price = 1789, Availability = 2, Interest = 4 },
                    new Guitar { Type = "Electric", Body = "Single Cut", Brand = "Gibson", Name = "Les Paul 59 Washed Cherry VOS", Price = 6299, Availability = 3, Interest = 2 },
                    new Guitar { Type = "Electric", Body = "Single Cut", Brand = "Gibson", Name = "Les Paul Modern Figured SFG", Price = 2699, Availability = 4, Interest = 1 },
                    new Guitar { Type = "Electric", Body = "Single Cut", Brand = "Harley Benton", Name = "SC-550 II FTF", Price = 299, Availability = 21, Interest = 7 },
                    new Guitar { Type = "Electric", Body = "Single Cut", Brand = "Harley Benton", Name = "SC-Custom III VBK", Price = 339, Availability = 15, Interest = 4 },
                    new Guitar { Type = "Electric", Body = "Single Cut", Brand = "Harley Benton", Name = "SC-500 BK Vintage Series", Price = 169, Availability = 17, Interest = 6 },
                    new Guitar { Type = "Electric", Body = "Double Cut", Brand = "Epiphone", Name = "SG Standard Ebony", Price = 499, Availability = 21, Interest = 4 },
                    new Guitar { Type = "Electric", Body = "Double Cut", Brand = "Epiphone", Name = "SG Standard Heritage Cherry", Price = 473, Availability = 19, Interest = 7 },
                    new Guitar { Type = "Electric", Body = "Double Cut", Brand = "Epiphone", Name = "SG 1961 Les Paul SG Standard CH", Price = 908, Availability = 8, Interest = 5 },
                    new Guitar { Type = "Electric", Body = "Double Cut", Brand = "Epiphone", Name = "SG Special P90 Sparkling Burg", Price = 417, Availability = 0, Interest = 7 },
                    new Guitar { Type = "Electric", Body = "Double Cut", Brand = "Gibson", Name = "SG 61 Standard VC", Price = 1888, Availability = 18, Interest = 2 },
                    new Guitar { Type = "Electric", Body = "Double Cut", Brand = "Gibson", Name = "SG Standard HC", Price = 1555, Availability = 14, Interest = 4 },
                    new Guitar { Type = "Electric", Body = "Double Cut", Brand = "Gibson", Name = "SG Standard TV Yellow", Price = 1449, Availability = 12, Interest = 4 },
                    new Guitar { Type = "Electric", Body = "Double Cut", Brand = "Gibson", Name = "SG Modern BBF", Price = 2199, Availability = 6, Interest = 2 },
                    new Guitar { Type = "Electric", Body = "Semiacoustic", Brand = "Epiphone", Name = "SIGNATURE Dave Grohl DG-335 Pelham Blue", Price = 999, Availability = 7, Interest = 4 },
                    new Guitar { Type = "Electric", Body = "Semiacoustic", Brand = "Epiphone", Name = "SIGNATURE Marty Schwartz ES-335 SC", Price = 868, Availability = 14, Interest = 5 },
                    new Guitar { Type = "Electric", Body = "Semiacoustic", Brand = "Epiphone", Name = "ES-335 Cherry", Price = 575, Availability = 24, Interest = 8 },
                    new Guitar { Type = "Electric", Body = "Semiacoustic", Brand = "Epiphone", Name = "ES-335 Vintage Sunburst", Price = 531, Availability = 17, Interest = 5 },
                    new Guitar { Type = "Electric", Body = "Semiacoustic", Brand = "Epiphone", Name = "Casino Vintage Sunburst", Price = 649, Availability = 0, Interest = 4 },
                    new Guitar { Type = "Electric", Body = "Semiacoustic", Brand = "Gibson", Name = "ES-335 Figured 60s Cherry", Price = 3499, Availability = 8, Interest = 5 },
                    new Guitar { Type = "Electric", Body = "Semiacoustic", Brand = "Gibson", Name = "ES-339 Trans Ebony", Price = 2629, Availability = 9, Interest = 4 },
                    new Guitar { Type = "Electric", Body = "Semiacoustic", Brand = "Gibson", Name = "ES-339 60s Cherry", Price = 2349, Availability = 11, Interest = 4 },
                    new Guitar { Type = "Electric", Body = "Semiacoustic", Brand = "Gibson", Name = "ES-335 Satin Cherry", Price = 2499, Availability = 10, Interest = 3 },
                    new Guitar { Type = "Electric", Body = "Semiacoustic", Brand = "Gibson", Name = "ES-335 Dot Vintage Burst", Price = 2777, Availability = 5, Interest = 2 },
                    new Guitar { Type = "Electric", Body = "Semiacoustic", Brand = "Gretsch", Name = "Pro nashville Bigsby CAD GRN", Price = 2829, Availability = 3, Interest = 2 },
                    new Guitar { Type = "Electric", Body = "Semiacoustic", Brand = "Gretsch", Name = "G5420T Electromatic AM", Price = 764, Availability = 5, Interest = 4 },
                    new Guitar { Type = "Electric", Body = "Heavy", Brand = "Gibson", Name = "70s Flying V CW", Price = 2589, Availability = 15, Interest = 4 },
                    new Guitar { Type = "Electric", Body = "Heavy", Brand = "Gibson", Name = "70s Explorer Antique Natural", Price = 2439, Availability = 1, Interest = 6 },
                    new Guitar { Type = "Electric", Body = "Heavy", Brand = "Epiphone", Name = "SIGNATURE Kirk Hammett 1979 Flying V EB", Price = 1111, Availability = 10, Interest = 4 },
                    new Guitar { Type = "Electric", Body = "Heavy", Brand = "Jackson", Name = "JS32 King V AH BK", Price = 369, Availability = 15, Interest = 9 },
                    new Guitar { Type = "Electric", Body = "Heavy", Brand = "Jackson", Name = "JS32 Rhoads AH BK", Price = 369, Availability = 11, Interest = 8 },
                    new Guitar { Type = "Electric", Body = "Heavy", Brand = "Jackson", Name = "JS32T Rhoads AH SBK", Price = 319, Availability = 14, Interest = 6 },
                    new Guitar { Type = "Electric", Body = "Heavy", Brand = "Jackson", Name = "KVXMG King V Satin BK", Price = 758, Availability = 11, Interest = 8 }
                );
                context.SaveChanges();
            }
        }
        public static void InitializeShops(IServiceProvider serviceProvider)
        {
            using (var context = new GuitarShopContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<GuitarShopContext>>()))
            {
                if (context == null || context.Shop == null)
                {
                    throw new ArgumentNullException("Null RazorPagesMovieContext");
                }

                // Look for any movies.
                if (context.Shop.Any())
                {
                    return;   // DB has been seeded
                }
                context.Shop.AddRange(
                    new Shop { Town = "Aytos", Name = "Na Misho kitarite" },
                    new Shop { Town = "Varna", Name = "Morski darove" },
                    new Shop { Town = "Burgas", Name = "Mazni kitari" },
                    new Shop { Town = "Sofia", Name = "\"Shopski\" kitari" },
                    new Shop { Town = "Ruse", Name = "Pochti rumunski kitari" },
                    new Shop { Town = "Plovdiv", Name = "Kitari maina" },
                    new Shop { Town = "Stara Zagora", Name = "Nova zagora ne stavat" },
                    new Shop { Town = "Sredets", Name = "Na Miro kitarite" },
                    new Shop { Town = "Pleven", Name = "Kitari duge" },
                    new Shop { Town = "Vidin", Name = "Za babite" },
                    new Shop { Town = "Gabrovo", Name = "Kitari ot Etura" }
                );

                context.SaveChanges();
            }
        }
    }
}
