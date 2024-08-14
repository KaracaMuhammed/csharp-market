using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.DTOs;
using BT.BrightMarket.Domain.Models.Products;
using BT.BrightMarket.Domain.Models.Users;
using FluentValidation;
using MediatR;
using SixLabors.ImageSharp.Formats;

namespace BT.BrightMarket.Application.CQRS.Products
{
    public class UpdateProductCommand : IRequest<Product>
    {
        public int ProductId { get; set; }
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
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {

        private readonly string[] supportedFormats = { "image/jpeg", "image/png" };
        public UpdateProductCommandValidator(IUnitofWork uow)
        {

            RuleFor(r => r)
                .MustAsync(async (request, cancellationToken, command) =>
                {
                    User user = await uow.UsersRepository.GetById(request.AuthenticatedUserId);
                    Product product = await uow.ProductsRepository.GetById(request.ProductId);

                    bool isAdmin = user.Role == Role.Admin;
                    bool isOwner = product.UserId == request.AuthenticatedUserId;
                    return isAdmin || isOwner; // user has to be ADMIN or product owner to be able to edit

                })
                .WithMessage("Not allowed to update this product.");

            RuleFor(p => p.ProductId)
                .MustAsync(async (id, cancellation) => await uow.ProductsRepository.ProductExistsAsync(id))
                .WithMessage("Product does not exist.");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("The product name cannot be empty.")
                .MaximumLength(40).WithMessage("The product name cannot exceed 40 characters.");

            RuleFor(p => p.Description)
                .MaximumLength(450).WithMessage("Description cannot exceed 400 characters.");

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
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Product>
    {
        private readonly IUnitofWork uow;
        public UpdateProductCommandHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var currentProduct = await uow.ProductsRepository.GetById(request.ProductId);

            if (currentProduct != null)
            {
                currentProduct.Name = request.Name;
                currentProduct.Description = request.Description;
                currentProduct.Price = request.Price;
                currentProduct.Status = currentProduct.Status;
                currentProduct.UserId = currentProduct.UserId;
                currentProduct.User = currentProduct.User;
                currentProduct.CreationDate = currentProduct.CreationDate;
                currentProduct.CategoryId = request.CategoryId;
                currentProduct.DisplayDuration = request.DisplayDuration;
                currentProduct.Images = new List<Image>();
                //currentProduct.ItemType = request.ItemType; // ItemType cannot be updated afterwards

                var updatedImages = request.Images.Select(image => image.Data).ToList();
                var currentImages = await uow.ImagesRepository.GetAllImagesByProductId(request.ProductId);
                var currentImageDatas = currentImages.Select(image => image.Data).ToList();

                // Add new images
                var newImages = updatedImages.Except(currentImageDatas).ToList();
                foreach (var imageData in newImages)
                {
                    var newImage = new Image { Data = imageData };
                    currentProduct.Images.Add(newImage);
                }

                // Remove images not present in updated images
                var removedImages = currentImageDatas.Except(updatedImages).ToList();
                foreach (var imageData in removedImages)
                {
                    var removedImage = currentProduct.Images.FirstOrDefault(image => image.Data == imageData);
                    if (removedImage != null)
                    {
                        currentProduct.Images.Remove(removedImage);
                    }
                }

            }

            await uow.ProductsRepository.Update(currentProduct);

            await uow.Commit();
            return currentProduct;
        }
    }
}
