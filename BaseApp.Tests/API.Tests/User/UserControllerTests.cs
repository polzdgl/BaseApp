using BaseApp.Data.User.Dtos;
using BaseApp.Data.User.Models;
using BaseApp.Server.Controllers;
using BaseApp.ServiceProvider.Interfaces.User;
using BaseApp.Shared.Dtos;
using BaseApp.Shared.Validation;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Text.Json;

namespace BaseApp.Tests.API.Tests.User
{
    public class UserControllerTests
    {
        private readonly IUserService _mockUserService;
        private readonly ILogger<UserController> _mockLogger;
        private readonly InputValidation _mockInputValidation;
        private readonly UserController _controller;
        private readonly IValidator<UserProfileDto> _mockUserProfileValidator;

        public UserControllerTests()
        {
            _mockUserService = Substitute.For<IUserService>();
            _mockLogger = Substitute.For<ILogger<UserController>>();
            _mockInputValidation = Substitute.For<InputValidation>();
            _mockUserProfileValidator = Substitute.For<IValidator<UserProfileDto>>();

            // Create substitutes for UserManager dependencies
            var userStore = Substitute.For<IUserStore<ApplicationUser>>();
            var options = Substitute.For<IOptions<IdentityOptions>>();
            var passwordHasher = Substitute.For<IPasswordHasher<ApplicationUser>>();
            var userValidators = Substitute.For<IEnumerable<IUserValidator<ApplicationUser>>>();
            var passwordValidators = Substitute.For<IEnumerable<IPasswordValidator<ApplicationUser>>>();
            var keyNormalizer = Substitute.For<ILookupNormalizer>();
            var errors = Substitute.For<IdentityErrorDescriber>();
            var services = Substitute.For<IServiceProvider>();
            var logger = Substitute.For<ILogger<UserManager<ApplicationUser>>>();

            // Initialize the controller with the mocked dependencies
            _controller = new UserController(_mockLogger, _mockUserService, _mockInputValidation, _mockUserProfileValidator);
        }


        [Fact]
        public async Task GetUsersAsync_ReturnsOkResult_WithPaginatedUsers()
        {
            // Setup mock data
            int page = 1;
            int pageSize = 2;

            var users = new List<UserDto>
            {
                new UserDto { Id = "1", FirstName = "John", LastName = "Doe", Email = "johndoe@company.com" },
                new UserDto { Id = "2", FirstName = "Jane", LastName = "Smith", Email = "janesmith@company.com" }
            };

            var paginatedResult = new PaginatedResult<UserDto>
            {
                Items = users,
                TotalCount = 10,
                Page = page,
                PageSize = pageSize
            };

            _mockUserService.GetUsersAsync(page, pageSize).Returns(Task.FromResult(paginatedResult));

            // Call the controller method
            var result = await _controller.GetUsersAsync(page, pageSize);

            // Confirm the result is an OkObjectResult
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResult = Assert.IsType<PaginatedResult<UserDto>>(okResult.Value);

            // Validate paginated data
            Assert.Equal(page, returnedResult.Page);
            Assert.Equal(pageSize, returnedResult.PageSize);
            Assert.Equal(10, returnedResult.TotalCount);

            // Validate returned user data
            Assert.Equal(users.Count, returnedResult.Items.Count());
            Assert.Equal(users[0].Id, returnedResult.Items.First().Id);
            Assert.Equal(users[1].Id, returnedResult.Items.Last().Id);
        }


        [Fact]
        public async Task GetUsersAsync_ReturnsOkResult_WithNoUsersData()
        {
            // Setup mock data
            int page = 1;
            int pageSize = 5;

            // No data
            var users = new List<UserDto>();

            var paginatedResult = new PaginatedResult<UserDto>
            {
                Items = users,
                TotalCount = 0,
                Page = page,
                PageSize = pageSize
            };

            _mockUserService.GetUsersAsync(page, pageSize).Returns(Task.FromResult(paginatedResult));

            // Call the controller method
            var result = await _controller.GetUsersAsync(page, pageSize);

            // Confirm the result is an OkObjectResult
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResult = Assert.IsType<PaginatedResult<UserDto>>(okResult.Value);

            // Validate paginated data
            Assert.Equal(page, returnedResult.Page);
            Assert.Equal(pageSize, returnedResult.PageSize);
            Assert.Equal(0, returnedResult.TotalCount);

            // Validate returned user data
            Assert.Equal(users.Count, returnedResult.Items.Count());
            Assert.Empty(returnedResult.Items);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsOkResult_WhenUserIsFound()
        {
            var userId = new Guid().ToString();
            var user = new UserDto { Id = userId, FirstName = "John", LastName = "Doe", Email = "johndoe@company.com" };

            _mockUserService.GetUserAsync(userId).Returns(user);

            var result = await _controller.GetUserByIdAsync(userId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<UserDto>(okResult.Value);

            Assert.Equal(userId, returnedUser.Id);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsBadRequest_WhenIdIsInvalid()
        {
            var invalidUserId = "-1";

            var result = await _controller.GetUserByIdAsync(invalidUserId);

            var badRequestResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsBadRequest_WhenIdIsNotFound()
        {
            var invalidUserId = new Guid().ToString();

            var result = await _controller.GetUserByIdAsync(invalidUserId);

            var notFoundResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task AddUserAsync_ReturnsCreatedResult_WhenUserIsValid()
        {
            var userRequest = new UserProfileDto
            {
                Email = "john.doe@example.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var validationFailures = new List<ValidationFailure>();
            _mockUserProfileValidator.Validate(userRequest).Returns(new ValidationResult(validationFailures));

            _mockUserService.CreateUserAsync(userRequest).Returns(true);

            var result = await _controller.CreateUserAsync(userRequest);

            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task AddUserAsync_ReturnsBadRequest_WhenEmailIsInvalid()
        {
            var userRequest = new UserProfileDto
            {
                FirstName = "InvalidFirstName",
                LastName = "InvalidLastName",
                Email = "invalidemail"
            };

            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Email", "Email is invalid"),
            };

            _mockUserProfileValidator.Validate(userRequest).Returns(new ValidationResult(validationFailures));

            var result = await _controller.CreateUserAsync(userRequest);

            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);

            // Ensure the response contains a specific error about the invalid email
            var responseContent = badRequestResult.Value;
            Assert.NotNull(responseContent);

            // Deserialize the response to inspect its details
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(JsonSerializer.Serialize(responseContent));

            Assert.NotNull(problemDetails);
            Assert.Contains("Email is invalid", problemDetails?.Detail ?? string.Empty, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task AddUserAsync_ReturnsBadRequest_WhenPhoneIsInvalid()
        {
            var userRequest = new UserProfileDto
            {
                FirstName = "InvalidFirstName",
                LastName = "InvalidLastName",
                Email = "john.doe@example.com",
                PhoneNumber = "invalidphone"
            };

            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Phone", "Phone number is invalid"),
            };

            _mockUserProfileValidator.Validate(userRequest).Returns(new ValidationResult(validationFailures));


            var result = await _controller.CreateUserAsync(userRequest);

            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);

            // Ensure the response contains a specific error about the invalid email
            var responseContent = badRequestResult.Value;
            Assert.NotNull(responseContent);

            // Deserialize the response to inspect its details
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(JsonSerializer.Serialize(responseContent));

            Assert.NotNull(problemDetails);
            Assert.Contains("Phone number is invalid", problemDetails?.Detail ?? string.Empty, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task AddUserAsync_ReturnsBadRequest_WhenDOBIsInvalid()
        {
            var userRequest = new UserProfileDto
            {
                FirstName = "InvalidFirstName",
                LastName = "InvalidLastName",
                Email = "john.doe@example.com",
                DateOfBirth = DateTime.Now.AddDays(1)
            };

            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("DOB", "DOB is invalid"),
            };

            _mockUserProfileValidator.Validate(userRequest).Returns(new ValidationResult(validationFailures));

            var result = await _controller.CreateUserAsync(userRequest);

            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);

            // Ensure the response contains a specific error about the invalid email
            var responseContent = badRequestResult.Value;
            Assert.NotNull(responseContent);

            // Deserialize the response to inspect its details
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(JsonSerializer.Serialize(responseContent));

            Assert.NotNull(problemDetails);
            Assert.Contains("DOB is invalid", problemDetails?.Detail ?? string.Empty, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task AddUserAsync_ReturnsBadRequest_WhenUserNameIsTaken()
        {
            var userId = new Guid().ToString();

            var user = new UserDto { Id = userId, FirstName = "John", LastName = "Doe", Email = "johndoe@company.com" };

            _mockUserService.GetUserAsync(userId).Returns(user);

            var userRequest = new UserProfileDto
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "johndoe@company.com",
            };

            var validationFailures = new List<ValidationFailure>();

            _mockUserProfileValidator.Validate(userRequest).Returns(new ValidationResult(validationFailures));

            var result = await _controller.CreateUserAsync(userRequest);

            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);

            // Ensure the response contains a specific error about the invalid email
            var responseContent = badRequestResult.Value;
            Assert.NotNull(responseContent);

            // Deserialize the response to inspect its details
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(JsonSerializer.Serialize(responseContent));

            Assert.NotNull(problemDetails);
            Assert.Contains("Failed to Create Username", problemDetails?.Detail ?? string.Empty, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task UpdateUserAsync_ReturnsOkResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            var userId = new Guid().ToString();
            var userProfileDto = new UserProfileDto
            {
                Email = "updated.john@example.com",
                FirstName = "John",
                LastName = "Doe"
            };

            _mockUserService.UpdateUserAsync(userId, userProfileDto).Returns(true);

            // Act
            var result = await _controller.EditUserAsync(userId, userProfileDto);

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
            var invalidUserId = "-1"; ;

            // Act
            var result = await _controller.DeleteUserAsync(invalidUserId);

            // Assert
            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }
    }
}
