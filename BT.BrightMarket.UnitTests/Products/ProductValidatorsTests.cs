using BT.BrightMarket.Application.CQRS.Products;
using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.DTOs;
using BT.BrightMarket.Domain.Models.Products;
using BT.BrightMarket.Domain.Models.Users;
using FluentValidation.TestHelper;
using Moq;

namespace YourNamespace.UnitTests.Products
{
    [TestClass]
    public class AddProductValidatorTests
    {

        private Mock<IUnitofWork> uowMock;
        private Mock<IProductsRepository> productRepoMock;

        [TestInitialize]
        public void Setup()
        {
            uowMock = new Mock<IUnitofWork>();
            productRepoMock = new Mock<IProductsRepository>();
            uowMock.Setup((u) => u.ProductsRepository).Returns(productRepoMock.Object);
        }

        [TestMethod]
        public async Task AddProductWithNonEmptyNamePositiveTest()
        {
            // Arrange
            var command = new AddProductCommand
            {
                Name = "Test Product",
                Description = "Sample description",
                Price = 10.99,
                ItemType = ItemType.Undefined
            };

            // Act
            var validator = new AddProductCommandValidator(uowMock.Object);
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(p => p.Name);
        }

        [TestMethod]
        public async Task AddProductWithEmptyNameNegativeTest()
        {
            // Arrange
            var command = new AddProductCommand
            {
                Name = "", // Empty name
                Description = "Sample description",
                Price = 10.99
            };

            // Act
            var validator = new AddProductCommandValidator(uowMock.Object);
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Name)
                .WithErrorMessage("The product name cannot be empty.");
        }

        [TestMethod]
        public async Task AddProductWithTooLongNameNegativeTest()
        {
            // Arrange
            var command = new AddProductCommand
            {
                Name = "ThisProductNameIsTooLongAndExceedsTheMaximumLengthAllowed", // Name exceeds max length
                Description = "Sample description",
                Price = 10.99
            };

            // Act
            var validator = new AddProductCommandValidator(uowMock.Object);
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Name)
                .WithErrorMessage("The product name cannot exceed 40 characters.");
        }

        [TestMethod]
        public async Task AddProductWithInvalidPriceNegativeTest()
        {
            // Arrange
            var command = new AddProductCommand
            {
                Name = "Test Product",
                Description = "Sample description",
                Price = -10.99 // Negative price
            };

            // Act
            var validator = new AddProductCommandValidator(uowMock.Object);
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Price)
                .WithErrorMessage("Price must be a non-negative value.");
        }

        [TestMethod]
        public async Task AddProductWithTooLongDescriptionNegativeTest()
        {
            // Arrange
            var command = new AddProductCommand
            {
                Name = "Test Product",
                Description = "This description is too long and exceeds the maximum length allowed. This description is too long and exceeds the maximum length allowed. This description is too long and exceeds the maximum length allowed. This description is too long and exceeds the maximum length allowed. This description is too long and exceeds the maximum length allowed. This description is too long and exceeds the maximum length allowed. This description is too long and exceeds the maximum length allowed. This description is too long and exceeds the maximum length allowed.", // Description exceeds max length
                Price = 10.99
            };

            // Act
            var validator = new AddProductCommandValidator(uowMock.Object);
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Description)
                .WithErrorMessage("Description cannot exceed 450 characters.");
        }

        [TestMethod]
        public async Task AddProductWithInvalidDisplayDurationNegativeTest()
        {
            // Arrange
            var command = new AddProductCommand
            {
                Name = "Test Product",
                Description = "Sample description",
                Price = 10.99,
                DisplayDuration = (Duration)99 // Invalid display duration
            };

            // Act
            var validator = new AddProductCommandValidator(uowMock.Object);
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.DisplayDuration)
                .WithErrorMessage("Invalid display duration value.");
        }

        [TestMethod]
        public async Task DeleteProductWithExistingProductIdShouldPass()
        {
            // Arrange
            var authenticatedUserId = 1; // Example authenticated user ID
            var productId = 1; // Example product ID
            var command = new DeleteProductCommand
            {
                Id = productId,
                AuthenticatedUserId = authenticatedUserId,
            };

            // Mock behavior to return a non-admin user (product owner)
            var ownerUser = new User { Id = authenticatedUserId, Role = Role.User };
            uowMock.Setup(u => u.UsersRepository.GetById(authenticatedUserId)).ReturnsAsync(ownerUser);

            // Mock behavior to return a product
            var product = new Product { Id = productId, UserId = authenticatedUserId }; // User owns the product
            uowMock.Setup(u => u.ProductsRepository.GetById(productId)).ReturnsAsync(product);

            // Setup mock behavior for ProductExistsAsync to return true for the provided ID
            productRepoMock.Setup(repo => repo.ProductExistsAsync(command.Id)).ReturnsAsync(true);

            // Act
            var validator = new DeleteProductCommandValidator(uowMock.Object);
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(p => p.Id);
        }


        [TestMethod]
        public async Task DeleteProductWithNonExistingProductIdShouldFail()
        {
            // Arrange
            var authenticatedUserId = 1; // Example authenticated user ID
            var productId = 1; // Example product ID
            var command = new DeleteProductCommand
            {
                Id = productId,
                AuthenticatedUserId = authenticatedUserId,
            };

            // Mock behavior to return a non-admin user (product owner)
            var ownerUser = new User { Id = authenticatedUserId, Role = Role.User };
            uowMock.Setup(u => u.UsersRepository.GetById(authenticatedUserId)).ReturnsAsync(ownerUser);

            // Mock behavior to return a product
            var product = new Product { Id = productId, UserId = authenticatedUserId }; // User owns the product
            uowMock.Setup(u => u.ProductsRepository.GetById(productId)).ReturnsAsync(product);

            // Setup mock behavior for ProductExistsAsync to return true for the provided ID
            productRepoMock.Setup(repo => repo.ProductExistsAsync(command.Id)).ReturnsAsync(false);

            var validator = new DeleteProductCommandValidator(uowMock.Object);
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Id)
                .WithErrorMessage("Product does not exist.");
        }

        [TestMethod]
        public async Task AddProductWithValidImagesPositiveTest()
        {
            // Arrange
            var command = new AddProductCommand
            {
                Name = "Test Product",
                Description = "Sample description",
                Price = 10.99,
                Images = GenerateValidImages(5)
            };

            // Act
            var validator = new AddProductCommandValidator(uowMock.Object);
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(p => p.Images);
        }

        [TestMethod]
        public async Task AddProductWithNoImagesNegativeTest()
        {
            // Arrange
            var command = new AddProductCommand
            {
                Name = "Test Product",
                Description = "Sample description",
                Price = 10.99,
                Images = new List<ImageDTO>()
            };

            // Act
            var validator = new AddProductCommandValidator(uowMock.Object);
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Images)
                .WithErrorMessage("At least one image is required.");
        }

        [TestMethod]
        public async Task AddProductWithTooManyImagesNegativeTest()
        {
            // Arrange
            var command = new AddProductCommand
            {
                Name = "Test Product",
                Description = "Sample description",
                Price = 10.99,
                Images = GenerateValidImages(6) // More than allowed
            };

            // Act
            var validator = new AddProductCommandValidator(uowMock.Object);
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Images)
                .WithErrorMessage("Maximum of 5 images allowed.");
        }

        [TestMethod]
        public async Task AddProductWithInvalidImageNegativeTest()
        {
            // Arrange
            var command = new AddProductCommand
            {
                Name = "Test Product",
                Description = "Sample description",
                Price = 10.99,
                Images = GenerateInvalidImages(1) // Invalid image
            };

            // Act
            var validator = new AddProductCommandValidator(uowMock.Object);
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Images)
                .WithErrorMessage("Invalid image format or size.");
        }

        [TestMethod]
        public async Task UpdateProductWithAdminRolePositiveTest()
        {
            // Arrange
            var authenticatedUserId = 1; // Example authenticated user ID
            var productId = 1; // Example product ID
            var command = new UpdateProductCommand
            {
                AuthenticatedUserId = authenticatedUserId,
                ProductId = productId
            };

            // Mock behavior to return an admin user
            var adminUser = new User { Id = authenticatedUserId, Role = Role.Admin };
            uowMock.Setup(u => u.UsersRepository.GetById(authenticatedUserId)).ReturnsAsync(adminUser);

            // Mock behavior to return a product
            var product = new Product { Id = productId, UserId = authenticatedUserId }; // User owns the product
            uowMock.Setup(u => u.ProductsRepository.GetById(productId)).ReturnsAsync(product);

            // Act
            var validator = new UpdateProductCommandValidator(uowMock.Object);
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(r => r);
        }

        [TestMethod]
        public async Task UpdateProductWithOwnerPositiveTest()
        {
            // Arrange
            var authenticatedUserId = 1; // Example authenticated user ID
            var productId = 1; // Example product ID
            var command = new UpdateProductCommand
            {
                AuthenticatedUserId = authenticatedUserId,
                ProductId = productId
            };

            // Mock behavior to return a non-admin user (product owner)
            var ownerUser = new User { Id = authenticatedUserId, Role = Role.User };
            uowMock.Setup(u => u.UsersRepository.GetById(authenticatedUserId)).ReturnsAsync(ownerUser);

            // Mock behavior to return a product
            var product = new Product { Id = productId, UserId = authenticatedUserId }; // User owns the product
            uowMock.Setup(u => u.ProductsRepository.GetById(productId)).ReturnsAsync(product);

            // Act
            var validator = new UpdateProductCommandValidator(uowMock.Object);
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(r => r);
        }

        [TestMethod]
        public async Task UpdateProductWithNonOwnerAndNonAdminNegativeTest()
        {
            // Arrange
            var authenticatedUserId = 1; // Example authenticated user ID
            var productId = 1; // Example product ID
            var command = new UpdateProductCommand
            {
                AuthenticatedUserId = authenticatedUserId,
                ProductId = productId
            };

            // Mock behavior to return a non-admin user (not product owner)
            var user = new User { Id = 2, Role = Role.User }; // User different from the product owner
            uowMock.Setup(u => u.UsersRepository.GetById(authenticatedUserId)).ReturnsAsync(user);

            // Mock behavior to return a product
            var product = new Product { Id = productId, UserId = 3 }; // Different user owns the product
            uowMock.Setup(u => u.ProductsRepository.GetById(productId)).ReturnsAsync(product);

            // Act
            var validator = new UpdateProductCommandValidator(uowMock.Object);
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r)
                .WithErrorMessage("Not allowed to update this product.");
        }


        private List<ImageDTO> GenerateValidImages(int count)
        {
            var images = new List<ImageDTO>();
            for (int i = 0; i < count; i++)
            {
                string base64String = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAIAAAC0tAIdAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAQSURBVChTYxgFo2AIAQYGAAKyAAEXtZa1AAAAAElFTkSuQmCC";
                byte[] imageData = Convert.FromBase64String(base64String);
                images.Add(new ImageDTO
                {
                    Data = imageData
                });
            }
            return images;
        }

        private List<ImageDTO> GenerateInvalidImages(int count)
        {
            var images = new List<ImageDTO>();
            for (int i = 0; i < count; i++)
            {
                images.Add(new ImageDTO
                {
                    Data = new byte[] { } // Example invalid image data (empty)
                });
            }
            return images;
        }
    }
}
