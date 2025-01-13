using BaseApp.Data.SecurityExchange.Dtos;
using BaseApp.ServiceProvider.Company.Interfaces;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BaseApp.Client.Pages.Company
{
    public partial class Companies
    {
        [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
        [Inject] protected NotificationService NotificationService { get; set; } = default!;
        [Inject] private ICompanyProvider CompanyProvider { get; set; } = default!;

        private IEnumerable<FundableCompanyDto>? fundableCompanies = new List<FundableCompanyDto>();

        protected override async Task OnInitializedAsync()
        {
            await GetFundableCompaniesAsync(new LoadDataArgs { });
        }

        private async Task GetFundableCompaniesAsync(LoadDataArgs args)
        {
            try
            {
                fundableCompanies = await CompanyProvider.GetCompaniesAsync();
            }
            catch (Exception ex)
            {
                ShowNotification("Error", "An error occurred while trying to retrieve Companies!", NotificationSeverity.Error);
            }
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
