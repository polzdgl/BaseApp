using BaseApp.Data.SecurityExchange.Dtos;
using BaseApp.ServiceProvider.Company.Interfaces;
using BaseApp.Shared.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BaseApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly ICompanyManager _companyManager;
        private readonly InputValidation _inputValidation;

        public CompanyController(ILogger<CompanyController> logger, ICompanyManager securityExchangeManager,
            InputValidation inputValidation)
        {
            _logger = logger;
            _companyManager = securityExchangeManager;
            _inputValidation = inputValidation;
        }

        [HttpPost("importMarketData", Name = "ImportMarketDataAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ImportMarketDataAsync()
        {
            try
            {
                _logger.LogInformation("Starting import of market data from SEC API...");

                // Call the manager to perform the import
                await _companyManager.ImportMarketDataAsync();

                _logger.LogInformation("Market data imported successfully.");
                return Ok(new { Message = "Market data imported successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while getting fundable Companies list!");
                return Problem(detail: ex.Message.ToString(), statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("isMarketDataLoaded", Name = "IsMarketDataLoadedAsync")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> IsMarketDataLoadedAsync()
        {
            try
            {
                _logger.LogInformation("Checking if Market Data is loaded..");

                bool isLoaded = await _companyManager.IsMarketDataLoadedAsync();

                return Ok(isLoaded);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while getting fundable Companies list!");
                return Problem(detail: ex.Message.ToString(), statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("import", Name = "ImportCompnanyDataAsync")]
        [ProducesResponseType(typeof(List<FundableCompanyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ImportCompaniesInfoAsync([FromQuery] IEnumerable<string> ciks)
        {
            try
            {
                _logger.LogInformation("Importing Company data for CIKs: {ciks}", string.Concat(',', ciks));

                await _companyManager.ImportCompnanyDataAsync(ciks);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while getting fundable Companies list!");
                return Problem(detail: ex.Message.ToString(), statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("companies", Name = "GetCompaniesAsync")]
        [ProducesResponseType(typeof(List<FundableCompanyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCompaniesAsync([FromQuery] string startsWith = null)
        {
            try
            {
                _logger.LogInformation("Getting Company data: {name}", startsWith.IsNullOrEmpty() ? "for All" : $"starts with: {startsWith}");

                List<FundableCompanyDto> companies = await _companyManager.GetCompanies(startsWith);
                return Ok(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while getting fundable Companies list!");
                return Problem(detail: ex.Message.ToString(), statusCode: StatusCodes.Status500InternalServerError);
            }
        }

    }
}