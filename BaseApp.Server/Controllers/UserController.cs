﻿using BaseApp.Data.User.Dtos;
using BaseApp.ServiceProvider.Interfaces.User;
using BaseApp.Shared.Dtos;
using BaseApp.Shared.Validation;
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

        public UserController(ILogger<UserController> logger, IUserService userService, InputValidation inputValidation)
        {
            _logger = logger;
            _userService = userService;
            _inputValidation = inputValidation;
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
                if (!_inputValidation.ValidateId(id, out var numberValidationError))
                {
                    _logger.LogWarning("Invalid Id: {id}. Errors: {erorrMessage}", id, numberValidationError);
                    return Problem(detail: $"Invalid Id: {id}. Errors: {numberValidationError}", statusCode: StatusCodes.Status400BadRequest);
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
                // Validate input data using InputValidation methods
                if (!_inputValidation.ValidateModel(userProfileDto, out var validationResults))
                {
                    var validationErrors = _inputValidation.GetValidationErrors(validationResults);
                    _logger.LogWarning("UserProfile model is invalid. Error: {validationErrors}", validationErrors);
                    return Problem(detail: $"Invalid model: {userProfileDto}. Errors: {validationErrors}", statusCode: StatusCodes.Status400BadRequest);
                }

                if (!_inputValidation.ValidateEmailAddress(userProfileDto.Email, out var emailErrorMessage))
                {
                    _logger.LogWarning("Invalid Email Address: {errorMessage}", emailErrorMessage);
                    return Problem(detail: $"Invalid Email: {userProfileDto.Email}. Errors: {emailErrorMessage}", statusCode: StatusCodes.Status400BadRequest);
                }

                // Validate the phone number only if it's not empty
                if (!string.IsNullOrWhiteSpace(userProfileDto.PhoneNumber) &&
                    !_inputValidation.ValidatePhoneNumber(userProfileDto.PhoneNumber, out var phoneValidationError))
                {
                    _logger.LogWarning("Invalid Phone: {errorMessage}", phoneValidationError);
                    return Problem(
                        detail: $"Invalid Phone for UserName: {userProfileDto.UserName}. Errors: {phoneValidationError}",
                        statusCode: StatusCodes.Status400BadRequest);
                }

                // Validate the date of birth only if it's not null
                if (userProfileDto.DateOfBirth != null &&
                    !_inputValidation.ValidateDateOfBirth(userProfileDto.DateOfBirth, out var dateValidationError))
                {
                    _logger.LogWarning("Invalid Date of Birth: {errorMessage}", dateValidationError);
                    return Problem(
                        detail: $"Invalid Date of Birth for UserName: {userProfileDto.UserName}. Errors: {dateValidationError}",
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

                // Validate UserName
                if (!_inputValidation.ValidateUserName(userProfileDto.UserName, out var userNameValidationError))
                {
                    _logger.LogWarning("Invalid Email: {errorMessage}", userNameValidationError);
                    return Problem(detail: $"Invalid UserName: {userProfileDto.UserName}. Errors: {userNameValidationError}", statusCode: StatusCodes.Status400BadRequest);
                }

                // Validate the Email
                if (!_inputValidation.ValidateEmailAddress(userProfileDto.Email, out var emailValidationError))
                {
                    _logger.LogWarning("Invalid Email: {errorMessage}", emailValidationError);
                    return Problem(detail: $"Invalid Email for Id: {id}. Errors: {emailValidationError}", statusCode: StatusCodes.Status400BadRequest);
                }

                // Validate the phone number only if it's not empty
                if (!string.IsNullOrWhiteSpace(userProfileDto.PhoneNumber) &&
                    !_inputValidation.ValidatePhoneNumber(userProfileDto.PhoneNumber, out var phoneValidationError))
                {
                    _logger.LogWarning("Invalid Phone: {errorMessage}", phoneValidationError);
                    return Problem(
                        detail: $"Invalid Phone for UserName: {userProfileDto.UserName}. Errors: {phoneValidationError}",
                        statusCode: StatusCodes.Status400BadRequest);
                }

                // Validate the date of birth only if it's not null
                if (userProfileDto.DateOfBirth != null &&
                    !_inputValidation.ValidateDateOfBirth(userProfileDto.DateOfBirth, out var dateValidationError))
                {
                    _logger.LogWarning("Invalid Date of Birth: {errorMessage}", dateValidationError);
                    return Problem(
                        detail: $"Invalid Date of Birth for UserName: {userProfileDto.UserName}. Errors: {dateValidationError}",
                        statusCode: StatusCodes.Status400BadRequest);
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
