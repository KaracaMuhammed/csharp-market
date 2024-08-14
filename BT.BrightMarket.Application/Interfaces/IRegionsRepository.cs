using BT.BrightMarket.Domain.Models.Users;

namespace BT.BrightMarket.Application.Interfaces
{
    public interface IRegionsRepository
    {
        Task<IEnumerable<Region>> GetAll();
        Task<Region> GetById(int id);
        Task<bool> Exists(int id);
    }
}
