using BaseApp.Data.User.Dtos;
using BaseApp.Web.ErrorHandling;
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

        private UserDto? User { get; set; }
        private bool IsLoading = true;
        private bool IsSaving = false;
        private bool HasError = false;
        private string? ErrorMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await LoadUserAsync();
        }

        private async Task LoadUserAsync()
        {
            if (Id <= 0)
            {
                IsLoading = false;
                return;
            }

            try
            {
                ResetErrorState();
                IsLoading = true;
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
                    var response = await ApiClient.UpdateUserAsync(Id, userRequest);
                    NavigationManager.NavigateTo("/users");
                }
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
            ErrorHandler.ResetErrorState(ref HasError, ref ErrorMessage);
        }

        private void HandleError(Exception ex)
        {
            ErrorHandler.HandleError(ex, ref HasError, ref ErrorMessage);
        }
    }
}
