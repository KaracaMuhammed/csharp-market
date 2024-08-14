using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Infrastructure.Contexts;

namespace BT.BrightMarket.Infrastructure.UoW
{
    public class UnitofWork : IUnitofWork
    {
        private readonly BrightMarketContext ctxt;
        private readonly IUsersRepository usersRepo;
        private readonly IRegionsRepository regionsRepo;
        private readonly IProductsRepository productsRepo;
        private readonly ICategoriesRepository categoriesRepo;
        private readonly IImagesRepository imagesRepo;
        private readonly IConversationsRepository conversationsRepo;
        private readonly IMessagesRepository messagesRepo;

        public UnitofWork(BrightMarketContext ctxt, IUsersRepository usersRepo, IRegionsRepository regionsRepo, IProductsRepository productsRepo, ICategoriesRepository categoriesRepo, IImagesRepository imagesRepo, IConversationsRepository conversationsRepo, IMessagesRepository messagesRepo)
        {
            this.ctxt = ctxt;
            this.usersRepo = usersRepo;
            this.regionsRepo = regionsRepo;
            this.productsRepo = productsRepo;
            this.categoriesRepo = categoriesRepo;
            this.imagesRepo = imagesRepo;
            this.conversationsRepo = conversationsRepo;
            this.messagesRepo = messagesRepo;
        }

        public IUsersRepository UsersRepository => usersRepo;
        public IRegionsRepository RegionsRepository => regionsRepo;
        public IProductsRepository ProductsRepository => productsRepo;
        public ICategoriesRepository CategoriesRepository => categoriesRepo;
        public IImagesRepository ImagesRepository => imagesRepo;
        public IConversationsRepository ConversationsRepository => conversationsRepo;
        public IMessagesRepository MessagesRepository => messagesRepo;

        public async Task Commit()
        {
            await ctxt.SaveChangesAsync();
        }
    }
}
