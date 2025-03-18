using BaseApp.Data.Company.Dtos;
using BaseApp.ServiceProvider.Company.Interfaces;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace BaseApp.Client.Pages.Company
{
    public partial class Company : ComponentBase
    {
        [Parameter] public string Id { get; set; } = string.Empty;

        [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
        [Inject] protected NotificationService NotificationService { get; set; } = default!;
        [Inject] private ICompanyClient CompanyClient { get; set; } = default!;

        private CompanyDetailsDto? CompanyDetails { get; set; }
        private IEnumerable<CompanyFinancialsDto>? CompanyFinancialDetails = new List<CompanyFinancialsDto>();

        private bool IsLoading = true;
        RadzenDataGrid<CompanyFinancialsDto> grid;

        protected override async Task OnInitializedAsync()
        {
            await GetCompanyDetails();
        }

        private async Task GetCompanyDetails()
        {
            if(string.IsNullOrEmpty(Id))
            {
                ShowNotification("Error", "Company ID cannot be null or empty.", NotificationSeverity.Error);
                return;
            }

            try
            {
                IsLoading = true;
                CompanyDetails = await CompanyClient.GetCompanyDetailsAsync(Id);
                CompanyFinancialDetails = CompanyDetails.CompanyFinancials;
            }
            catch (Exception ex)
            {
                ShowNotification("Error", ex.Message, NotificationSeverity.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        // Show a notification message
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
