using BaseApp.API.Controllers;
using BaseApp.Data.User.Dtos;
using BaseApp.ServiceProvider.Interfaces;
using BaseApp.Shared.Dtos;
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
        public async Task GetUsersAsync_ReturnsOkResult_WithPaginatedUsers()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;

            var users = new List<UserDto>
            {
                new UserDto { Id = "1", UserName = "User1", FirstName = "John", LastName = "Doe", Email = "johndoe@company.com" },
                new UserDto { Id = "2", UserName = "User2", FirstName = "Jane", LastName = "Smith", Email = "janesmith@company.com" }
            };

            var paginatedResult = new PaginatedResult<UserDto>
            {
                Items = users,
                TotalCount = 10,
                Page = page,
                PageSize = pageSize
            };

            _mockUserService.GetUsersAsync(page, pageSize).Returns(Task.FromResult(paginatedResult));

            // Act
            var result = await _controller.GetUsersAsync(page, pageSize);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResult = Assert.IsType<PaginatedResult<UserDto>>(okResult.Value);

            // Validate pagination metadata
            Assert.Equal(page, returnedResult.Page);
            Assert.Equal(pageSize, returnedResult.PageSize);
            Assert.Equal(10, returnedResult.TotalCount);

            // Validate returned user data
            Assert.Equal(users.Count, returnedResult.Items.Count());
            Assert.Equal(users[0].Id, returnedResult.Items.First().Id);
            Assert.Equal(users[1].Id, returnedResult.Items.Last().Id);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsOkResult_WhenUserIsFound()
        {
            // Arrange
            var userId = new Guid().ToString();
            var user = new UserDto { Id = userId, UserName = "User1", FirstName = "John", LastName = "Doe", Email = "johndoe@company.com" };

            _mockUserService.GetUserAsync(userId).Returns(user);

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
            var invalidUserId = "-1";

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

            _mockUserService.CreateUserAsync(userRequest).Returns(true);

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
                FirstName = "InvalidFirstName",
                LastName = "InvalidLastName",
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
            var userId = new Guid().ToString();
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
            var userId = new Guid().ToString();

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
            var invalidUserId = "-1";;

            // Act
            var result = await _controller.DeleteUserAsync(invalidUserId);

            // Assert
            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }
    }
}
