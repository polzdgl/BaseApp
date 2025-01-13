using BaseApp.Client.Shared;
using BaseApp.Data.User.Dtos;
using BaseApp.ServiceProvider.User.Interfaces;
using BaseApp.Shared.ErrorHandling;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BaseApp.Client.Pages.User
{
    public partial class Users : ComponentBase
    {
        [Inject] protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject] protected NotificationService NotificationService { get; set; } = default!;

        [Inject] private IUserProvider UserProvider { get; set; } = default!;


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


        // Gets the list of Users from the Backend api.
        // This method uses severside paging
        private async Task GetUserList(LoadDataArgs args)
        {
                try
                {
                // Calculate page index based on Radzen's `Skip` and `Top` properties
                var pageIndex = args.Skip / args.Top + 1;
                var pageSize = args.Top;

                var result = await UserProvider.GetUsersAsync((int)pageIndex, (int)pageSize);

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

        // This method is called when the user clicks on the delete button
        // This method shows a confirmation popup before deleting the user
        private void ShowDeletePopup(string userId, string userName)
        {
            SelectedUserId = userId;
            DeleteConfirmationPopup.Message = $"Are you sure you want to delete the user '{userName}'?";
            DeleteConfirmationPopup.OpenPopup();
        }

        // This method is called when the user confirms the deletion
        private async Task DeleteUser()
        {
            try
            {
                var response = await UserProvider.DeleteUserAsync(SelectedUserId);

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

        // This method is called when the user clicks on the edit button
        private void NavigateToEdit(string userId)
        {
            NavigationManager.NavigateTo($"/user/edit/{userId}");
        }

        // This method is called when the user clicks on the create button
        private void NavigateToCreateUser()
        {
            NavigationManager.NavigateTo("/user/create");
        }

        // This method is called when the user navigates to the Users page 
        // Refresh the user list
        private async Task RefreshUserList()
        {
            await GetUserList(new LoadDataArgs { Skip = 0, Top = PageSize });
        }

        // This method is called when the user cancels the deletion
        private void HandleCancel()
        {
            SelectedUserId = string.Empty;
        }

        // Show a notification message
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