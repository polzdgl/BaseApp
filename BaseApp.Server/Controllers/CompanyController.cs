using BaseApp.Data.Company.Dtos;
using BaseApp.Data.Company.Models;
using BaseApp.Data.User.Dtos;
using BaseApp.ServiceProvider.Company.Interfaces;
using BaseApp.Shared.Validations;
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

        public CompanyController(ILogger<CompanyController> logger, ICompanyManager securityExchangeManager, InputValidation inputValidation)
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
                CikImportResult result = await _companyManager.ImportMarketDataAsync();

                if (result.FailedCiks.Any())
                {
                    _logger.LogInformation("Market data imported partially.");
                    return StatusCode(StatusCodes.Status207MultiStatus, result);
                }

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status207MultiStatus)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ImportCompaniesInfoAsync([FromQuery] IEnumerable<string> ciks)
        {
            try
            {
                _logger.LogInformation("Importing Company data for CIKs: {ciks}", string.Concat(',', ciks));

                var result = await _companyManager.ImportCompnanyDataAsync(ciks);

                if (result.FailedCiks.Any())
                {
                    _logger.LogInformation("CIKs data imported partially.");
                    return StatusCode(StatusCodes.Status207MultiStatus, result);
                }

                _logger.LogInformation("CIKs data imported successfully.");
                return Ok(new { Message = "CIKs data imported successfully." });
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

                List<FundableCompanyDto> companies = await _companyManager.GetCompaniesAsync(startsWith);
                return Ok(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while getting fundable Companies list!");
                return Problem(detail: ex.Message.ToString(), statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}", Name = "GetCompanyByIdAsync")]
        [ProducesResponseType(typeof(CompanyFinancialsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCompanyByIdAsync(string id)
        {
            try
            {
                if (!_inputValidation.ValidateCik(id, out var cikValidator))
                {
                    _logger.LogWarning("Invalid Id: {id}. Errors: {erorrMessage}", id, cikValidator);
                    return Problem(detail: $"Invalid Id: {id}. Errors: {cikValidator}", statusCode: StatusCodes.Status400BadRequest);
                }

                _logger.LogInformation("Getting UserId:{id}", id);

                //UserDto user = await _companyManager.GetCompanyDetailsAsync(id);

                // Mock data
                CompanyDetailsDto companyDetails = new CompanyDetailsDto
                {
                    Name = "Apple Inc.",
                    Ticker = "AAPL",
                    Cik = 320193,
                    Industry = "Computer Hardware",
                    Sector = "Technology",
                    Description = "Apple Inc. designs, manufactures, and markets smartphones, personal computers, tablets, wearables, and accessories worldwide.",
                    Website = "http://www.apple.com",
                    Exchange = "NASDAQ",
                    Currency = "USD",
                    LastUpdated = DateTime.Now,

                    CompanyFinancials = new List<CompanyFinancialsDto>
                    {
                        new CompanyFinancialsDto
                        {
                            InfoFactUsGaapIncomeLossUnitsUsdId = 1,
                            StartDate =  new DateOnly(2020, 1, 1),
                            EndDate = new DateOnly(2021, 1, 1),
                            Value = 274515000000,
                            FiscalPeriod = "FY",
                            FiscalYear = 2021,
                            FiledAt =  new DateOnly(2020, 1, 1),
                            Form = "10-K",
                            Frame = "CY2021"
                        },
                        new CompanyFinancialsDto
                        {
                            InfoFactUsGaapIncomeLossUnitsUsdId = 2,
                            StartDate =  new DateOnly(2019, 1, 1),
                            EndDate = new DateOnly(2020, 1, 1),
                            Value = 260174000000,
                            FiscalPeriod = "FY",
                            FiscalYear = 2020,
                            FiledAt =  new DateOnly(2019, 1, 1),
                            Form = "10-K",
                            Frame = "CY2020"
                        },
                        new CompanyFinancialsDto
                        {
                            InfoFactUsGaapIncomeLossUnitsUsdId = 3,
                            StartDate =  new DateOnly(2018, 1, 1),
                            EndDate = new DateOnly(2019, 1, 1),
                            Value = 265595000000,
                            FiscalPeriod = "FY",
                            FiscalYear = 2019,
                            FiledAt =  new DateOnly(2018, 1, 1),
                            Form = "10-K",
                            Frame = "CY2019"
                        }
                    }
                };

                return companyDetails is not null ? Ok(companyDetails) :
                    Problem(detail: $"Company ID: {id} was not found!", statusCode: StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while getting UserId: {id}");
                return Problem(detail: ex.Message.ToString(), statusCode: StatusCodes.Status500InternalServerError);
            }
        }

    }
}