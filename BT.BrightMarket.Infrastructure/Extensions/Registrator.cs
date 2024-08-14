using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Infrastructure.Contexts;
using BT.BrightMarket.Infrastructure.Repositories;
using BT.BrightMarket.Infrastructure.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BT.BrightMarket.Infrastructure.Extensions
{
    public static class Registrator
    {
        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
        {
            services.RegisterDbContext();
            services.RegisterRepositories();
            return services;
        }
        public static IServiceCollection RegisterDbContext(this IServiceCollection services)
        {
            services.AddDbContext<BrightMarketContext>(options =>
                        options.UseSqlServer("name=ConnectionStrings:BrightMarketDatabase"));
            return services;
        }

        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitofWork, UnitofWork>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IRegionsRepository, RegionsRepository>();
            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            services.AddScoped<IImagesRepository, ImagesRepository>();
            services.AddScoped<IConversationsRepository, ConversationsRepository>();
            services.AddScoped<IMessagesRepository, MessagesRepository>();
            return services;
        }
    }
}
