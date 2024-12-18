using BaseApp.Data.User.Dtos;
using BaseApp.Web.Clients;
using Microsoft.AspNetCore.Components;

namespace BaseApp.Web.Components.Pages.User
{
    public partial class UserDetails
    {
        private readonly NavigationManager _navigationManager;
        private readonly UserClient _userClient;

        public UserDetails(NavigationManager navigationManager, UserClient userClient)
        {
            _navigationManager = navigationManager;
            _userClient = userClient;
        }

        [Parameter]
        public int Id { get; set; } // User Id from url

        [SupplyParameterFromForm]
        private UserDto? User { get; set; }

        private bool IsLoading = true;
        private bool HasError = false;
        private bool IsSaving = false;
        private string? ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (Id > 0)
            {
                await GetUserAsync();
            }
        }

        private async Task GetUserAsync()
        {
            try
            {
                IsLoading = true;
                HasError = false;
                ErrorMessage = string.Empty;

                if (User == null)
                {
                    User = await _userClient.GetUserAsync(Id);
                }
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task UpdateUserAsync()
        {
            IsSaving = true;
            ErrorMessage = string.Empty;
            HasError = false;

            try
            {
                if (Id > 0)
                {
                    UserRequestDto userRequestDto = new UserRequestDto
                    {
                        UserName = User.UserName,
                        FirstName = User.FirstName,
                        LastName = User.LastName,
                        Email = User.Email,
                        PhoneNumber = User.PhoneNumber,
                        DateOfBirth = User.DateOfBirth,
                    };

                    await _userClient.UpdateUserAsync(Id, userRequestDto);
                }
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsSaving = false;
            }
        }
    }
}
