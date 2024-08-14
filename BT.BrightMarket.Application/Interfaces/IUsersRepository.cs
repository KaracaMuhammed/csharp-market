using BT.BrightMarket.Domain.Models.Users;

namespace BT.BrightMarket.Application.Interfaces
{
    public interface IUsersRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);
        Task<User> GetByEmail(string email);
        Task<User> Create(User user);
        Task<bool> Exists(string email);
    }
}
