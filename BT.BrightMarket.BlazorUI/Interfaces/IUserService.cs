using BT.BrightMarket.Shared.DTOs;
using BT.BrightMarket.Shared.Models.Users;

namespace BT.BrightMarket.BlazorUI.Interfaces
{
    public interface IUserService
    {
        Task<User> GetPersonalUserAsync();
        Task<User> CreateUserAsync(UserDTO.PostUserObject user);

    }
}
