﻿using BaseApp.Data.SecurityExchange.Dtos;
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

        [HttpPost("importMarketData", Name = "ImportMarketDataAsync")]
        [ProducesResponseType(typeof(List<FundableCompanyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ImportMarketDataAsync()
        {
            try
            {
                _logger.LogInformation("Loading Company data from SEC API..");

                await _securityExchangeManager.ImportMarketDataAsync();

                return Ok();
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

                bool isLoaded = await _securityExchangeManager.IsMarketDataLoadedAsync();

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

                await _securityExchangeManager.ImportCompnanyDataAsync(ciks);
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

                List<FundableCompanyDto> companies = await _securityExchangeManager.GetCompanies(startsWith);
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