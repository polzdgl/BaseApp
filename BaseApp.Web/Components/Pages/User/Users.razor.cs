using BaseApp.Web.Clients;
using Microsoft.AspNetCore.Components;

namespace BaseApp.Web.Components.Pages.User
{
    public partial class Users : ComponentBase
    {
        private NavigationManager _navigationManager;
        private readonly UserClient _userClient;

        private IEnumerable<Data.User.Dtos.UserDto> users;

        public Users(NavigationManager navigationManager, UserClient userClient)
        {
            _navigationManager = navigationManager;
            _userClient = userClient;
        }

        bool IsLoading = true;
        bool HasError = false;
        private string? ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                users = await _userClient.GetUsersAsync();
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