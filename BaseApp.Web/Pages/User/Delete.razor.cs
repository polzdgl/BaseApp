using BaseApp.Web.ErrorHandling;
using BaseApp.Web.ServiceClients;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace BaseApp.Web.Pages.User
{
    public partial class Delete
    {
        [Inject] private ApiClient ApiClient { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        [Parameter]
        public int Id { get; set; }

        [Parameter]
        public string? UserName { get; set; }

        internal bool IsDeleted = false;
        private bool IsLoading = true;
        private bool HasError = false;
        private string? ErrorMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await GetQueryParameters();
        }

        private async Task GetQueryParameters()
        {
            try
            {
                var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
                if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("userName", out var userName))
                {
                    UserName = userName;
                }
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = ex.Message;
            }
        }

        private async Task DeleteUser()
        {
            try
            {
                IsDeleted = true;
                ResetErrorState();

                await ApiClient.DeleteUserAsync(Id);
                NavigationManager.NavigateTo("users");
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
            finally
            {
                IsDeleted = false;
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
