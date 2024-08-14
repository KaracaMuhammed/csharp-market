using BT.BrightMarket.Application.Exceptions;
using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Users;
using MediatR;

namespace BT.BrightMarket.Application.CQRS.Users
{
    public class GetPersonalUserQuery : IRequest<User>
    {
        public int AuthenticatedUserId { get; set; }
    }

    public class GetPersonalUserQueryHandler : IRequestHandler<GetPersonalUserQuery, User>
    {
        private readonly IUnitofWork uow;
        public GetPersonalUserQueryHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task<User> Handle(GetPersonalUserQuery request, CancellationToken cancellationToken)
        {
            var user = await uow.UsersRepository.GetById(request.AuthenticatedUserId) ?? throw new RelationNotFoundException($"User with id {request.AuthenticatedUserId} could not be found");
            return user;
        }
    }
}
