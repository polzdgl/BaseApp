﻿using BaseApp.Data.Repositories;
using BaseApp.Data.Repositories.Interfaces;
using BaseApp.Data.User.Dtos;
using BaseApp.Data.User.Models;
using BaseApp.ServiceProvider.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;

namespace BaseApp.ServiceProvider.Services
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
                if (this._repository == null)
                {
                    this._repository = new RepositoryFactory(this.Repository.Context);
                }

                return this._repository;
            }
        }

        public async Task<IEnumerable<UserDto>> GetAllUserAsync()
        {
            _logger.LogInformation("Getting all Users");

            var users = await this.Repository.UserRepository.GetAllAsync();

            if (users == null)
            {
                return Enumerable.Empty<UserDto>();
            }

            return users.Select(u => UserDto.FromModel(u));
        }

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            _logger.LogInformation("Getting User with Id: {id}", id);

            var user = await this.Repository.UserRepository.GetUserAsync(id);

            if (user == null)
            {
                _logger.LogWarning("User with ID {id} not found", id);
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            return UserDto.FromModel(user);
        }

        public async Task<bool> UpdateUserAsync(string id, UserRequestDto userRequestDto)
        {
            var user = await this.Repository.UserRepository.GetUserAsync(id);

            if (user == null)
            {
                _logger.LogWarning("User with ID {id} not found", id);
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            // Check if UserName has changed
            if (userRequestDto.UserName != user.UserName)
            {
                // Check if UserName already taken
                bool isUserNameTaken = await this.Repository.UserRepository.IsUserNameTaken(id, userRequestDto.UserName);

                if (isUserNameTaken)
                {
                    _logger.LogWarning("UserName: {userName} is not available!", userRequestDto.UserName);
                    throw new DuplicateNameException("UserName not available!");
                }
            }

            _logger.LogInformation("Updating User with Id: {id}", id);

            // Update the properties necessary
            user.UserName = userRequestDto.UserName;
            user.Email = userRequestDto.Email;
            user.FirstName = userRequestDto.FirstName;
            user.LastName = userRequestDto.LastName;
            user.PhoneNumber = userRequestDto.PhoneNumber;
            user.DateOfBirth = userRequestDto.DateOfBirth;
            user.IsActive = userRequestDto.IsActive;

            return await this.Repository.UserRepository.UpdateAsync(user);
        }

        public async Task<bool> AddUserAsync(UserRequestDto userRequestDto)
        {
            // Check if UserName already exists
            bool isExistingUserName = await this.Repository.UserRepository.IsExistingUser(userRequestDto.UserName);

            if (isExistingUserName)
            {
                _logger.LogWarning("UserName: {userName} already exist!", userRequestDto.UserName);
                throw new DuplicateNameException("UserName already exist!");
            }

            _logger.LogInformation("Creating new User for UserName: {userName}, Email: {email}", userRequestDto.UserName, userRequestDto.Email);

            // Set User IsActive = 1 by default
            userRequestDto.IsActive = true;

            return await this.Repository.UserRepository.CreateAsync(ApplicationUser.FromDto(userRequestDto));
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            _logger.LogInformation("Deleting User with Id: {id}", id);

            var user = await this.Repository.UserRepository.GetUserAsync(id);

            if (user == null)
            {
                _logger.LogWarning("User with Id: {id} not found", id);
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            return await this.Repository.UserRepository.DeleteAsync(user);
        }
    }
}
