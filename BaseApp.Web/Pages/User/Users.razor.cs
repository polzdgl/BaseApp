using BaseApp.Data.User.Dtos;
using BaseApp.Web.ServiceClients;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BaseApp.Web.Pages.User
{
    public partial class Users : ComponentBase
    {
        private NavigationManager _navigationManager;
        private readonly ApiClient _apiClient;

        public Users(NavigationManager navigationManager, ApiClient apiClient)
        {
            _navigationManager = navigationManager;
            _apiClient = apiClient;
        }

        private IEnumerable<Data.User.Dtos.UserDto> users;

        bool IsLoading = true;
        bool HasError = false;
        private string? ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                users = await _apiClient.GetUsersAsync();
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}