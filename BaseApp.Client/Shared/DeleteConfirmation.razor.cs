using Microsoft.AspNetCore.Components;

namespace BaseApp.Client.Shared
{
    public partial class DeleteConfirmation : ComponentBase
    {
        [Parameter] public string Title { get; set; } = "Confirm Deletion";
        [Parameter] public string Message { get; set; } = "Are you sure you want to delete this item?";
        [Parameter] public EventCallback OnConfirm { get; set; }
        [Parameter] public EventCallback OnCancel { get; set; }

        private string PanelStyle => IsOpen
               ? "display: block; width: 100%; max-width: 500px; margin: auto; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%); z-index: 1000;"
               : "display: none;";

        private bool IsOpen { get; set; }
        private bool IsProcessing { get; set; }
        private bool HasError { get; set; }
        private string ErrorMessage { get; set; } = string.Empty;

        public void OpenPopup()
        {
            ResetState();
            IsOpen = true;
        }

        private async Task ConfirmDeletion()
        {
            IsProcessing = true;
            try
            {
                await OnConfirm.InvokeAsync(); // Notify parent component about the confirmation
                ClosePopup();
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsProcessing = false;
            }
        }

        private void ClosePopup()
        {
            IsOpen = false;
            OnCancel.InvokeAsync(); // Notify parent component about cancellation
        }

        private void ResetState()
        {
            HasError = false;
            ErrorMessage = string.Empty;
        }
    }
}
