using BT.BrightMarket.Application.CQRS.Users;
using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Users;
using FluentValidation.TestHelper;
using Moq;

namespace BT.BrightMarket.UnitTests.Users
{
    [TestClass]
    public class UserValidatorsTests
    {
        private Mock<IUnitofWork> uowMock;
        private Mock<IUsersRepository> userRepoMock;
        private Mock<IRegionsRepository> regionRepoMock;

        [TestInitialize]
        public void Setup()
        {
            uowMock = new Mock<IUnitofWork>();
            userRepoMock = new Mock<IUsersRepository>();
            regionRepoMock = new Mock<IRegionsRepository>();

            uowMock.Setup((u) => u.UsersRepository).Returns(userRepoMock.Object);
            uowMock.Setup((u) => u.RegionsRepository).Returns(regionRepoMock.Object);

        }

        [TestMethod]
        public async Task AddUserWithNonEmptyEmailPositiveTest()
        {
            // Arrange
            var unit = new AddUserCommand
            {
                Email = "test@example.com",
                Name = "Test User",
                Role = Role.User,
                RegionId = 1
            };

            uowMock.Setup(u => u.RegionsRepository.Exists(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await new AddUserCommandValidator(uowMock.Object)
                .TestValidateAsync(unit);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]
        public async Task AddUserWithEmptyEmailNegativeTest()
        {

            // Arrange
            var unit = new AddUserCommand
            {
                Email = "",
                Name = "Test User",
                Role = Role.User,
                RegionId = 1
            };

            uowMock.Setup(u => u.RegionsRepository.Exists(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await new AddUserCommandValidator(uowMock.Object)
                .TestValidateAsync(unit);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.Email)
                .WithErrorMessage("Email cannot be empty.");
        }

        [TestMethod]
        public async Task AddUserWithSameEmailNegativeTest()
        {
            // Arrange
            var duplicateEmail = "test@example.com";
            var userWithDuplicateEmail = new AddUserCommand
            {
                Email = duplicateEmail,
                Name = "Test User",
                Role = Role.User,
                RegionId = 1
            };

            userRepoMock.Setup(u => u.GetByEmail(duplicateEmail))
                       .ReturnsAsync(new User());

            var validator = new AddUserCommandValidator(uowMock.Object);

            // Act
            var result = await validator.TestValidateAsync(userWithDuplicateEmail);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.Email)
                  .WithErrorMessage("The email already exists.");
        }

        [TestMethod]
        public async Task AddUserWithEmptyNameNegativeTest()
        {
            // Arrange
            var userWithEmptyName = new AddUserCommand
            {
                Email = "test@example.com",
                Name = "",
                Role = Role.User,
                RegionId = 1
            };

            var validator = new AddUserCommandValidator(uowMock.Object);

            // Act
            var result = await validator.TestValidateAsync(userWithEmptyName);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.Name)
                  .WithErrorMessage("Name cannot be empty.");
        }

        [TestMethod]
        public async Task AddUserWithValidRolePositiveTest()
        {
            // Arrange
            var userWithValidRole = new AddUserCommand
            {
                Email = "test@example.com",
                Name = "Test User",
                Role = Role.User, // Valid role
                RegionId = 1
            };

            var validator = new AddUserCommandValidator(uowMock.Object);

            // Act
            var result = await validator.TestValidateAsync(userWithValidRole);

            // Assert
            result.ShouldNotHaveValidationErrorFor(u => u.Role);
        }

        [TestMethod]
        public async Task AddUserWithInvalidRoleNegativeTest()
        {
            // Arrange
            var userWithInvalidRole = new AddUserCommand
            {
                Email = "test@example.com",
                Name = "Test User",
                Role = (Role) int.MinValue, // Invalid role
                RegionId = 1
            };

            var validator = new AddUserCommandValidator(uowMock.Object);

            // Act
            var result = await validator.TestValidateAsync(userWithInvalidRole);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.Role)
                  .WithErrorMessage("Role is not valid.");
        }

        [TestMethod]
        public async Task AddUserWithValidRegionIdPositiveTest()
        {
            // Arrange
            var userWithValidRegionId = new AddUserCommand
            {
                Email = "test@example.com",
                Name = "Test User",
                Role = Role.User,
                RegionId = 1 // Valid RegionId
            };

            uowMock.Setup(u => u.RegionsRepository.Exists(It.IsAny<int>())).ReturnsAsync(true);

            var validator = new AddUserCommandValidator(uowMock.Object);

            // Act
            var result = await validator.TestValidateAsync(userWithValidRegionId);

            // Assert
            result.ShouldNotHaveValidationErrorFor(u => u.RegionId);
        }

        [TestMethod]
        public async Task AddUserWithInvalidRegionIdNegativeTest()
        {
            // Arrange
            var userWithInvalidRegionId = new AddUserCommand
            {
                Email = "test@example.com",
                Name = "Test User",
                Role = Role.User,
                RegionId = int.MinValue
            };

            uowMock.Setup(u => u.RegionsRepository.Exists(It.IsAny<int>())).ReturnsAsync(false);

            var validator = new AddUserCommandValidator(uowMock.Object);

            // Act
            var result = await validator.TestValidateAsync(userWithInvalidRegionId);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.RegionId)
                  .WithErrorMessage("The specified region ID does not exist.");
        }

    }
}
