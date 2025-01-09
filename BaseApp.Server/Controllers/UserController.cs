using BaseApp.Data.User.Dtos;
using BaseApp.ServiceProvider.Interfaces.User;
using BaseApp.Shared.Dtos;
using BaseApp.Shared.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly InputValidation _inputValidation;
        private readonly IValidator<UserProfileDto> _userProfileValidator;


        public UserController(ILogger<UserController> logger, IUserService userService, InputValidation inputValidation,
            IValidator<UserProfileDto> userProfileValidator)
        {
            _logger = logger;
            _userService = userService;
            _inputValidation = inputValidation;
            _userProfileValidator = userProfileValidator;
        }

        [HttpGet("users", Name = "GetUsersAsync")]
        [ProducesResponseType(typeof(PaginatedResult<UserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsersAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                PaginatedResult<UserDto> users = await _userService.GetUsersAsync(page, pageSize);

                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while getting all Users list!");
                return Problem(detail: ex.Message.ToString(), statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}", Name = "GetUserByIdAsync")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserByIdAsync(string id)
        {
            try
            {
                if (!_inputValidation.ValidateId(id, out var guidValidationError))
                {
                    _logger.LogWarning("Invalid Id: {id}. Errors: {erorrMessage}", id, guidValidationError);
                    return Problem(detail: $"Invalid Id: {id}. Errors: {guidValidationError}", statusCode: StatusCodes.Status400BadRequest);
                }

                UserDto user = await _userService.GetUserAsync(id);

                return user is not null ? Ok(user) :
                    Problem(detail: $"User ID: {id} was not found!", statusCode: StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while getting UserId: {id}");
                return Problem(detail: ex.Message.ToString(), statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost(Name = "CreateUserAsync")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUserAsync(UserProfileDto userProfileDto)
        {
            try
            {
                // Validate the request
                var validationResult = _userProfileValidator.Validate(userProfileDto);

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

                bool isUserCreated = await _userService.CreateUserAsync(userProfileDto);

                return isUserCreated ? this.Created() :
                    Problem(detail: $"Failed to Create Username: {userProfileDto.UserName}!", statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while creating UserName: {userProfileDto.UserName}");
                return Problem(detail: ex.Message.ToString(), statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPut("{id}", Name = "EditUserAsync")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditUserAsync(string id, [FromBody] UserProfileDto userProfileDto)
        {
            try
            {
                // Validate the UserId
                if (!_inputValidation.ValidateId(id, out var numberValidationError))
                {
                    _logger.LogWarning("Invalid Id: {errorMessage}", numberValidationError);
                    return Problem(detail: $"Invalid Id: {id}. Errors: {numberValidationError}", statusCode: StatusCodes.Status400BadRequest);
                }

                // Validate model
                if (!_inputValidation.ValidateModel(userProfileDto, out var validationResults))
                {
                    var validationErrors = _inputValidation.GetValidationErrors(validationResults);
                    _logger.LogWarning("UserProfile model is invalid. Errors: {validationErrors}", validationErrors);
                    return Problem(detail: $"Invalid Model for Id: {id}. Errors: {validationErrors}", statusCode: StatusCodes.Status400BadRequest);
                }

                bool updatedUser = await _userService.UpdateUserAsync(id, userProfileDto);

                return updatedUser ? Ok() :
                 Problem(detail: $"Failed to Update UserId: {id}!", statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while updating UserId: {id}");
                return Problem(detail: ex.Message.ToString(), statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}", Name = "DeleteUserAsync")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            try
            {
                // Validate the UserAccountId
                if (!_inputValidation.ValidateId(id, out var numberValidationError))
                {
                    _logger.LogWarning("Invalid Id: {errorMessage}", numberValidationError);
                    return Problem(detail: $"Invalid Id: {id}. Errors: {numberValidationError}", statusCode: StatusCodes.Status400BadRequest);
                }

                bool deletedUser = await _userService.DeleteUserAsync(id);

                return deletedUser ? Ok() :
                  Problem(detail: $"Failed to Delete UserId: {id}!", statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while deleting a UserId: {id}");
                return Problem(detail: ex.Message.ToString(), statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
