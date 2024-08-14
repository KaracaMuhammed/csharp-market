using BT.BrightMarket.Domain.Models.Products;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BT.BrightMarket.Infrastructure.Seeding
{
    public static class CategorySeeding
    {
        public static void Seed(this EntityTypeBuilder<Category> modelBuilder)
        {

            modelBuilder.HasData(
                 new Category()
                 {
                     Id = 1,
                     Name = "Kleding en accessoires"
                 },
                new Category()
                {
                    Id = 2,
                    Name = "Elektronica"
                },
                new Category()
                {
                    Id = 3,
                    Name = "Voertuigen"
                },
                new Category()
                {
                    Id = 4,
                    Name = "Huis en tuin"
                },
                new Category()
                {
                    Id = 5,
                    Name = "Sport en recreatie"
                },
                new Category()
                {
                    Id = 6,
                    Name = "Huisdieren"
                },
                new Category()
                {
                    Id = 7,
                    Name = "Boeken en tijdschriften"
                },
                new Category()
                {
                    Id = 8,
                    Name = "Verzamelobjecten"
                },
                new Category()
                {
                    Id = 9,
                    Name = "Muziekinstrumenten"
                },
                new Category()
                { 
                    Id = 10,
                    Name = "Gezondheid en schoonheid"
                }
            );
        }
    }
}
