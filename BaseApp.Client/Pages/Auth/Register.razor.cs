using BaseApp.Data.User.Dtos;
using BaseApp.ServiceProvider.Interfaces.Auth;
using BaseApp.Shared.ErrorHandling;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BaseApp.Client.Pages.Auth
{
    public partial class Register
    {

        [Inject] protected NotificationService NotificationService { get; set; } = default!;
        [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IAuthApiClient AuthApiClient { get; set; } = default!;

        private bool IsLoading = false;
        private bool IsSaving = false;

        [SupplyParameterFromForm]
        private UserRegisterDto UserRegisterDto { get; set; } = new UserRegisterDto();

        protected async Task OnInitializedAsync()
        {
            IsLoading = false;
        }

        protected async Task OnRegisterAsync()
        {
            try
            {
                IsSaving = true;

                var response = await AuthApiClient.RegisterAsync(UserRegisterDto);

                if (response.IsSuccessStatusCode)
                {
                    ShowNotification("Success", "You have registered successfully!", NotificationSeverity.Success);
                    NavigationManager.NavigateTo("/user/all");
                }
                else
                {
                    string errorMessage = await ErrorHandler.ExtractErrorMessageAsync(response);
                    ShowNotification("Error", errorMessage, NotificationSeverity.Error);
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Error", ex.Message, NotificationSeverity.Error);
            }
            finally
            {
                IsSaving = false;
            }
        }

        private void ShowNotification(string summary, string detail, NotificationSeverity severity)
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = severity,
                Summary = summary,
                Detail = detail,
                Duration = 4000
            });
        }
    }
}