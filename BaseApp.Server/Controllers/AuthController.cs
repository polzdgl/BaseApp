using BaseApp.Data.User.Dtos;
using BaseApp.Data.User.Models;
using BaseApp.Shared.Dtos;
using BaseApp.Shared.Validation;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly InputValidation _inputValidation;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

        public AuthController(ILogger<AuthController> logger, InputValidation inputValidation,
            Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _inputValidation = inputValidation;
            _userManager = userManager;
        }

        [HttpPost("register", Name = "RegisterUser")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            try
            {
                // Validate input data using InputValidation methods
                if (!_inputValidation.ValidateModel(userRegisterDto, out var validationResults))
                {
                    var validationErrors = _inputValidation.GetValidationErrors(validationResults);
                    _logger.LogWarning("UserProfile model is invalid. Error: {ValidationErrors}", validationErrors);
                    return Problem(
                        detail: $"Invalid model: {userRegisterDto}. Errors: {validationErrors}",
                        statusCode: StatusCodes.Status400BadRequest);
                }

                if (!_inputValidation.ValidateEmailAddress(userRegisterDto.Email, out var emailErrorMessage))
                {
                    _logger.LogWarning("Invalid Email Address: {ErrorMessage}", emailErrorMessage);
                    return Problem(
                        detail: $"Invalid Email: {userRegisterDto.Email}. Errors: {emailErrorMessage}",
                        statusCode: StatusCodes.Status400BadRequest);
                }

                // Validate the phone number only if it's not empty
                if (!string.IsNullOrWhiteSpace(userRegisterDto.PhoneNumber) &&
                    !_inputValidation.ValidatePhoneNumber(userRegisterDto.PhoneNumber, out var phoneValidationError))
                {
                    _logger.LogWarning("Invalid Phone: {ErrorMessage}", phoneValidationError);
                    return Problem(
                        detail: $"Invalid Phone for UserName: {userRegisterDto.UserName}. Errors: {phoneValidationError}",
                        statusCode: StatusCodes.Status400BadRequest);
                }

                // Validate the date of birth only if it's not null
                if (userRegisterDto.DateOfBirth != null &&
                    !_inputValidation.ValidateDateOfBirth(userRegisterDto.DateOfBirth, out var dateValidationError))
                {
                    _logger.LogWarning("Invalid Date of Birth: {ErrorMessage}", dateValidationError);
                    return Problem(
                        detail: $"Invalid Date of Birth for UserName: {userRegisterDto.UserName}. Errors: {dateValidationError}",
                        statusCode: StatusCodes.Status400BadRequest);
                }

                // Map custom properties to ApplicationUser
                var user = new ApplicationUser
                {
                    UserName = userRegisterDto.UserName,
                    Email = userRegisterDto.Email,
                    FirstName = userRegisterDto.FirstName,
                    LastName = userRegisterDto.LastName,
                    DateOfBirth = userRegisterDto.DateOfBirth,
                    PhoneNumber = userRegisterDto.PhoneNumber,
                    IsActive = userRegisterDto.IsActive = true //Set IsActive to true by default
                };

                // Create the user with the provided password
                var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

                if (!result.Succeeded)
                {
                    var errorDescriptions = result.Errors.Select(e => e.Description).ToList();
                    _logger.LogWarning("User creation failed. Errors: {ErrorDescriptions}", errorDescriptions);
                    return Problem(
                        detail: $"User registration failed. Errors: {string.Join(", ", errorDescriptions)}",
                        statusCode: StatusCodes.Status400BadRequest);
                }

                return Ok(new { Message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during user registration.");
                return Problem(
                    detail: "An unexpected error occurred while processing your request.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("connectedUser", Name = "GetConnectedUser")]
        [ProducesResponseType(typeof(ConnectedUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> GetConnectedUser()
        {
            var user = await _userManager.FindByIdAsync(this.User.Identity.GetUserId());
            if (user == null)
            {
                return Problem(detail: "User not authorized!", statusCode: StatusCodes.Status400BadRequest);
            }

            await _userManager.GetClaimsAsync(user);

            return Ok(new ConnectedUser
            {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Isactive = user.IsActive
            });
        }
    }
}
