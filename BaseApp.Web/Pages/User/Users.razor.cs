using BaseApp.Data.User.Dtos;
using BaseApp.ServiceProvider.Interfaces;
using BaseApp.Shared.ErrorHandling;
using BaseApp.Web.Shared;
using Microsoft.AspNetCore.Components;

namespace BaseApp.Web.Pages.User
{
    public partial class Users : ComponentBase
    {
        [Inject] private IUserApiClient ApiClient { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        private DeleteConfirmation DeleteConfirmationPopup { get; set; } = default!;

        private bool IsLoading = true;
        private bool HasError = false;
        private string? ErrorMessage = string.Empty;
        private string SelectedUserId = string.Empty;

        private IEnumerable<UserDto>? users;
        private int CurrentPage = 1;
        private int TotalPages = 1;
        private int PageSize = 10; // Default page size

        protected override async Task OnInitializedAsync()
        {
            await GetUserList();
        }

        private async Task GetUserList()
        {
            try
            {
                IsLoading = true;
                ResetErrorState();

                // Fetch paginated users from the API
                var result = await ApiClient.GetUsersAsync(CurrentPage, PageSize);

                if (result == null)
                {
                    users = new List<UserDto>();
                    TotalPages = 0;
                    return;
                }

                users = result.Items ?? new List<UserDto>();
                TotalPages = (int)Math.Ceiling((double)result.TotalCount / PageSize);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
            finally
            {
                IsLoading = false;
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

        private async Task LoadPage(int page)
        {
            if (page < 1 || page > TotalPages)
                return;

            CurrentPage = page;
            await GetUserList();
        }

        private async Task UpdatePageSize(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newPageSize) && newPageSize > 0)
            {
                PageSize = newPageSize;
                CurrentPage = 1; // Reset to the first page
                await GetUserList();
            }
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