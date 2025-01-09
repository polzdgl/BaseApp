using BaseApp.Data.User.Dtos;
using BaseApp.ServiceProvider.Interfaces.User;
using BaseApp.Shared.ErrorHandling;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BaseApp.Client.Pages.User
{
    public partial class Create
    {
        [Inject] protected NotificationService NotificationService { get; set; } = default!;
        [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IUserApiClient UserApiClient { get; set; } = default!;

        private bool IsLoading = false;
        private bool IsSaving = false;
        private UserProfileDto UserProfileDto { get; set; } = new UserProfileDto();

        protected override async Task OnInitializedAsync()
        {
            IsLoading = false;
        }

        protected async Task CreateUserAsync()
        {
            try
            {
                IsSaving = true;

                var response = await UserApiClient.CreateUserAsync(UserProfileDto);

                if (response.IsSuccessStatusCode)
                {
                    ShowNotification("Success", "User created successfully!", NotificationSeverity.Success);
                    NavigationManager.NavigateTo("/user/all");
                }
                else
                {
                    // Extract and show validation errors from server response
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
