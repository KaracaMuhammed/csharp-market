using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Products;
using FluentValidation;
using MediatR;

namespace BT.BrightMarket.Application.CQRS.Categories
{
    public class AddCategoryCommand : IRequest<Category>
    {
        public string Name { get; set; }
    }

    public class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommand>
    {
        public AddCategoryCommandValidator(IUnitofWork uow)
        {

            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("The category name cannot be empty.")
                .MaximumLength(40).WithMessage("The category name cannot exceed 40 characters.");

        }
    }

    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, Category>
    {
        private readonly IUnitofWork uow;
        public AddCategoryCommandHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task<Category> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {

            Category category = new Category
            {
                Name = request.Name
            };

            await uow.CategoriesRepository.Create(category);
            await uow.Commit();

            return category;
        }

    }

}
