using BT.BrightMarket.Application.Exceptions;
using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Users;
using MediatR;

namespace BT.BrightMarket.Application.CQRS.Users
{
    public class GetUserByEmailQuery : IRequest<User>
    {
        public string Email { get; set; }
    }

    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, User>
    {
        private readonly IUnitofWork uow;
        public GetUserByEmailQueryHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task<User> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await uow.UsersRepository.GetByEmail(request.Email) ?? throw new RelationNotFoundException($"No user found for the entered email address");
            return user;
        }
    }
}
