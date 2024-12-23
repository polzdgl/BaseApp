using BaseApp.Data.User.Dtos;
using BaseApp.Web.ErrorHandling;
using BaseApp.Web.ServiceClients;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BaseApp.Web.Pages.User
{
    public partial class Users : ComponentBase
    {
        [Inject] private ApiClient ApiClient { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        private IEnumerable<Data.User.Dtos.UserDto> users;

        private bool IsLoading = true;
        private bool HasError = false;
        private string? ErrorMessage = string.Empty;

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