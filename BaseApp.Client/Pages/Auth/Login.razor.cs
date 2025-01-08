using BaseApp.Data.User.Dtos;
using BaseApp.ServiceProvider.Interfaces.Auth;
using BaseApp.Shared.ErrorHandling;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BaseApp.Client.Pages.Auth
{
    public partial class Login
    {
        [Inject] protected NotificationService NotificationService { get; set; } = default!;
        [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IAuthApiClient AuthApiClient { get; set; } = default!;

        private bool IsLoading = false;
        private bool IsLoggingIn = false;

        private UserLoginDto UserLoginDto { get; set; } = new UserLoginDto();

        protected override async Task OnInitializedAsync()
        {
            IsLoading = false;
        }

        protected async Task OnLoginAsync()
        {
            try
            {
                IsLoggingIn = true;

                var response = await AuthApiClient.LoginAsync(UserLoginDto);

                if (response.IsSuccessStatusCode)
                {
                    ShowNotification("Success", "You have logged in successfully!", NotificationSeverity.Success);
                    NavigationManager.NavigateTo("/");
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
                IsLoggingIn = false;
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
