using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Users;
using BT.BrightMarket.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BT.BrightMarket.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly BrightMarketContext context;
        public UsersRepository(BrightMarketContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await context.Users
                .Include(u => u.Region)
                .ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await context.Users
                .Include(u => u.Region)
                .FirstOrDefaultAsync(u => u.Id.Equals(id));
        }

        public async Task<User> GetByEmail(string email)
        {
            return await context.Users
                .Include(u => u.Region)
                .FirstOrDefaultAsync(u => u.Email.Equals(email));
        }

        public async Task<User> Create(User user) 
        {
            await context.Users.AddAsync(user);
            return user;
        }

        public async Task<bool> Exists(string email)
        {
            return await context.Users.AnyAsync(u => u.Email == email);
        }

    }
}
