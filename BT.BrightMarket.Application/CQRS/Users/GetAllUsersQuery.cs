using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Users;
using MediatR;

namespace BT.BrightMarket.Application.CQRS.Users
{
    public class GetAllUsersQuery : IRequest<IEnumerable<User>> { }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<User>> 
    {
        private readonly IUnitofWork uow;
        public GetAllUsersQueryHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task<IEnumerable<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return await uow.UsersRepository.GetAll();
        }
    }

}
