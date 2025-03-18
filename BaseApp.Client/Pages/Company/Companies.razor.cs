using BaseApp.Client.ServiceClients.User;
using BaseApp.Data.Company.Dtos;
using BaseApp.ServiceProvider.Company.Interfaces;
using BaseApp.Shared.ErrorHandling;
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
        private int PageSize = 25;
        RadzenDataGrid<FundableCompanyDto> grid;

        private IEnumerable<FundableCompanyDto> fundableCompanies = new List<FundableCompanyDto>();


        protected override async Task OnInitializedAsync()
        {
            // Check if Market Data is loaded
            await CheckMarketDataLoadStatus();

            if (IsMarketDataLoaded)
            {
                // If Market Data is loaded, data is available, get Company lists
                await GetFundableCompaniesAsync();
            }
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

                // Extract the filter value for "Name"
                string nameFilter = args?.Filters?
                    .FirstOrDefault(f => f.Property == nameof(FundableCompanyDto.Name))?
                    .FilterValue?.ToString();

                // Call the API provider with the filter value
                fundableCompanies = await CompanyClient.GetCompaniesAsync(nameFilter);
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

        // This method is called when the user clicks on the edit button
        private void NavigateToDetails(int cik)
        {
            NavigationManager.NavigateTo($"/company/{cik}");
        }

        // This method is called when the user clicks on the delete button
        // This method shows a confirmation popup before deleting the user
        private void ShowUpdatePopup(int cik, string CompanyName)
        {
            //SelectedUserId = userId;
            //DeleteConfirmationPopup.Message = $"Are you sure you want to delete the user '{userName}'?";
            //DeleteConfirmationPopup.OpenPopup();
        }

        // This method is called when the user confirms the deletion
        private async Task UpdateCompany()
        {
            //try
            //{
            //    var response = await UserClient.DeleteUserAsync(SelectedUserId);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        ShowNotification("Success", "User deleted successfully.", NotificationSeverity.Success);
            //        await RefreshUserList();
            //    }
            //    else
            //    {
            //        var errorMessage = await ErrorHandler.ExtractErrorMessageAsync(response);
            //        ShowNotification("Error", errorMessage, NotificationSeverity.Error);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ShowNotification("Error", ex.Message, NotificationSeverity.Error);
            //}
        }

        // Show loading on the UI
        async Task ShowLoading()
        {
            IsLoading = true;

            await Task.Yield();

            IsLoading = false;
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
