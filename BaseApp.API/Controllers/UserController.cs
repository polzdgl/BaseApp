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

        [HttpGet(Name = "GetUsersAsync")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsersAsync()
        {
            try
            {
                IEnumerable<UserDto> users = await _userService.GetAllUserAsync();

                return users is not null ? Ok(users) :
                    Problem(detail: $"No users were found!", statusCode: StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while getting all Users list!");
                return Problem(detail: $"An unexpected error occurred while getting all Users list!", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}", Name = "GetUserByIdAsync")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {
            try
            {
                if (!_inputValidation.ValidateNumber(id, out var numberValidationError))
                {
                    _logger.LogWarning("Invalid Id: {id}. Errors: {erorrMessage}", id, numberValidationError);
                    return Problem(detail: $"Invalid Id: {id}. Errors: {numberValidationError}", statusCode: StatusCodes.Status400BadRequest);
                }

                UserDto user = await _userService.GetUserByIdAsync(id);

                return user is not null ? Ok(user) :
                    Problem(detail: $"No UserId: {id} was found!", statusCode: StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while getting UserId: {id}");
                return Problem(detail: $"An unexpected error occurred while getting UserId: {id}", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost(Name = "AddUserAsync")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddUserAsync(UserDto userRequest)
        {
            try
            {
                // Validate input data using InputValidation methods
                if (!_inputValidation.ValidateModel(userRequest, out var validationResults))
                {
                    var validationErrors = _inputValidation.GetValidationErrors(validationResults);
                    _logger.LogWarning("UserProfile model is invalid. Error: {validationErrors}", validationErrors);
                    return Problem(detail: $"Invalid model: {userRequest}. Errors: {validationErrors}", statusCode: StatusCodes.Status400BadRequest);
                }

                if (!_inputValidation.ValidateEmailAddress(userRequest.Email, out var emailErrorMessage))
                {
                    _logger.LogWarning("Invalid Email Address: {errorMessage}", emailErrorMessage);
                    return Problem(detail: $"Invalid Email: {userRequest.Email}. Errors: {emailErrorMessage}", statusCode: StatusCodes.Status400BadRequest);
                }

                bool isUserCreated = await _userService.AddUserAsync(userRequest);

                return isUserCreated ? this.Created() :
                    Problem(detail: $"Failed to create UserName: {userRequest.UserName}!", statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while creating UserName: {userRequest.UserName}");
                return Problem(detail: $"An unexpected error occurred while creating UserName: {userRequest.UserName}", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}", Name = "UpdateUserAsync")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody] UserDto userRequest)
        {
            try
            {
                // Validate the UserAccountId
                if (!_inputValidation.ValidateNumber(id, out var numberValidationError))
                {
                    _logger.LogWarning("Invalid Id: {errorMessage}", numberValidationError);
                    return Problem(detail: $"Invalid Id: {id}. Errors: {numberValidationError}", statusCode: StatusCodes.Status400BadRequest);
                }

                // Validate model
                if (!_inputValidation.ValidateModel(userRequest, out var validationResults))
                {
                    var validationErrors = _inputValidation.GetValidationErrors(validationResults);
                    _logger.LogWarning("UserProfile model is invalid. Errors: {validationErrors}", validationErrors);
                    return Problem(detail: $"Invalid Id: {id}. Errors: {validationErrors}", statusCode: StatusCodes.Status400BadRequest);
                }

                bool updatedUser = await _userService.UpdateUserAsync(id, userRequest);

                return updatedUser ? this.Ok() :
                  Problem(detail: $"Failed to update UserId: {userRequest.Id}!", statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while updating UserId: {userRequest.Id}");
                return Problem(detail: $"An unexpected error occurred while updating User: {userRequest.Id}", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}", Name = "DeleteUserAsync")]
        [ProducesResponseType(typeof(User), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            try
            {
                // Validate the UserAccountId
                if (!_inputValidation.ValidateNumber(id, out var numberValidationError))
                {
                    _logger.LogWarning("Invalid Id: {errorMessage}", numberValidationError);
                    return Problem(detail: $"Invalid Id: {id}. Errors: {numberValidationError}", statusCode: StatusCodes.Status400BadRequest);
                }

                bool deletedUser = await _userService.DeleteUserAsync(id);

                return deletedUser ? this.Ok() :
                 Problem(detail: $"Failed to delete UserId: {id}!", statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while deleting a UserId: {id}");
                return Problem(detail: $"An unexpected error occurred while deleting a UserId: {id}", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
