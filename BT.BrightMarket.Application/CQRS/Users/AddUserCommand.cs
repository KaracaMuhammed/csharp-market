using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Users;
using FluentValidation;
using MediatR;

namespace BT.BrightMarket.Application.CQRS.Users
{
    public class AddUserCommand : IRequest<User>
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public Role Role { get; set; }
        public int RegionId { get; set; }

    }

    public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
    {
        public AddUserCommandValidator(IUnitofWork uow)
        {

            // empty & duplicate check
            RuleFor(u => u.Email)
                .NotEmpty()
                .WithMessage("Email cannot be empty.")
                .MustAsync(async (email, cancellation) =>
                {
                    var user = await uow.UsersRepository.GetByEmail(email);
                    return user == null ? true : false;
                })
                .WithMessage("The email already exists.");

            // name not null check
            RuleFor(u => u.Name)
                .NotEmpty()
                .WithMessage("Name cannot be empty.");

            // role validity check
            RuleFor(u => u.Role)
                .Must(role => Enum.IsDefined(typeof(Role), role))
                .WithMessage("Role is not valid.");

            // null & validity check
            RuleFor(u => u.RegionId)
                .NotEmpty()
                .WithMessage("Region ID cannot be empty.")
                .MustAsync(async (regionId, cancellation) =>
                {
                    return await uow.RegionsRepository.Exists(regionId);
                })
                .WithMessage($"The specified region ID does not exist.");

        }
    }

    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, User>
    {
        private readonly IUnitofWork uow;
        public AddUserCommandHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task<User> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {

            User user = new User
            {
                Email = request.Email,
                Name = request.Name,
                Role = request.Role,
                RegionId = request.RegionId,
                Region = await uow.RegionsRepository.GetById(request.RegionId)
            };

            await uow.UsersRepository.Create(user);
            await uow.Commit();

            return user;
        }


    }

}
