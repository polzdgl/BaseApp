using BaseApp.Data.Context;
using BaseApp.Data.Repositories;
using BaseApp.Data.Repositories.Interfaces;
using BaseApp.Data.User.Dtos;
using BaseApp.ServiceProvider.Interfaces;
using BaseApp.Data.User.Models;
using System.Linq;

namespace BaseApp.ServiceProvider.Services
{
    public class UserService : IUserService
    {
        private IRepositoryFactory _repository;

        public UserService(IRepositoryFactory repository)
        {
            _repository = repository;
        }

        public UserService(ApplicationDbContext context, IRepositoryFactory repository)
        {
            _repository = repository;
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
            var users = await this.Repository.UserRepository.GetAllAsync();

            if(users == null)
            {
                return Enumerable.Empty<UserDto>();
            }

            return users.Select(u => UserDto.FromModel(u));
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await this.Repository.UserRepository.FindAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            return UserDto.FromModel(user);
        }

        public async Task<bool> UpdateUserAsync(int id, UserDto personDto)
        {
            var user = await this.Repository.UserRepository.FindAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            // Update the properties necessary
            user.UserName = personDto.UserName;
            user.Email = personDto.Email;
            user.FirstName = personDto.FirstName;
            user.LastName = personDto.LastName;
            user.PhoneNumber = personDto.PhoneNumber;
            user.DateOfBirth = personDto.DateOfBirth;
            user.EmailConfirmed = personDto.EmailConfirmed;
            user.PhoneNumberConfirmed = personDto.PhoneNumberConfirmed;

            return await this.Repository.UserRepository.UpdateAsync(user);
        }

        public async Task<bool> AddUserAsync(UserDto personDto)
        {
            return await this.Repository.UserRepository.CreateAsync(User.FromDto(personDto));
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await this.Repository.UserRepository.FindAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            return await this.Repository.UserRepository.DeleteAsync(user);
        }
    }
}
