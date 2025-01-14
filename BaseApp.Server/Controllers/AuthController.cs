using BaseApp.Data.User.Dtos;
using BaseApp.Data.User.Models;
using BaseApp.Shared.Dtos.Auth;
using FluentValidation;
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
        private readonly IValidator<UserRegisterDto> _userRegisterValidator;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

        public AuthController(ILogger<AuthController> logger, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
            IValidator<UserRegisterDto> userRegisterValidator)
        {
            _logger = logger;
            _userManager = userManager;
            _userRegisterValidator = userRegisterValidator;
        }

        [HttpPost("register", Name = "RegisterUser")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            try
            {
                // Validate the request
                var validationResult = _userRegisterValidator.Validate(userRegisterDto);

                if (!validationResult.IsValid) 
                {
                    // Format validation errors into a string summary
                    var errorDetails = string.Join("; ", validationResult.Errors
                        .Select(failure => $"{failure.PropertyName}: {failure.ErrorMessage}"));

                    return Problem(
                        title: "Validation failed",
                        detail: errorDetails,
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
                    detail: ex.Message,
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
