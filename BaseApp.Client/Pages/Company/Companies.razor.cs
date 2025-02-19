using BaseApp.Data.Company.Dtos;
using BaseApp.ServiceProvider.Company.Interfaces;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace BaseApp.Client.Pages.Company
{
    public partial class Companies
    {
        [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
        [Inject] protected NotificationService NotificationService { get; set; } = default!;
        [Inject] private ICompanyClient CompanyClient { get; set; } = default!;

        private bool IsLoading = true;
        private bool IsMarketDataLoaded = false;
        private int TotalRecords = 0;
        private int PageSize = 50;
        RadzenDataGrid<FundableCompanyDto> grid;

        private IEnumerable<FundableCompanyDto>? fundableCompanies = new List<FundableCompanyDto>();

        protected override async Task OnInitializedAsync()
        {
            // Check if Market Data is loaded
            //await CheckMarketDataLoadStatus();

            //if (IsMarketDataLoaded)
            //{
            //    // If Market Data is loaded, data is available, get Company lists
            LoadDataArgs loadDataArgs = new LoadDataArgs
            {
                Skip = 0,
                Top = PageSize
            };

            await GetFundableCompaniesAsync(loadDataArgs);
            //}
        }

        // Check if Data is Imported from the SEC EgdarConpany API, and update the status for IsMarketDataLoaded
        private async Task CheckMarketDataLoadStatus()
        {
            try
            {
                IsLoading = true;
                IsMarketDataLoaded = await CompanyClient.IsMarketDataLoaded();
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

        // Get Companies from the Backend API/Database
        // LoadDataArgs is used to pass filters and other parameters to the API
        // Filtering is handled by the BackendApi 
        private async Task GetFundableCompaniesAsync(LoadDataArgs args = null)
        {
            try
            {
                IsLoading = true;

                // Calculate page index based on Radzen's `Skip` and `Top` properties
                var pageIndex = args.Skip / args.Top + 1;
                var pageSize = args.Top;

                // Extract the filter value for "Name"
                string nameFilter = args?.Filters?
                    .FirstOrDefault(f => f.Property == nameof(FundableCompanyDto.Name))?
                    .FilterValue?.ToString();

                // Call the API provider with the filter value
                var result = await CompanyClient.GetCompaniesAsync((int)pageIndex, (int)pageSize.Value, nameFilter);

                if (result != null)
                {
                    fundableCompanies = result.Items ?? new List<FundableCompanyDto>();
                    TotalRecords = result.TotalCount;
                }
                else
                {
                    fundableCompanies = new List<FundableCompanyDto>();
                    TotalRecords = 0;
                }

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

        // Import Companies and their Market Data from the SEC EdgarCompany API and persist in the database
        private async Task ImportMarketData()
        {
            try
            {
                IsLoading = true;

                await CompanyClient.ImportMarketDataAsync();

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


        // Clear all filters and reload all data.
        private async Task ClearFilters()
        {
            try
            {
                // Reset filters and reload all data
                grid.Reset(true);
                await grid.Reload();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing filters: {ex.Message}");
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
