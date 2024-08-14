namespace BT.BrightMarket.Application.Interfaces
{
    public interface IUnitofWork
    {
        public IUsersRepository UsersRepository { get; }
        public IRegionsRepository RegionsRepository { get; }
        public IProductsRepository ProductsRepository { get; }
        public ICategoriesRepository CategoriesRepository { get; }
        public IImagesRepository ImagesRepository { get; }
        public IConversationsRepository ConversationsRepository { get; }
        public IMessagesRepository MessagesRepository { get; }
        public Task Commit();
    }
}
