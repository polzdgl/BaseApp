using BaseApp.Data.User.Dtos;
using BaseApp.ServiceProvider.Interfaces;
using BaseApp.Shared.ErrorHandling;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BaseApp.Web.Pages.User
{
    public partial class Create : ComponentBase
    {
        [Inject] private IUserApiClient ApiClient { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private NotificationService NotificationService { get; set; } = default!;

        private bool IsLoading = true;
        private bool IsSaving = false;

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

                var response = await ApiClient.CreateUserAsync(UserRequestDto);

                if (response.IsSuccessStatusCode)
                {
                    ShowNotification("Success", "User created successfully.", NotificationSeverity.Success);
                    NavigationManager.NavigateTo("users");
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
