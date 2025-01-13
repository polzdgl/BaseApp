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
        private bool IsLoading = true;
        private bool IsMarketDataLoaded = false;

        protected override async Task OnInitializedAsync()
        {
            // Check if Market Data is loaded
            await CheckMarketDataLoadStatus();

            if (IsMarketDataLoaded)
            {
                // If Market Data is loaded, get Fundable Companies
                await GetFundableCompaniesAsync(new LoadDataArgs { });
            }
        }

        private async Task CheckMarketDataLoadStatus()
        {
            try
            {
                IsLoading = true;
                IsMarketDataLoaded = await CompanyProvider.IsMarketDataLoaded();
            }
            catch (Exception ex)
            {
                ShowNotification("Error", "An error occurred while trying to check Market Data Load Status!", NotificationSeverity.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task GetFundableCompaniesAsync(LoadDataArgs args)
        {
            try
            {
                IsLoading = true;
                fundableCompanies = await CompanyProvider.GetCompaniesAsync();
            }
            catch (Exception ex)
            {
                ShowNotification("Error", "An error occurred while trying to retrieve Companies!", NotificationSeverity.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ImportMarketData()
        {
            try
            {
                IsLoading = true;

                await CompanyProvider.ImportMarketDataAsync();

                ShowNotification("Success", "Market data imported successfully.", NotificationSeverity.Success);

                // Update the market data load status
                await CheckMarketDataLoadStatus();

                if (IsMarketDataLoaded)
                {
                    // Load fundable companies if the market data is now loaded
                    await GetFundableCompaniesAsync(new LoadDataArgs { });
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Error", "An error occurred while importing Market Data!", NotificationSeverity.Error);
            }
            finally
            {
                IsLoading = false;
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
