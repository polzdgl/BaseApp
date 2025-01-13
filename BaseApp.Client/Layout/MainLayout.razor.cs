using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace BaseApp.Client.Layout
{
    public partial class MainLayout
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        private bool sidebarExpanded = true;

        void SidebarToggleClick()
        {
            sidebarExpanded = !sidebarExpanded;
        }

        private class DropdownItem
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }

        private string UserName { get; set; }

        private List<DropdownItem> dropdownItems = new()
        {
            new DropdownItem { Text = "Profile", Value = "profile" },
            new DropdownItem { Text = "Settings", Value = "settings" },
            new DropdownItem { Text = "Logout", Value = "logout" }
        };

        private void OnDropdownItemClick(RadzenSplitButtonItem args)
        {
            // Handle null args or null Value
            if (args?.Value == null)
            {
                // Default action for null value is Profile
                NavigationManager.NavigateTo("/user/profile");
                return;
            }

            switch (args.Value?.ToString())
            {
                case "settings":
                    NavigationManager.NavigateTo("/user/settings");
                    break;
                case "profile":
                    NavigationManager.NavigateTo("/user/profile");
                    break;
                case "logout":
                    NavigationManager.NavigateTo("/auth/logout");
                    break;
                default:
                    NavigationManager.NavigateTo("/user/profile");
                    break;
            }
        }
        private void NavigateToLogin()
        {
            NavigationManager.NavigateTo("/auth/login");
        }
    }
}
