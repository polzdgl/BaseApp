using BaseApp.Data.User.Dtos;
using BaseApp.Data.User.Models;
using BaseApp.ServiceProvider.Interfaces;
using BaseApp.Shared.Validation;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly InputValidation _inputValidation;

        public UserController(ILogger<UserController> logger, IUserService userService, InputValidation inputValidation)
        {
            _logger = logger;
            _userService = userService;
            _inputValidation = inputValidation;
        }

        [HttpGet("users", Name = "GetUsersAsync")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsersAsync()
        {
            try
            {
                IEnumerable<UserDto> users = await _userService.GetAllUserAsync();

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
                if (!_inputValidation.ValidateId(id, out var numberValidationError))
                {
                    _logger.LogWarning("Invalid Id: {id}. Errors: {erorrMessage}", id, numberValidationError);
                    return Problem(detail: $"Invalid Id: {id}. Errors: {numberValidationError}", statusCode: StatusCodes.Status400BadRequest);
                }

                UserDto user = await _userService.GetUserByIdAsync(id);

                return user is not null ? Ok(user) :
                    Problem(detail: $"User ID: {id} was not found!", statusCode: StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while getting UserId: {id}");
                return Problem(detail: ex.Message.ToString(), statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost(Name = "AddUserAsync")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddUserAsync(UserRequestDto userRequestDto)
        {
            try
            {
                // Validate input data using InputValidation methods
                if (!_inputValidation.ValidateModel(userRequestDto, out var validationResults))
                {
                    var validationErrors = _inputValidation.GetValidationErrors(validationResults);
                    _logger.LogWarning("UserProfile model is invalid. Error: {validationErrors}", validationErrors);
                    return Problem(detail: $"Invalid model: {userRequestDto}. Errors: {validationErrors}", statusCode: StatusCodes.Status400BadRequest);
                }

                if (!_inputValidation.ValidateEmailAddress(userRequestDto.Email, out var emailErrorMessage))
                {
                    _logger.LogWarning("Invalid Email Address: {errorMessage}", emailErrorMessage);
                    return Problem(detail: $"Invalid Email: {userRequestDto.Email}. Errors: {emailErrorMessage}", statusCode: StatusCodes.Status400BadRequest);
                }

                // Validate the phone number only if it's not empty
                if (!string.IsNullOrWhiteSpace(userRequestDto.PhoneNumber) &&
                    !_inputValidation.ValidatePhoneNumber(userRequestDto.PhoneNumber, out var phoneValidationError))
                {
                    _logger.LogWarning("Invalid Phone: {errorMessage}", phoneValidationError);
                    return Problem(
                        detail: $"Invalid Phone for UserName: {userRequestDto.UserName}. Errors: {phoneValidationError}",
                        statusCode: StatusCodes.Status400BadRequest);
                }

                // Validate the date of birth only if it's not null
                if (userRequestDto.DateOfBirth != null &&
                    !_inputValidation.ValidateDateOfBirth(userRequestDto.DateOfBirth, out var dateValidationError))
                {
                    _logger.LogWarning("Invalid Date of Birth: {errorMessage}", dateValidationError);
                    return Problem(
                        detail: $"Invalid Date of Birth for UserName: {userRequestDto.UserName}. Errors: {dateValidationError}",
                        statusCode: StatusCodes.Status400BadRequest);
                }

                bool isUserCreated = await _userService.AddUserAsync(userRequestDto);

                return isUserCreated ? this.Created() :
                    Problem(detail: $"Failed to Create Username: {userRequestDto.UserName}!", statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while creating UserName: {userRequestDto.UserName}");
                return Problem(detail: ex.Message.ToString(), statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}", Name = "UpdateUserAsync")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserAsync(string id, [FromBody] UserRequestDto userRequestDto)
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
                if (!_inputValidation.ValidateModel(userRequestDto, out var validationResults))
                {
                    var validationErrors = _inputValidation.GetValidationErrors(validationResults);
                    _logger.LogWarning("UserProfile model is invalid. Errors: {validationErrors}", validationErrors);
                    return Problem(detail: $"Invalid Model for Id: {id}. Errors: {validationErrors}", statusCode: StatusCodes.Status400BadRequest);
                }

                // Validate UserName
                if (!_inputValidation.ValidateUserName(userRequestDto.UserName, out var userNameValidationError))
                {
                    _logger.LogWarning("Invalid Email: {errorMessage}", userNameValidationError);
                    return Problem(detail: $"Invalid UserName: {userRequestDto.UserName}. Errors: {userNameValidationError}", statusCode: StatusCodes.Status400BadRequest);
                }

                // Validate the Email
                if (!_inputValidation.ValidateEmailAddress(userRequestDto.Email, out var emailValidationError))
                {
                    _logger.LogWarning("Invalid Email: {errorMessage}", emailValidationError);
                    return Problem(detail: $"Invalid Email for Id: {id}. Errors: {emailValidationError}", statusCode: StatusCodes.Status400BadRequest);
                }

                // Validate the phone number only if it's not empty
                if (!string.IsNullOrWhiteSpace(userRequestDto.PhoneNumber) &&
                    !_inputValidation.ValidatePhoneNumber(userRequestDto.PhoneNumber, out var phoneValidationError))
                {
                    _logger.LogWarning("Invalid Phone: {errorMessage}", phoneValidationError);
                    return Problem(
                        detail: $"Invalid Phone for UserName: {userRequestDto.UserName}. Errors: {phoneValidationError}",
                        statusCode: StatusCodes.Status400BadRequest);
                }

                // Validate the date of birth only if it's not null
                if (userRequestDto.DateOfBirth != null &&
                    !_inputValidation.ValidateDateOfBirth(userRequestDto.DateOfBirth, out var dateValidationError))
                {
                    _logger.LogWarning("Invalid Date of Birth: {errorMessage}", dateValidationError);
                    return Problem(
                        detail: $"Invalid Date of Birth for UserName: {userRequestDto.UserName}. Errors: {dateValidationError}",
                        statusCode: StatusCodes.Status400BadRequest);
                }

                bool updatedUser = await _userService.UpdateUserAsync(id, userRequestDto);

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
