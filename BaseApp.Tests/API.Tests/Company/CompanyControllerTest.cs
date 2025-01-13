using BaseApp.Data.SecurityExchange.Dtos;
using BaseApp.Data.SecurityExchange.Models;
using BaseApp.Server.Controllers;
using BaseApp.ServiceProvider.Company.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace BaseApp.Tests.API.Tests.Companies
{
    public class CompanyControllerTests
    {
        private readonly ICompanyManager _mockCompanyManager;
        private readonly ILogger<CompanyController> _mockLogger;
        private readonly CompanyController _mockController;

        public CompanyControllerTests()
        {
            _mockCompanyManager = Substitute.For<ICompanyManager>();
            _mockLogger = Substitute.For<ILogger<CompanyController>>();

            // Initialize the controller with the mocked dependencies
            _mockController = new CompanyController(_mockLogger, _mockCompanyManager);
        }

        [Fact]
        public async Task GetFundableCompaniesAsync_NoFilter_ReturnsList()
        {
            // Arrange
            var mockCompanies = new List<FundableCompanyDto>
            {
                new FundableCompanyDto { Id = 1, Name = "ABC, Inc.", StandardFundableAmount = 100, SpecialFundableAmount = 120 },
                new FundableCompanyDto { Id = 2, Name = "XYZ, LLC.", StandardFundableAmount = 200, SpecialFundableAmount = 230 }
            };

            _mockCompanyManager
                .GetCompaniesAsync(Arg.Any<string>())
                .Returns(mockCompanies);

            // Act
            var result = await _mockController.GetCompaniesAsync(null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCompanies = Assert.IsType<List<FundableCompanyDto>>(okResult.Value);

            Assert.Equal(2, returnedCompanies.Count);
            Assert.Equal("ABC, Inc.", returnedCompanies[0].Name);
        }

        [Fact]
        public async Task GetFundableCompaniesAsync_StartsWithFilter_ReturnsFiltered()
        {
            // Arrange
            var mockCompanies = new List<FundableCompanyDto>
            {
                new FundableCompanyDto { Id = 1, Name = "Alpha Corp." },
                new FundableCompanyDto { Id = 2, Name = "Beta Solutions" }
            };

            // Only "Alpha Corp." starts with 'A'
            _mockCompanyManager
                .GetCompaniesAsync("A")
                .Returns(new List<FundableCompanyDto> { mockCompanies[0] });

            // Act
            var result = await _mockController.GetCompaniesAsync("A");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCompanies = Assert.IsType<List<FundableCompanyDto>>(okResult.Value);

            Assert.Single(returnedCompanies);
            Assert.Equal("Alpha Corp.", returnedCompanies[0].Name);
        }

        [Fact]
        public async Task ImportMarketDataAsync_NoFailedCiks_ReturnsOk()
        {
            // Arrange
            var importResult = new CikImportResult
            {
                FailedCiks = new List<string>(),    // No failures
                SucceededCiks = new List<string> { "0000001234", "0000005678" }
            };

            // Mock the manager call
            _mockCompanyManager
                .ImportMarketDataAsync()
                .Returns(Task.FromResult(importResult));

            // Act
            var result = await _mockController.ImportMarketDataAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            // You could also check the returned message if needed
            Assert.Contains("Market data imported successfully", okResult.Value.ToString());
        }

        [Fact]
        public async Task ImportMarketDataAsync_SomeFailedCiks_Returns207MultiStatus()
        {
            // Arrange
            var importResult = new CikImportResult
            {
                FailedCiks = new List<string> { "0000012345" }, // At least one failure
                SucceededCiks = new List<string> { "0000005678" }
            };

            // Mock the manager call
            _mockCompanyManager
                .ImportMarketDataAsync()
                .Returns(Task.FromResult(importResult));

            // Act
            var result = await _mockController.ImportMarketDataAsync();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status207MultiStatus, objectResult.StatusCode);

            // The returned object should be the CikImportResult itself
            var returnedImportResult = Assert.IsType<CikImportResult>(objectResult.Value);
            Assert.Single(returnedImportResult.FailedCiks);
            Assert.Equal("0000012345", returnedImportResult.FailedCiks.First());
        }

        [Fact]
        public async Task ImportMarketDataAsync_ThrowsException_Returns500()
        {
            // Arrange
            _mockCompanyManager
                .When(x => x.ImportMarketDataAsync())
                .Do(_ => { throw new Exception("Test Exception"); });

            // Act
            var result = await _mockController.ImportMarketDataAsync();

            // Assert
            var problemResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, problemResult.StatusCode);

            // Cast to ProblemDetails
            var problemDetails = Assert.IsType<ProblemDetails>(problemResult.Value);

            // Now read Detail
            Assert.Equal("Test Exception", problemDetails.Detail);
        }

        [Fact]
        public async Task IsMarketDataLoadedAsync_ReturnsOk_WhenNoException()
        {
            // Arrange
            // We'll pretend that market data is loaded
            _mockCompanyManager
                .IsMarketDataLoadedAsync()
                .Returns(true);

            // Act
            var result = await _mockController.IsMarketDataLoadedAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            // Check the bool value
            var isLoaded = Assert.IsType<bool>(okResult.Value);
            Assert.True(isLoaded);
        }

        [Fact]
        public async Task IsMarketDataLoadedAsync_ReturnsFalse_WhenMarketDataNotLoaded()
        {
            // Arrange
            // We'll pretend that market data is NOT loaded
            _mockCompanyManager
                .IsMarketDataLoadedAsync()
                .Returns(false);

            // Act
            var result = await _mockController.IsMarketDataLoadedAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            // Check the bool value
            var isLoaded = Assert.IsType<bool>(okResult.Value);
            Assert.False(isLoaded);
        }

        [Fact]
        public async Task IsMarketDataLoadedAsync_Returns500_WhenExceptionIsThrown()
        {
            // Arrange
            _mockCompanyManager
                .When(x => x.IsMarketDataLoadedAsync())
                .Do(_ => { throw new Exception("Test Exception"); });

            // Act
            var result = await _mockController.IsMarketDataLoadedAsync();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

            // Cast to ProblemDetails to inspect the detail string
            var problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
            Assert.Equal("Test Exception", problemDetails.Detail);
        }

        [Fact]
        public async Task ImportCompaniesInfoAsync_AllCiksSucceed_ReturnsOk()
        {
            // Arrange
            var ciks = new List<string> { "12345", "67890" };

            var importResult = new CikImportResult
            {
                FailedCiks = new List<string>(),    // No failures
                SucceededCiks = ciks
            };

            _mockCompanyManager
                .ImportCompnanyDataAsync(ciks)
                .Returns(Task.FromResult(importResult));

            // Act
            var result = await _mockController.ImportCompaniesInfoAsync(ciks);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            Assert.Contains("imported successfully", okResult.Value?.ToString() ?? string.Empty);
        }

        [Fact]
        public async Task ImportCompaniesInfoAsync_SomeCiksFail_Returns207()
        {
            // Arrange
            var ciks = new List<string> { "12345", "67890" };

            // Force partial failure
            var importResult = new CikImportResult
            {
                FailedCiks = new List<string> { "67890" },
                SucceededCiks = new List<string> { "12345" }
            };

            _mockCompanyManager
                .ImportCompnanyDataAsync(ciks)
                .Returns(Task.FromResult(importResult));

            // Act
            var result = await _mockController.ImportCompaniesInfoAsync(ciks);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status207MultiStatus, objectResult.StatusCode);

            // Verify we got the CikImportResult with failed items
            var returnedResult = Assert.IsType<CikImportResult>(objectResult.Value);
            Assert.Single(returnedResult.FailedCiks);
            Assert.Equal("67890", returnedResult.FailedCiks.First());
        }

        [Fact]
        public async Task ImportCompaniesInfoAsync_ThrowsException_Returns500()
        {
            // Arrange
            var ciks = new List<string> { "12345", "67890" };

            _mockCompanyManager
                .When(x => x.ImportCompnanyDataAsync(ciks))
                .Do(_ => { throw new Exception("Test Exception"); });

            // Act
            var result = await _mockController.ImportCompaniesInfoAsync(ciks);

            // Assert
            var problemResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, problemResult.StatusCode);

            // Since it's a ProblemDetails object, we can inspect its Detail property
            var problemDetails = Assert.IsType<ProblemDetails>(problemResult.Value);
            Assert.Equal("Test Exception", problemDetails.Detail);
        }

        [Fact]
        public async Task GetCompaniesAsync_NoFilter_ReturnsAllCompanies()
        {
            // Arrange
            var companies = new List<FundableCompanyDto>
            {
                new FundableCompanyDto { Id = 1, Name = "Alpha, Inc.", StandardFundableAmount = 100, SpecialFundableAmount = 120 },
                new FundableCompanyDto { Id = 2, Name = "Beta, LLC", StandardFundableAmount = 200, SpecialFundableAmount = 230 }
            };

            // The manager should return all companies when 'startsWith' is null
            _mockCompanyManager
                .GetCompaniesAsync(null)
                .Returns(Task.FromResult(companies));

            // Act
            var result = await _mockController.GetCompaniesAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var returnedCompanies = Assert.IsType<List<FundableCompanyDto>>(okResult.Value);
            Assert.Equal(2, returnedCompanies.Count);
            Assert.Equal("Alpha, Inc.", returnedCompanies[0].Name);
            Assert.Equal("Beta, LLC", returnedCompanies[1].Name);
        }

        [Fact]
        public async Task GetCompaniesAsync_WithFilter_ReturnsFilteredCompanies()
        {
            // Arrange
            var startsWith = "A";
            var allCompanies = new List<FundableCompanyDto>
            {
                new FundableCompanyDto { Id = 1, Name = "Alpha, Inc.", StandardFundableAmount = 100, SpecialFundableAmount = 120 },
                new FundableCompanyDto { Id = 2, Name = "Beta, LLC", StandardFundableAmount = 200, SpecialFundableAmount = 230 }
            };

            // Suppose only "Alpha, Inc." matches the filter
            var filteredCompanies = allCompanies
                .Where(c => c.Name.StartsWith(startsWith, StringComparison.OrdinalIgnoreCase))
                .ToList();

            _mockCompanyManager
                .GetCompaniesAsync(startsWith)
                .Returns(Task.FromResult(filteredCompanies));

            // Act
            var result = await _mockController.GetCompaniesAsync(startsWith);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var returnedCompanies = Assert.IsType<List<FundableCompanyDto>>(okResult.Value);
            Assert.Single(returnedCompanies);
            Assert.Equal("Alpha, Inc.", returnedCompanies.First().Name);
        }

        [Fact]
        public async Task GetCompaniesAsync_ThrowsException_Returns500()
        {
            // Arrange
            var startsWith = "A";

            _mockCompanyManager
                .When(x => x.GetCompaniesAsync(startsWith))
                .Do(_ => { throw new Exception("Test Exception"); });

            // Act
            var result = await _mockController.GetCompaniesAsync(startsWith);

            // Assert
            var problemResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, problemResult.StatusCode);

            // Cast to ProblemDetails to inspect the detail string
            var problemDetails = Assert.IsType<ProblemDetails>(problemResult.Value);
            Assert.Equal("Test Exception", problemDetails.Detail);
        }
    }
}
