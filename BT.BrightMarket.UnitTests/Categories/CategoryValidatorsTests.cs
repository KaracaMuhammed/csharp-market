using BT.BrightMarket.Application.CQRS.Categories;
using BT.BrightMarket.Application.Interfaces;
using FluentValidation.TestHelper;
using Moq;

namespace BT.BrightMarket.UnitTests.Categories
{
    public class CategoryValidatorsTests
    {
        [TestClass]
        public class AddCategoryValidatorTests
        {

            private Mock<IUnitofWork> uowMock;
            private Mock<ICategoriesRepository> categoryRepoMock;

            [TestInitialize]
            public void Setup()
            {
                uowMock = new Mock<IUnitofWork>();
                categoryRepoMock = new Mock<ICategoriesRepository>();
                uowMock.Setup(u => u.CategoriesRepository).Returns(categoryRepoMock.Object);
            }

            [TestMethod]
            public async Task AddCategoryWithNonEmptyNamePositiveTest()
            {
                // Arrange
                var command = new AddCategoryCommand
                {
                    Name = "Test Category"
                };

                var validator = new AddCategoryCommandValidator(uowMock.Object);

                // Act
                var result = await validator.TestValidateAsync(command);

                // Assert
                result.ShouldNotHaveValidationErrorFor(c => c);
            }

            [TestMethod]
            public async Task AddCategoryWithEmptyNameNegativeTest()
            {
                // Arrange
                var command = new AddCategoryCommand
                {
                    Name = "" // Empty name
                };

                // Act
                var validator = new AddCategoryCommandValidator(uowMock.Object);
                var result = await validator.TestValidateAsync(command);

                // Assert
                result.ShouldHaveValidationErrorFor(c => c.Name)
                    .WithErrorMessage("The category name cannot be empty.");
            }

            [TestMethod]
            public async Task AddCategoryWithTooLongNameNegativeTest()
            {
                // Arrange
                var command = new AddCategoryCommand
                {
                    Name = "ThisCategoryNameIsTooLongAndExceedsTheMaximumLengthAllowed" // Name exceeds max length
                };

                var validator = new AddCategoryCommandValidator(uowMock.Object);

                // Act
                var result = await validator.TestValidateAsync(command);

                // Assert
                result.ShouldHaveValidationErrorFor(c => c.Name)
                    .WithErrorMessage("The category name cannot exceed 40 characters.");
            }
        }
    }
}
