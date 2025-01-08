using BaseApp.Data.User.Dtos;
using BaseApp.Data.User.Models;
using BaseApp.Server.Controllers;
using BaseApp.Shared.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Text.Json;

namespace BaseApp.Tests.API.Tests.User
{
    public class AuthControllerTests
    {
        private readonly ILogger<AuthController> _mockLogger;
        private readonly InputValidation _mockInputValidation;
        private readonly AuthController _controller;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthControllerTests()
        {
            _mockLogger = Substitute.For<ILogger<AuthController>>();
            _mockInputValidation = Substitute.For<InputValidation>();

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

            // Initialize UserManager with substitutes
            _userManager = new UserManager<ApplicationUser>(
                userStore,
                options,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errors,
                services,
                logger);

            // Initialize the controller with the mocked dependencies
            _controller = new AuthController(_mockLogger, _mockInputValidation, _userManager);
        }

        [Fact]
        public async Task AddUserAsync_ReturnsBadRequest_WhenEmailIsInvalid()
        {
            var userRequest = new UserRegisterDto
            {
                FirstName = "InvalidFirstName",
                LastName = "InvalidLastName",
                Email = "invalidemail",
                Password = "a",
                ConfirmPassword = "b"
            };

            var result = await _controller.Register(userRequest);

            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);

            // Ensure the response contains a specific error about the invalid email
            var responseContent = badRequestResult.Value;
            Assert.NotNull(responseContent);

            // Deserialize the response to inspect its details
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(JsonSerializer.Serialize(responseContent));

            Assert.NotNull(problemDetails);
            Assert.Contains("invalid email", problemDetails?.Detail ?? string.Empty, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task AddUserAsync_ReturnsBadRequest_WhenPhoneIsInvalid()
        {
            var userRequest = new UserRegisterDto
            {
                FirstName = "InvalidFirstName",
                LastName = "InvalidLastName",
                Email = "john.doe@example.com",
                Password = "Password123",
                ConfirmPassword = "Password123",
                PhoneNumber = "invalidphone"
            };

            var result = await _controller.Register(userRequest);

            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);

            // Ensure the response contains a specific error about the invalid email
            var responseContent = badRequestResult.Value;
            Assert.NotNull(responseContent);

            // Deserialize the response to inspect its details
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(JsonSerializer.Serialize(responseContent));

            Assert.NotNull(problemDetails);
            Assert.Contains("invalid phone", problemDetails?.Detail ?? string.Empty, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task AddUserAsync_ReturnsBadRequest_WhenDOBIsInvalid()
        {
            var userRequest = new UserRegisterDto
            {
                FirstName = "InvalidFirstName",
                LastName = "InvalidLastName",
                Email = "john.doe@example.com",
                Password = "Password123",
                ConfirmPassword = "Password123",
                DateOfBirth = DateTime.Now.AddDays(1)
            };

            var result = await _controller.Register(userRequest);

            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);

            // Ensure the response contains a specific error about the invalid email
            var responseContent = badRequestResult.Value;
            Assert.NotNull(responseContent);

            // Deserialize the response to inspect its details
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(JsonSerializer.Serialize(responseContent));

            Assert.NotNull(problemDetails);
            Assert.Contains("Invalid Date of Birth", problemDetails?.Detail ?? string.Empty, StringComparison.OrdinalIgnoreCase);
        }

    }
}
