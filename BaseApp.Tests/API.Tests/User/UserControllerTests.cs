using BaseApp.API.Controllers;
using BaseApp.Data.User.Dtos;
using BaseApp.ServiceProvider.Interfaces;
using BaseApp.Shared.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.ComponentModel.DataAnnotations;

namespace BaseApp.Tests.API.Tests.User
{
    public class UserControllerTests
    {
        private readonly IUserService _mockUserService;
        private readonly ILogger<UserController> _mockLogger;
        private readonly InputValidation _mockInputValidation;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockUserService = Substitute.For<IUserService>();
            _mockLogger = Substitute.For<ILogger<UserController>>();
            _mockInputValidation = Substitute.For<InputValidation>();
            _controller = new UserController(_mockLogger, _mockUserService, _mockInputValidation);
        }

        [Fact]
        public async Task GetUsersAsync_ReturnsOkResult_WithListOfUsers()
        {
            // Arrange
            var users = new List<UserDto>
            {
                new UserDto { Id = 1, FirstName = "John", LastName = "Doe" },
                new UserDto { Id = 2, FirstName = "Jane", LastName = "Smith" }
            };

            _mockUserService.GetAllUserAsync().Returns(users);

            // Act
            var result = await _controller.GetUsersAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsAssignableFrom<IEnumerable<UserDto>>(okResult.Value);
            Assert.Equal(users.Count, ((List<UserDto>)returnedUsers).Count);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsOkResult_WhenUserIsFound()
        {
            // Arrange
            var userId = 1;
            var user = new UserDto { Id = userId, FirstName = "John", LastName = "Doe" };

            _mockUserService.GetUserByIdAsync(userId).Returns(user);

            // Act
            var result = await _controller.GetUserByIdAsync(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<UserDto>(okResult.Value);
            Assert.Equal(userId, returnedUser.Id);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsBadRequest_WhenIdIsInvalid()
        {
            // Arrange
            var invalidUserId = -1;

            // Act
            var result = await _controller.GetUserByIdAsync(invalidUserId);

            // Assert
            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task AddUserAsync_ReturnsCreatedResult_WhenUserIsValid()
        {
            // Arrange
            var userRequest = new UserRequestDto
            {
                UserName = "john.doe",
                Email = "john.doe@example.com",
                FirstName = "John",
                LastName = "Doe"
            };

            _mockUserService.AddUserAsync(userRequest).Returns(true);

            // Act
            var result = await _controller.AddUserAsync(userRequest);

            // Assert
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task AddUserAsync_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var userRequest = new UserRequestDto
            {
                UserName = "invalidUser",
                Email = "invalidemail"
            };

            // Act
            var result = await _controller.AddUserAsync(userRequest);

            // Assert
            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task UpdateUserAsync_ReturnsOkResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            var userId = 1;
            var userRequest = new UserRequestDto
            {
                UserName = "updated.john",
                Email = "updated.john@example.com",
                FirstName = "John",
                LastName = "Doe"
            };

            _mockUserService.UpdateUserAsync(userId, userRequest).Returns(true);

            // Act
            var result = await _controller.UpdateUserAsync(userId, userRequest);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteUserAsync_ReturnsOkResult_WhenDeleteIsSuccessful()
        {
            // Arrange
            var userId = 1;

            _mockUserService.DeleteUserAsync(userId).Returns(true);

            // Act
            var result = await _controller.DeleteUserAsync(userId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteUserAsync_ReturnsBadRequest_WhenIdIsInvalid()
        {
            // Arrange
            var invalidUserId = -1;;

            // Act
            var result = await _controller.DeleteUserAsync(invalidUserId);

            // Assert
            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }
    }
}
