using BaseApp.Data.User.Dtos;
using BaseApp.ServiceProvider.Auth.Interfaces;
using BaseApp.Shared.ErrorHandling;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BaseApp.Client.Pages.Auth
{
    public partial class Login
    {
        [Inject] protected NotificationService NotificationService { get; set; } = default!;
        [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IAuthClient AuthClient { get; set; } = default!;

        private bool IsLoading = false;
        private bool IsLoggingIn = false;

        private UserLoginDto UserLoginDto { get; set; } = new UserLoginDto();

        protected override async Task OnInitializedAsync()
        {
            IsLoading = false;
        }

        // This method is called when the form is submitted, and it logs in the user
        protected async Task OnLoginAsync()
        {
            try
            {
                IsLoggingIn = true;

                var response = await AuthClient.LoginAsync(UserLoginDto);

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

        // This method shows the notification message on the screen
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
