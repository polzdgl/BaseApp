using BaseApp.Data.User.Dtos;
using BaseApp.Web.ServiceClients;
using Microsoft.AspNetCore.Components;

namespace BaseApp.Web.Pages.User
{
    public partial class Create
    {
        private NavigationManager _navigationManager;
        private readonly ApiClient _apiClient;

        public Create(NavigationManager navigationManager, ApiClient apiClient)
        {
            _navigationManager = navigationManager;
            _apiClient = apiClient;
        }

        private bool IsLoading = true;
        private bool HasError = false;
        private bool IsSaving = false;
        private string? ErrorMessage { get; set; }

        [SupplyParameterFromForm]
        private UserRequestDto? UserRequestDto { get; set; } = new UserRequestDto();

        protected override async Task OnInitializedAsync()
        {
        }

        private async Task CreateUserAsync()
        {
            IsSaving = true;
            HasError = false;
            ErrorMessage = string.Empty;

            try
            {
                await _apiClient.CreateUserAsync(UserRequestDto);
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
