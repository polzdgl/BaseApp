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

        private DeleteConfirmation DeleteConfirmationPopup { get; set; } = default!;

        private bool HasError = false;
        private string? ErrorMessage = string.Empty;
        private string SelectedUserId = string.Empty;

        private IEnumerable<UserDto>? users = new List<UserDto>();
        private IList<UserDto>? selectedUsers;

        private int TotalRecords = 0;
        private int PageSize = 5;

        protected override async Task OnInitializedAsync()
        {
            await GetUserList(new LoadDataArgs { Skip = 0, Top = PageSize });
        }

        private async Task GetUserList(LoadDataArgs args)
        {
            try
            {
                ResetErrorState();

                // Calculate page index based on Radzen's `Skip` and `Top` properties
                var pageIndex = args.Skip / args.Top + 1;
                var pageSize = args.Top;

                // Fetch paginated users from the API
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
                HandleError(ex);
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
                ResetErrorState();
                var response = await ApiClient.DeleteUserAsync(SelectedUserId);

                if (response.IsSuccessStatusCode)
                {
                    // Refresh User List
                    await RefreshUserList();
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
        }

        private void NavigateToEdit(string userId)
        {
            NavigationManager.NavigateTo($"/users/edit/{userId}");
        }

        private async Task RefreshUserList()
        {
            await OnInitializedAsync(); // Reload the list after deletion.
        }

        private void HandleCancel()
        {
            SelectedUserId = string.Empty; // Reset the selection
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