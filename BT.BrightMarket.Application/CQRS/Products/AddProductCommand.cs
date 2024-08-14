using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.DTOs;
using BT.BrightMarket.Domain.Models.Products;
using FluentValidation;
using MediatR;
using SixLabors.ImageSharp.Formats;

namespace BT.BrightMarket.Application.CQRS.Products
{
    public class AddProductCommand : IRequest<Product>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public Status Status { get; set; }
        public int AuthenticatedUserId { get; set; }
        public int CategoryId { get; set; }
        public Duration DisplayDuration { get; set; }
        public List<ImageDTO> Images { get; set; }
        public ItemType ItemType { get; set; }
    }

    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        private readonly string[] supportedFormats = { "image/jpeg", "image/png" };
        public AddProductCommandValidator(IUnitofWork uow)
        {

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("The product name cannot be empty.")
                .MaximumLength(40).WithMessage("The product name cannot exceed 40 characters.");

            RuleFor(p => p.Description)
                .MaximumLength(450).WithMessage("Description cannot exceed 450 characters.");

            RuleFor(p => p.Price)
                .NotEmpty().WithMessage("Price is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Price must be a non-negative value.")
                .Must(price => Math.Round(price, 2) == price).WithMessage("Price can have up to two decimal places.");

            RuleFor(p => p.DisplayDuration)
                .IsInEnum().WithMessage("Invalid display duration value.");

            RuleFor(p => p.Images)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("At least one image is required.")
                .Must(images => images.Count <= 5).WithMessage("Maximum of 5 images allowed.")
                .Must(ValidateImages).WithMessage("Invalid image format or size.");

            RuleFor(p => p.ItemType)
                .NotEmpty().WithMessage("Item type is required.")
                .Must(type => type != null && type != ItemType.Undefined)
                .WithMessage("Invalid item type.");

        }

        private bool ValidateImages(List<ImageDTO> images)
        {
            foreach (var image in images)
            {
                if (!IsValidImage(image.Data))
                    return false;
            }
            return true;
        }

        private bool IsValidImage(byte[] imageData)
        {
            try
            {
                using (var ms = new MemoryStream(imageData))
                {
                    // Detect image format
                    var format = SixLabors.ImageSharp.Image.DetectFormat(ms);

                    // Reset the stream position to the beginning
                    ms.Seek(0, SeekOrigin.Begin);

                    // Check image size
                    long imageSize = ms.Length;
                    if (imageSize > 7 * 1024 * 1024)
                    {
                        return false; // Image size exceeds the limit
                    }

                    // Load image
                    using (var image = SixLabors.ImageSharp.Image.Load(ms))
                    {
                        // Image format is valid; check if it's supported
                        return IsSupportedImageFormat(format);
                    }
                }
            }
            catch
            {
                return false; // Error occurred, image is invalid
            }
        }


        private bool IsSupportedImageFormat(IImageFormat format)
        {
            string mimeType = $"image/{format.Name.ToLower()}";
            return supportedFormats.Contains(mimeType);
        }

    }



    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, Product>
    {
        private readonly IUnitofWork uow;
        public AddProductCommandHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task<Product> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {

            Product product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Status = Status.Available,
                UserId = request.AuthenticatedUserId,
                User = await uow.UsersRepository.GetById(request.AuthenticatedUserId),
                CreationDate = DateTime.Now,
                CategoryId = request.CategoryId,
                DisplayDuration = request.DisplayDuration,
                ItemType = request.ItemType
            };

            product = await uow.ProductsRepository.Create(product);
            await uow.Commit();

            if (request.Images != null)
            {
                List<Image> images = new List<Image>();

                foreach (var image in request.Images)
                    images.Add(new Image { Data = image.Data, ProductId = product.Id });

                await uow.ImagesRepository.CreateRange(images);
                await uow.Commit();
            }

            return product;
        }


    }

}
