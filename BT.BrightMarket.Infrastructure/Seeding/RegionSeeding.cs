using BT.BrightMarket.Domain.Models.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BT.BrightMarket.Infrastructure.Seeding
{
    public static class RegionSeeding
    {
        public static void Seed(this EntityTypeBuilder<Region> modelBuilder)
        {

            modelBuilder.HasData(
                 new Region()
                 {
                     Id = 1,
                     Name = "Kontich"
                 },
                new Region()
                {
                    Id = 2,
                    Name = "Genk"
                },
                new Region()
                {
                    Id = 3,
                    Name = "Ghent"
                }
            );
        }
    }
}
