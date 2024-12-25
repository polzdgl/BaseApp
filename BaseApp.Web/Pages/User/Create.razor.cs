using BaseApp.Data.User.Dtos;
using BaseApp.ServiceProvider.Interfaces;
using BaseApp.Web.Shared;
using Microsoft.AspNetCore.Components;

namespace BaseApp.Web.Pages.User
{
    public partial class Create : ComponentBase
    {
        [Inject] private IUserApiClient ApiClient { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        private bool IsLoading = true;
        private bool IsSaving = false;
        private bool HasError = false;
        private string? ErrorMessage = string.Empty;

        [SupplyParameterFromForm]
        private UserRequestDto? UserRequestDto { get; set; } = new UserRequestDto();

        protected override async Task OnInitializedAsync()
        {
            IsLoading = false;
        }

        private async Task CreateUserAsync()
        {
            try
            {
                IsSaving = true;
                ResetErrorState();

                var response = await ApiClient.CreateUserAsync(UserRequestDto);

                if (response.IsSuccessStatusCode)
                {
                    // Navigate to the users list on success
                    NavigationManager.NavigateTo("users");
                }
                else
                {
                    HasError = true;    
                    ErrorMessage = await ErrorHandler.ExtractErrorMessageAsync(response);
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
            finally
            {
                IsSaving = false;      
            }
        }

        private void ResetErrorState()
        {
            ErrorHandler.ResetErrorState(ref HasError, ref ErrorMessage);
        }

        private void HandleError(Exception ex)
        {
            ErrorHandler.HandleError(ex, ref HasError, ref ErrorMessage);
        }
    }
}
