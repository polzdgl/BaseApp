using BaseApp.Data.User.Dtos;
using BaseApp.Web.ServiceClients;
using BaseApp.Web.Shared;
using Microsoft.AspNetCore.Components;

namespace BaseApp.Web.Pages.User
{
    public partial class Users : ComponentBase
    {
        [Inject] private ApiClient ApiClient { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        private DeleteConfirmation DeleteConfirmationPopup { get; set; } = default!;

        private bool IsLoading = true;
        private bool HasError = false;
        private string? ErrorMessage = string.Empty;
        private string SelectedUserId = string.Empty;

        private IEnumerable<UserDto>? users;

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

                users = await ApiClient.GetUsersAsync();
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
                await ApiClient.DeleteUserAsync(SelectedUserId);
                await RefreshUserList();
            }
            catch (Exception ex)
            {
                HandleError(ex);
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