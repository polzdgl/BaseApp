﻿using BaseApp.Data.SecurityExchange.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.ServiceProvider.Company.Interfaces
{
    public interface ICompanyProvider
    {
        Task<IEnumerable<FundableCompanyDto>> GetCompaniesAsync(string? nameFilter = null, CancellationToken cancellationToken = default);
        Task ImportMarketDataAsync(CancellationToken cancellationToken = default);
        Task<bool> IsMarketDataLoaded(CancellationToken cancellationToken = default);
    }
}
