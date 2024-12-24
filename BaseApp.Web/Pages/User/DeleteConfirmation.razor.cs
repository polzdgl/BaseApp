using BaseApp.Web.ErrorHandling;
using BaseApp.Web.ServiceClients;
using Microsoft.AspNetCore.Components;

namespace BaseApp.Web.Pages.User
{
    public partial class DeleteConfirmation : ComponentBase
    {
        [Inject] private ApiClient ApiClient { get; set; } = default!;
        //[Inject] private NavigationManager NavigationManager { get; set; } = default!;

        [Parameter] public string Id { get; set; } = string.Empty;
        [Parameter] public string UserName { get; set; } = string.Empty;
        [Parameter] public EventCallback OnDeleteConfirmed { get; set; }
        [Parameter] public EventCallback OnCancel { get; set; }

        private bool IsOpen { get; set; }
        private bool IsDeleted { get; set; }
        private bool HasError = false;
        private string? ErrorMessage = string.Empty;

        public void OpenPopup(string id, string userName)
        {
            Id = id;
            UserName = userName;
            IsOpen = true;
        }

        private async Task DeleteUser()
        {
            try
            {
                ResetErrorState();
                IsDeleted = true;

                await ApiClient.DeleteUserAsync(Id);

                //NavigationManager.NavigateTo("users");

                IsDeleted = false;
                IsOpen = false;
                await OnDeleteConfirmed.InvokeAsync();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private async Task ClosePopup()
        {
            IsOpen = false;
            await OnCancel.InvokeAsync();
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
