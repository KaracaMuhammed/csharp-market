using BT.BrightMarket.Application.Exceptions;
using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Users;
using MediatR;

namespace BT.BrightMarket.Application.CQRS.Users
{
    public class GetUserByIdQuery : IRequest<User>
    {
        public int Id { get; set; }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User>
    {
        private readonly IUnitofWork uow;
        public GetUserByIdQueryHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await uow.UsersRepository.GetById(request.Id) ?? throw new RelationNotFoundException($"User with id {request.Id} could not be found");
            return user;
        }
    }
}
