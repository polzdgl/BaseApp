using BaseApp.Data.User.Dtos;
using BaseApp.Data.User.Models;
using BaseApp.Server.Controllers;
using BaseApp.Shared.Dtos;
using BaseApp.Shared.Validations;
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
    public class AuthControllerTests
    {
        private readonly ILogger<AuthController> _mockLogger;
        private readonly InputValidation _mockInputValidation;
        private readonly AuthController _controller;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IValidator<UserRegisterDto> _mockUserRegisterValidator;

        public AuthControllerTests()
        {
            _mockLogger = Substitute.For<ILogger<AuthController>>();
            _mockInputValidation = Substitute.For<InputValidation>();
            _mockUserRegisterValidator = Substitute.For<IValidator<UserRegisterDto>>();

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
            _controller = new AuthController(_mockLogger, _userManager, _mockUserRegisterValidator);
        }


        [Fact]
        public async Task Register_ShouldReturnValidationErrors_WhenValidationFails()
        {
            // Arrange
            var userDto = new UserRegisterDto
            {
                Email = "",
                Password = "123",
                ConfirmPassword = "456"
            };

            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Email", "Email is required"),
                new ValidationFailure("Password", "Password is too short"),
                new ValidationFailure("ConfirmPassword", "Passwords must match")
            };

            _mockUserRegisterValidator.Validate(userDto).Returns(new ValidationResult(validationFailures));

            // Act
            var result = await _controller.Register(userDto);

            // Assert
            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);

            var problemDetails = Assert.IsType<ProblemDetails>(badRequestResult.Value);
            Assert.Contains("Email: Email is required", problemDetails.Detail);
            Assert.Contains("Password: Password is too short", problemDetails.Detail);
            Assert.Contains("ConfirmPassword: Passwords must match", problemDetails.Detail);
        }

        [Fact]
        public async Task AddUserAsync_ReturnsBadRequest_WhenEmailIsInvalid()
        {
            var userRequest = new UserRegisterDto
            {
                FirstName = "InvalidFirstName",
                LastName = "InvalidLastName",
                Email = "invalidemail",
                Password = "Password12345",
                ConfirmPassword = "Password12345",
            };

            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Email", "Email is invalid"),
            };

            _mockUserRegisterValidator.Validate(userRequest).Returns(new ValidationResult(validationFailures));

            var result = await _controller.Register(userRequest);

            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);

            // Ensure the response contains a specific error about the invalid email
            var responseContent = badRequestResult.Value;
            Assert.NotNull(responseContent);

            var problemDetails = Assert.IsType<ProblemDetails>(badRequestResult.Value);

            Assert.NotNull(problemDetails);
            Assert.Contains("Email is invalid", problemDetails.Detail);
        }

        [Fact]
        public async Task AddUserAsync_ReturnsBadRequest_WhenPhoneIsInvalid()
        {
            var userRequest = new UserRegisterDto
            {
                FirstName = "InvalidFirstName",
                LastName = "InvalidLastName",
                Email = "john.doe@example.com",
                Password = "Password12345",
                ConfirmPassword = "Password12345",
                PhoneNumber = "invalidphone"
            };

            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Phone", "Phone number is invalid"),
            };

            _mockUserRegisterValidator.Validate(userRequest).Returns(new ValidationResult(validationFailures));

            var result = await _controller.Register(userRequest);

            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);

            // Ensure the response contains a specific error about the invalid email
            var responseContent = badRequestResult.Value;
            Assert.NotNull(responseContent);

            var problemDetails = Assert.IsType<ProblemDetails>(badRequestResult.Value);

            Assert.NotNull(problemDetails);
            Assert.Contains("Phone number is invalid", problemDetails.Detail);
        }

        [Fact]
        public async Task AddUserAsync_ReturnsBadRequest_WhenDOBIsInvalid()
        {
            var userRequest = new UserRegisterDto
            {
                FirstName = "InvalidFirstName",
                LastName = "InvalidLastName",
                Email = "john.doe@example.com",
                Password = "Password12345",
                ConfirmPassword = "Password12345",
                DateOfBirth = DateTime.Now.AddDays(1)
            };

            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("DOB", "DOB is invalid"),
            };

            _mockUserRegisterValidator.Validate(userRequest).Returns(new ValidationResult(validationFailures));

            var result = await _controller.Register(userRequest);

            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);

            // Ensure the response contains a specific error about the invalid email
            var responseContent = badRequestResult.Value;
            Assert.NotNull(responseContent);

            // Deserialize the response to inspect its details
            var problemDetails = Assert.IsType<ProblemDetails>(badRequestResult.Value);

            Assert.NotNull(problemDetails);
            Assert.Contains("DOB is invalid", problemDetails.Detail);
        }
    }
}
