using BaseApp.Data.User.Dtos;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BaseApp.Web.Pages.User
{
    public partial class Users : ComponentBase
    {
        //private NavigationManager _navigationManager;
        private readonly HttpClient _apiClient;

        public Users(HttpClient userClient)
        {
            //_navigationManager = navigationManager;
            _apiClient = userClient;
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

                //users = await _userClient.GetUsersAsync();
                users = await _apiClient.GetFromJsonAsync<IEnumerable<UserDto>>("https://localhost:7115/user");
            
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