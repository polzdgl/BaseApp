using BaseApp.Data.User.Dtos;
using BaseApp.Web.ServiceClients;
using Microsoft.AspNetCore.Components;

namespace BaseApp.Web.Pages.User
{
    public partial class Edit
    {
        [Parameter]
        public int Id { get; set; } // User ID from the URL
        [Inject] private ApiClient ApiClient { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        private UserDto? User { get; set; } = new(); // Initialize with an empty object
        private bool IsLoading { get; set; } = true;
        private bool IsSaving { get; set; } = false;
        private bool HasError { get; set; } = false;
        private string? ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (Id > 0)
            {
                await LoadUserAsync();
            }
            else
            {
                IsLoading = false;
            }
        }

        private async Task LoadUserAsync()
        {
            try
            {
                ResetErrorState();
                User = await ApiClient.GetUserAsync(Id);
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

        private async Task SaveUserAsync()
        {
            IsSaving = true;
            ResetErrorState();

            try
            {
                if (Id > 0) // Update user
                {
                    var userRequest = MapToRequestDto(User);
                    await ApiClient.UpdateUserAsync(Id, userRequest);
                }
                NavigationManager.NavigateTo("/users");
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
            finally
            {
                IsSaving = false;
            }
        }

        private static UserRequestDto MapToRequestDto(UserDto? user)
        {
            if (user == null) 
            { 
                throw new ArgumentNullException(nameof(user)); 
            }

            return new UserRequestDto
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                IsActive = user.IsActive
            };
        }

        private void ResetErrorState()
        {
            HasError = false;
            ErrorMessage = null;
        }

        private void HandleError(Exception ex)
        {
            HasError = true;
            ErrorMessage = ex.Message;
        }
    }
}
