using BaseApp.Data.User.Dtos;
using BaseApp.ServiceProvider.Interfaces;
using BaseApp.Shared.ErrorHandling;
using BaseApp.Web.Shared;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BaseApp.Web.Pages.User
{
    public partial class Users : ComponentBase
    {
        [Inject] private IUserApiClient ApiClient { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private NotificationService NotificationService { get; set; } = default!;

        private DeleteConfirmation DeleteConfirmationPopup { get; set; } = default!;

        private IEnumerable<UserDto>? users = new List<UserDto>();
        private IList<UserDto>? selectedUsers;

        private int TotalRecords = 0;
        private int PageSize = 5;
        private string SelectedUserId = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await GetUserList(new LoadDataArgs { Skip = 0, Top = PageSize });
        }

        private async Task GetUserList(LoadDataArgs args)
        {
            try
            {
                // Calculate page index based on Radzen's `Skip` and `Top` properties
                var pageIndex = args.Skip / args.Top + 1;
                var pageSize = args.Top;

                var result = await ApiClient.GetUsersAsync((int)pageIndex, (int)pageSize);

                if (result != null)
                {
                    users = result.Items ?? new List<UserDto>();
                    TotalRecords = result.TotalCount;
                }
                else
                {
                    users = new List<UserDto>();
                    TotalRecords = 0;
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Error", ex.Message, NotificationSeverity.Error);
            }
        }

        private void ShowDeletePopup(string userId, string userName)
        {
            SelectedUserId = userId;
            DeleteConfirmationPopup.Message = $"Are you sure you want to delete the user '{userName}'?";
            DeleteConfirmationPopup.OpenPopup();
        }

        private async Task DeleteUser()
        {
            try
            {
                var response = await ApiClient.DeleteUserAsync(SelectedUserId);

                if (response.IsSuccessStatusCode)
                {
                    ShowNotification("Success", "User deleted successfully.", NotificationSeverity.Success);
                    await RefreshUserList();
                }
                else
                {
                    var errorMessage = await ErrorHandler.ExtractErrorMessageAsync(response);
                    ShowNotification("Error", errorMessage, NotificationSeverity.Error);
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Error", ex.Message, NotificationSeverity.Error);
            }
        }

        private void NavigateToEdit(string userId)
        {
            NavigationManager.NavigateTo($"/users/edit/{userId}");
        }

        private void NavigateToCreateUser()
        {
            NavigationManager.NavigateTo("/users/create");
        }

        private async Task RefreshUserList()
        {
            await GetUserList(new LoadDataArgs { Skip = 0, Top = PageSize });
        }

        private void HandleCancel()
        {
            SelectedUserId = string.Empty;
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
