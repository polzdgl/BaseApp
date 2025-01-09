using BaseApp.Data.Repositories;
using BaseApp.Data.Repositories.Interfaces;
using BaseApp.Data.User.Dtos;
using BaseApp.Data.User.Models;
using BaseApp.ServiceProvider.Interfaces.User;
using BaseApp.Shared.Dtos;
using Microsoft.Extensions.Logging;
using System.Data;

namespace BaseApp.ServiceProvider.Services.User
{
    public class UserService : IUserService
    {
        private IRepositoryFactory _repository;
        private readonly ILogger _logger;

        public UserService(IRepositoryFactory repository, ILogger<UserService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public IRepositoryFactory Repository
        {
            get
            {
                if (_repository == null)
                {
                    _repository = new RepositoryFactory(Repository.Context);
                }

                return _repository;
            }
        }

        public async Task<PaginatedResult<UserDto>> GetUsersAsync(int page, int pageSize)
        {
            _logger.LogInformation($"Getting Users for page {page} with page size {pageSize}");

            var totalUsers = await Repository.UserRepository.CountAsync(); // Get total user count
            var users = await Repository.UserRepository.GetPagedAsync(page, pageSize); // Get paginated data

            if (users == null || !users.Any())
            {
                return new PaginatedResult<UserDto>
                {
                    Items = Enumerable.Empty<UserDto>(),
                    TotalCount = 0,
                    Page = page,
                    PageSize = pageSize
                };
            }

            return new PaginatedResult<UserDto>
            {
                Items = users.Select(u => UserDto.FromModel(u)),
                TotalCount = totalUsers,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<UserDto> GetUserAsync(string id)
        {
            _logger.LogInformation("Getting User with Id: {id}", id);

            var user = await Repository.UserRepository.FindAsync(id);

            if (user == null)
            {
                _logger.LogWarning("User with ID {id} not found", id);
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            return UserDto.FromModel(user);
        }

        public async Task<bool> UpdateUserAsync(string id, UserProfileDto userProfileDto)
        {
            var user = await Repository.UserRepository.GetUserAsync(id);

            if (user == null)
            {
                _logger.LogWarning("User with ID {id} not found", id);
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            // Check if UserName has changed
            if (userProfileDto.UserName != user.UserName)
            {
                // Check if UserName already taken
                bool isUserNameTaken = await Repository.UserRepository.IsUserNameTakenAsync(id, userProfileDto.UserName);

                if (isUserNameTaken)
                {
                    _logger.LogWarning("UserName: {userName} is not available!", userProfileDto.UserName);
                    throw new DuplicateNameException("UserName not available!");
                }
            }

            _logger.LogInformation("Updating User with Id: {id}", id);

            // Update the properties necessary
            user.UserName = userProfileDto.UserName;
            user.Email = userProfileDto.Email;  
            user.FirstName = userProfileDto.FirstName;
            user.LastName = userProfileDto.LastName;
            user.PhoneNumber = userProfileDto.PhoneNumber;
            user.DateOfBirth = userProfileDto.DateOfBirth;
            user.IsActive = userProfileDto.IsActive;

            return await Repository.UserRepository.UpdateAsync(user);
        }

        public async Task<bool> CreateUserAsync(UserProfileDto userProfileDto)
        {
            // Check if UserName already exists
            bool isExistingUserName = await Repository.UserRepository.IsExistingUserAsync(userProfileDto.UserName);

            if (isExistingUserName)
            {
                _logger.LogWarning("UserName: {userName} already exist!", userProfileDto.UserName);
                throw new DuplicateNameException("UserName already exist!");
            }

            _logger.LogInformation("Creating new User for UserName: {userName}, Email: {email}", userProfileDto.UserName, userProfileDto.Email);

            // Set User IsActive = 1 by default
            userProfileDto.IsActive = true;

            return await Repository.UserRepository.CreateAsync(ApplicationUser.FromDto(userProfileDto));
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            _logger.LogInformation("Deleting User with Id: {id}", id);

            var user = await Repository.UserRepository.GetUserAsync(id);

            if (user == null)
            {
                _logger.LogWarning("User with Id: {id} not found", id);
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            return await Repository.UserRepository.DeleteAsync(user);
        }
    }
}
