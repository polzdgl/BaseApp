using BaseApp.Data.SecurityExchange.Dtos;
using BaseApp.ServiceProvider.SecurityExchange.Interfaces;
using BaseApp.Shared.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BaseApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecurityExchangeController : ControllerBase
    {
        private readonly ILogger<SecurityExchangeController> _logger;
        private readonly ISecurityExchangeManager _securityExchangeManager;
        private readonly InputValidation _inputValidation;

        public SecurityExchangeController(ILogger<SecurityExchangeController> logger, ISecurityExchangeManager securityExchangeManager,
            InputValidation inputValidation)
        {
            _logger = logger;
            _securityExchangeManager = securityExchangeManager;
            _inputValidation = inputValidation;
        }

        [HttpGet("import", Name = "ImportCompnanyDataAsync")]
        [ProducesResponseType(typeof(List<FundableCompanyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ImportCompaniesInfoAsync([FromQuery] IEnumerable<string> ciks)
        {
            try
            {
                _logger.LogInformation("Importing Company data for CIKs: {ciks}", string.Concat(',',ciks));

                await _securityExchangeManager.ImportCompnanyDataAsync(ciks);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while getting fundable Companies list!");
                return Problem(detail: ex.Message.ToString(), statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("fundablecompanies", Name = "GetFundableCompaniesAsync")]
        [ProducesResponseType(typeof(List<FundableCompanyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFundableCompaniesAsync([FromQuery] string startsWith = null)
        {
            try
            {
                _logger.LogInformation("Gettimg Company data for: {name}", startsWith.IsNullOrEmpty() ? "All" : startsWith);

                List<FundableCompanyDto> companies = await _securityExchangeManager.GetFunableCompanies(startsWith);
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