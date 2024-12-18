using BaseApp.Data.User.Dtos;
using BaseApp.Web.Clients;
using Microsoft.AspNetCore.Components;

namespace BaseApp.Web.Components.Pages.User
{
    public partial class Create
    {
        private NavigationManager _navigationManager;
        private readonly UserClient _userClient;

        public Create(NavigationManager navigationManager, UserClient userClient)
        {
            _navigationManager = navigationManager;
            _userClient = userClient;
        }

        private bool IsLoading = true;
        private bool HasError = false;
        private bool IsSaving = false;
        private string? ErrorMessage { get; set; }

        [SupplyParameterFromForm]
        private UserRequestDto? UserRequestDto { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (UserRequestDto == null)
            {
                UserRequestDto = new UserRequestDto();
            }
        }

        private async Task CreateUserAsync()
        {
            IsSaving = true;
            HasError = false;
            ErrorMessage = string.Empty;

            try
            {
                await _userClient.CreateUserAsync(UserRequestDto);
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
