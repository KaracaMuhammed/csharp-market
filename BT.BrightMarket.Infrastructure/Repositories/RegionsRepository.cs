using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Users;
using BT.BrightMarket.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BT.BrightMarket.Infrastructure.Repositories
{
    public class RegionsRepository : IRegionsRepository
    {

        private readonly BrightMarketContext context;
        public RegionsRepository(BrightMarketContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Region>> GetAll()
        {
            return await context.Regions.ToListAsync();
        }

        public async Task<Region> GetById(int id)
        {
            return await context.Regions.FirstOrDefaultAsync(r => r.Id.Equals(id));
        }

        public async Task<bool> Exists(int id)
        {
            return await context.Regions.AnyAsync(r => r.Id.Equals(id));
        }

    }
}
