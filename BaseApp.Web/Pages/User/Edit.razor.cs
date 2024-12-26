using BaseApp.Data.User.Dtos;
using BaseApp.ServiceProvider.Interfaces;
using BaseApp.Shared.ErrorHandling;
using Microsoft.AspNetCore.Components;

namespace BaseApp.Web.Pages.User
{
    public partial class Edit : ComponentBase
    {
        [Parameter]
        public string Id { get; set; } // User ID from the URL

        [Inject] private IUserApiClient ApiClient { get; set; } = default!;
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
            if (string.IsNullOrEmpty(Id))
            {
                IsLoading = false;
                HandleError(new ArgumentException("User ID cannot be null or empty."));
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
            try
            {
                if (!string.IsNullOrEmpty(Id)) // Update user
                {
                    IsSaving = true;
                    ResetErrorState();

                    var userRequest = MapToRequestDto(User);
                    var response = await ApiClient.UpdateUserAsync(Id, userRequest);

                    if (response.IsSuccessStatusCode)
                    {
                        // Navigate to the users list on success
                        NavigationManager.NavigateTo("users");
                    }
                    else
                    {
                        HasError = true;
                        ErrorMessage = await ErrorHandler.ExtractErrorMessageAsync(response);
                    }
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
