using BaseApp.Data.User.Dtos;
using BaseApp.Web.ErrorHandling;
using BaseApp.Web.ServiceClients;
using Microsoft.AspNetCore.Components;

namespace BaseApp.Web.Pages.User
{
    public partial class Create
    {
        [Inject] private ApiClient ApiClient { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        private bool IsLoading = true;
        private bool IsSaving = false;
        private bool HasError = false;
        private string? ErrorMessage = string.Empty;

        private string? _errorMessage;

        [SupplyParameterFromForm]
        private UserRequestDto? UserRequestDto { get; set; } = new UserRequestDto();

        protected override async Task OnInitializedAsync()
        {
            IsLoading = false;
        }

        private async Task CreateUserAsync()
        {
            IsSaving = true;
            ResetErrorState();

            try
            {
                await ApiClient.CreateUserAsync(UserRequestDto);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
            finally
            {
                IsSaving = false;

                if (!HasError) 
                { 
                    NavigationManager.NavigateTo("users"); 
                }         
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
