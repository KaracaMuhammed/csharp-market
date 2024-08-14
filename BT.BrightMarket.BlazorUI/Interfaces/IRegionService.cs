using BT.BrightMarket.Shared.Models.Users;

namespace BT.BrightMarket.BlazorUI.Interfaces
{
    public interface IRegionService
    {
        Task<List<Region>> GetAllRegionsAsync();
    }
}
