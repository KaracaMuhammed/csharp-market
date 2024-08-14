using BT.BrightMarket.Application.CQRS.Categories;
using BT.BrightMarket.Application.CQRS.Conversations;
using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Products;
using BT.BrightMarket.Domain.Models.Users;
using FluentValidation.TestHelper;
using Moq;

namespace BT.BrightMarket.UnitTests.Conversaties
{
    [TestClass]
    public class ConversationValidatorTests
    {
        private readonly Mock<IUnitofWork> unitOfWorkMock;
        private readonly Mock<IUsersRepository> usersRepositoryMock;
        private readonly Mock<IConversationsRepository> conversationsRepositoryMock;
        private readonly Mock<IProductsRepository> productsRepositoryMock;

        public ConversationValidatorTests()
        {
            unitOfWorkMock = new Mock<IUnitofWork>();
            usersRepositoryMock = new Mock<IUsersRepository>();
            conversationsRepositoryMock = new Mock<IConversationsRepository>();
            productsRepositoryMock = new Mock<IProductsRepository>();
            unitOfWorkMock.Setup(u => u.UsersRepository).Returns(usersRepositoryMock.Object);
            unitOfWorkMock.Setup(u => u.ConversationsRepository).Returns(conversationsRepositoryMock.Object);
            unitOfWorkMock.Setup(u => u.ProductsRepository).Returns(productsRepositoryMock.Object);
        }


        [TestMethod]
        public async Task AddConversationForOwnProductNegativeTest()
        {
            // Arrange
            var command = new AddConversationCommand
            {
                ProductId = 1,
                AuthenticatedUserId = 2
            };

            // Setup mock behavior for ProductExistsAsync to return true for the provided ID
            productsRepositoryMock.Setup(repo => repo.ProductExistsAsync(command.ProductId)).ReturnsAsync(true);

            // Setup mock behavior for Exists to return true indicating conversation exists
            conversationsRepositoryMock.Setup(repo => repo.Exists(command.ProductId, command.AuthenticatedUserId)).ReturnsAsync(true);

            // Setup mock behavior for GetById to return the owner user
            usersRepositoryMock.Setup(repo => repo.GetById(2)).ReturnsAsync(new User { Id = 2, Name = "Owner User" });

            // Setup mock behavior for GetById to return the product with the owner user ID
            productsRepositoryMock.Setup(repo => repo.GetById(command.ProductId)).ReturnsAsync(new Product { Id = 1, UserId = 2 });

            var validator = new AddConversationCommandValidator(unitOfWorkMock.Object);

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c)
                  .WithErrorMessage("You cannot start a conversation for your own product.");
        }

        [TestMethod]
        public async Task AddConversationDuplicateNegativeTest()
        {
            // Arrange
            var command = new AddConversationCommand
            {
                ProductId = 1,
                AuthenticatedUserId = 2
            };

            // Mock the behavior for ProductExistsAsync to return true
            productsRepositoryMock.Setup(repo => repo.ProductExistsAsync(command.ProductId)).ReturnsAsync(true);

            // Mock the behavior for GetById to return the owner user
            usersRepositoryMock.Setup(repo => repo.GetById(2)).ReturnsAsync(new User { Id = 2, Name = "Owner User" });

            // Mock the behavior for GetById to return the product with the owner user ID
            productsRepositoryMock.Setup(repo => repo.GetById(command.ProductId)).ReturnsAsync(new Product { Id = 1, UserId = 2 });

            // Mock the behavior for Exists to return true, indicating that the conversation already exists
            conversationsRepositoryMock.Setup(r => r.Exists(1, 2)).ReturnsAsync(true);

            var validator = new AddConversationCommandValidator(unitOfWorkMock.Object);

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c)
                .WithErrorMessage("A conversation for this product and buyer already exists.");
        }

        [TestMethod]
        public async Task AddConversationWithNonExistingProductNegativeTest()
        {
            // Arrange
            var command = new AddConversationCommand
            {
                ProductId = 1,
                AuthenticatedUserId = 1
            };

            // Mock the behavior for ProductExistsAsync to return false, indicating that the product does not exist
            productsRepositoryMock.Setup(repo => repo.ProductExistsAsync(command.ProductId)).ReturnsAsync(false);

            // Setup mock behavior for GetById to return the owner user
            usersRepositoryMock.Setup(repo => repo.GetById(1)).ReturnsAsync(new User { Id = 1, Name = "Owner User" });

            // Setup mock behavior for GetById to return the product with the owner user ID
            productsRepositoryMock.Setup(repo => repo.GetById(command.ProductId)).ReturnsAsync(new Product { Id = 1, UserId = 1 });

            var validator = new AddConversationCommandValidator(unitOfWorkMock.Object);

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.ProductId)
                .WithErrorMessage("Product does not exist.");
        }

        [TestMethod]
        public async Task AddConversationPositiveTest()
        {
            // Arrange
            var command = new AddConversationCommand
            {
                ProductId = 1,
                AuthenticatedUserId = 2 // Assuming a different user than the product owner
            };

            // Mock the behavior for ProductExistsAsync to return true, indicating that the product exists
            productsRepositoryMock.Setup(repo => repo.ProductExistsAsync(command.ProductId)).ReturnsAsync(true);

            // Setup mock behavior for GetById to return the product with a different owner user ID
            productsRepositoryMock.Setup(repo => repo.GetById(command.ProductId)).ReturnsAsync(new Product { Id = 1, UserId = 1 });

            // Mock the behavior for Exists to return false, indicating that a conversation does not already exist for this product and user
            conversationsRepositoryMock.Setup(repo => repo.Exists(command.ProductId, command.AuthenticatedUserId)).ReturnsAsync(false);

            var validator = new AddConversationCommandValidator(unitOfWorkMock.Object);

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
        }



    }
}
