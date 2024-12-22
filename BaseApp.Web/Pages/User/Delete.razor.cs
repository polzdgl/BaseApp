using BaseApp.Web.ServiceClients;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace BaseApp.Web.Pages.User
{
    public partial class Delete
    {
        private readonly NavigationManager _navigationManager;
        private readonly ApiClient _apiClient;

        public Delete(NavigationManager navigationManager, ApiClient apiClient)
        {
            _navigationManager = navigationManager;
            _apiClient = apiClient;
        }

        [Parameter]
        public int Id { get; set; }

        [Parameter]
        public string? UserName { get; set; }

        internal bool IsDeleted = false;
        private bool HasError = false;
        private string? ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var uri = _navigationManager.ToAbsoluteUri(_navigationManager.Uri);
                if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("userName", out var userName))
                {
                    UserName = userName;
                }
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
                ErrorMessage = string.Empty;
                HasError = false;

                await _apiClient.DeleteUserAsync(Id);
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsDeleted = false;
            }
        }
    }
}
