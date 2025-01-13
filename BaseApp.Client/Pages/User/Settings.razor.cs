using Microsoft.AspNetCore.Components;
using Radzen;

namespace BaseApp.Client.Pages.User
{
    public partial class Settings
    {
      
        [Inject] protected NotificationService NotificationService { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; } = default!;


        private List<Theme> FilteredThemes;

        // List of themes are allowed to be selected, and available/free from the Razden ThemeService
        private List<string> AllowedThemes = new List<string> 
        {
            "material", "material-dark", 
            "Standard", "standard-dark", 
            "default", "dark",
            "humanistic", "humanistic-dark",
            "software", "software-dark",
        };

        void ChangeTheme(string value)
        {
            ThemeService.SetTheme(value);
        }

        protected override void OnInitialized()
        {
            // Filter the themes by excluding specific items
            FilteredThemes = Themes.All
                .Where(theme => AllowedThemes.Contains(theme.Value))
                .ToList();

            ThemeService.ThemeChanged += OnThemeChanged;
        }

        public void Dispose()
        {
            ThemeService.ThemeChanged -= OnThemeChanged;
        }

        private void OnThemeChanged()
        {
            ShowNotification("Success", "Theme updated successfully!", NotificationSeverity.Success);
        }

        private void NavigateToLogin()
        {
            NavigationManager.NavigateTo("/auth/login");
        }

        private void ShowNotification(string summary, string detail, NotificationSeverity severity)
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = severity,
                Summary = summary,
                Detail = detail,
                Duration = 4000
            });
        }
    }
}