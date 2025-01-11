using BaseApp.Data.User.Dtos;
using BaseApp.ServiceProvider.User.Interfaces;
using BaseApp.Shared.ErrorHandling;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BaseApp.Client.Pages.User
{
    public partial class Edit : ComponentBase
    {
        [Parameter] public string Id { get; set; } = string.Empty;

        [Inject] private IUserProvider UserProvider { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private NotificationService NotificationService { get; set; } = default!;

        private UserDto? User { get; set; }
        private bool IsLoading = true;
        private bool IsSaving = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadUserAsync();
        }

        private async Task LoadUserAsync()
        {
            if (string.IsNullOrEmpty(Id))
            {
                ShowNotification("Error", "User ID cannot be null or empty.", NotificationSeverity.Error);
                return;
            }

            try
            {
                IsLoading = true;
                User = await UserProvider.GetUserAsync(Id);
            }
            catch (Exception ex)
            {
                ShowNotification("Error", ex.Message, NotificationSeverity.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SaveUserAsync()
        {
            try
            {
                IsSaving = true;

                if (User == null)
                {
                    ShowNotification("Error", "No User data is found!", NotificationSeverity.Error);
                    return;
                }

                var userRequest = MapToRequestDto(User);
                var response = await UserProvider.EditUserAsync(Id, userRequest);

                if (response.IsSuccessStatusCode)
                {
                    ShowNotification("Success", "User updated successfully.", NotificationSeverity.Success);
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

        private static UserProfileDto MapToRequestDto(UserDto user)
        {
            return new UserProfileDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                IsActive = user.IsActive
            };
        }

        private void ShowNotification(string summary, string detail, NotificationSeverity severity)
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = severity,
                Summary = summary,
                Detail = detail,
                Duration = 3000
            });
        }
    }
}