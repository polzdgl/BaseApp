using BaseApp.Data.Company.Models;
using BaseApp.Data.Repositories.Interfaces;
using BaseApp.ServiceProvider.Company.Interfaces;
using BaseApp.ServiceProvider.Company.Manager;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace BaseApp.Tests.ServiceProvider.Company
{
    public class CompanyManagerTests
    {
        private readonly IRepositoryFactory _mockRepositoryFactory;
        private readonly ISecurityExchangeProvider _mockSecurityExchangeProvider;
        private readonly ILogger<CompanyManager> _mockLogger;
        private readonly ICompanyManager _mockCompanyManager;

        public CompanyManagerTests()
        {
            _mockRepositoryFactory = Substitute.For<IRepositoryFactory>();
            _mockSecurityExchangeProvider = Substitute.For<ISecurityExchangeProvider>();
            _mockLogger = Substitute.For<ILogger<CompanyManager>>();

            // Pass these mocks into CompanyManager constructor
            _mockCompanyManager = new CompanyManager(_mockRepositoryFactory, _mockSecurityExchangeProvider, _mockLogger);
        }

        #region CalculateStandardFundableAmount Tests

        [Fact]
        public void CalculateStandardFundableAmount_ReturnsZero_IfMissingAnyYear()
        {
            // Arrange
            // We only include 4 distinct years (missing 2022, for example)
            var incomeData = new List<InfoFactUsGaapIncomeLossUnitsUsd>
            {
                MakeUsd("CY2018", 100000),
                MakeUsd("CY2019", 200000),
                MakeUsd("CY2020", 300000),
                MakeUsd("CY2021", 400000)
            };

            // Act
            var result = _mockCompanyManager.CalculateStandardFundableAmount(incomeData);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void CalculateStandardFundableAmount_ReturnsZero_IfNoPositiveIncomeFor2021Or2022()
        {
            // Arrange
            // 2021 is not positive, or 2022 is not positive
            var incomeData = new List<InfoFactUsGaapIncomeLossUnitsUsd>
            {
                MakeUsd("CY2018", 100000),
                MakeUsd("CY2019", 200000),
                MakeUsd("CY2020", 300000),
                MakeUsd("CY2021", -1),      // Negative
                MakeUsd("CY2022", 400000),
            };

            // Act
            var result = _mockCompanyManager.CalculateStandardFundableAmount(incomeData);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void CalculateStandardFundableAmount_Uses2151Percent_IfHighestUnder10B()
        {
            // Arrange
            // All 5 years, 2021 & 2022 are positive, highest is 9B (under 10B)
            var incomeData = new List<InfoFactUsGaapIncomeLossUnitsUsd>
            {
                MakeUsd("CY2018", 500_000_000),
                MakeUsd("CY2019", 2_000_000_000),
                MakeUsd("CY2020", 4_000_000_000),
                MakeUsd("CY2021", 6_000_000_000),
                MakeUsd("CY2022", 9_000_000_000),
            };

            // highest = 9,000,000,000 => 9B * 0.2151
            decimal expected = 9_000_000_000m * 0.2151m;

            // Act
            var result = _mockCompanyManager.CalculateStandardFundableAmount(incomeData);

            // Assert
            Assert.Equal(expected, result, precision: 4);
        }

        [Fact]
        public void CalculateStandardFundableAmount_Uses1233Percent_IfHighestAtLeast10B()
        {
            // Arrange
            var incomeData = new List<InfoFactUsGaapIncomeLossUnitsUsd>
            {
                MakeUsd("CY2018", 5_000_000_000),
                MakeUsd("CY2019", 10_000_000_000), // exactly 10B
                MakeUsd("CY2020", 10_500_000_000),
                MakeUsd("CY2021", 10_000_000_001), // 2021 must be positive
                MakeUsd("CY2022", 11_000_000_000), // 2022 must be positive
            };

            // Highest is 11_000_000_000 => * 0.1233
            decimal expected = 11_000_000_000m * 0.1233m;

            // Act
            var result = _mockCompanyManager.CalculateStandardFundableAmount(incomeData);

            // Assert
            Assert.Equal(expected, result, precision: 4);
        }

        [Fact]
        public void CalculateStandardFundableAmount_ReturnsZero_IfException()
        {
            // Arrange
            // Let's force an exception: e.g., one entry has a null Frame
            var incomeData = new List<InfoFactUsGaapIncomeLossUnitsUsd>
            {
                MakeUsd(null, 100000),
                MakeUsd("CY2019", 200000),
                MakeUsd("CY2020", 300000),
                MakeUsd("CY2021", 400000),
                MakeUsd("CY2022", 500000),
            };

            // Act
            var result = _mockCompanyManager.CalculateStandardFundableAmount(incomeData);

            // Assert
            // The code catches exceptions and returns 0
            Assert.Equal(0, result);
        }

        #endregion

        #region CalculateSpecialFundableAmount Tests

        [Fact]
        public void CalculateSpecialFundableAmount_NoConditionsNoChanges()
        {
            // Arrange
            decimal standardAmount = 1000;
            string name = "Zeta Corp";
            decimal income2021 = 10_000;
            decimal income2022 = 10_000; // Not less than 2021

            // Act
            var result = _mockCompanyManager.CalculateSpecialFundableAmount(standardAmount, name, income2021, income2022);

            // Assert
            // No vowel start, no 2022 < 2021 => no modifications
            Assert.Equal(standardAmount, result);
        }

        [Fact]
        public void CalculateSpecialFundableAmount_Add15Percent_IfNameStartsWithVowel()
        {
            // Arrange
            decimal standardAmount = 1000;
            string name = "Alpha Inc.";
            decimal income2021 = 10_000;
            decimal income2022 = 15_000;

            // 15% of 1000 => 150
            decimal expected = 1150;

            // Act
            var result = _mockCompanyManager.CalculateSpecialFundableAmount(standardAmount, name, income2021, income2022);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CalculateSpecialFundableAmount_Sub25Percent_If2022IsLessThan2021()
        {
            // Arrange
            decimal standardAmount = 1000;
            string name = "Beta LLC";
            decimal income2021 = 12_000;
            decimal income2022 = 11_999; // less => subtract 25%

            decimal expected = 750; // 1000 - 250 = 750

            // Act
            var result = _mockCompanyManager.CalculateSpecialFundableAmount(standardAmount, name, income2021, income2022);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CalculateSpecialFundableAmount_BothConditions()
        {
            // Arrange
            decimal standardAmount = 1000;
            string name = "Apple Inc."; // starts w/ vowel => +15% = +150 => 1150
            decimal income2021 = 10_000;
            decimal income2022 = 5_000; // less => -25% of original standard = -250 => 1150 - 250 => 900

            // So final = 900
            decimal expected = 900;

            // Act
            var result = _mockCompanyManager.CalculateSpecialFundableAmount(standardAmount, name, income2021, income2022);

            // Assert
            Assert.Equal(expected, result);
        }

        #endregion

        #region Helper

        /// <summary>
        /// Simple helper to create an InfoFactUsGaapIncomeLossUnitsUsd instance
        /// with a specified 'Frame' (e.g. "CY2018") and value (Val).
        /// </summary>
        private InfoFactUsGaapIncomeLossUnitsUsd MakeUsd(string frame, decimal val)
        {
            return new InfoFactUsGaapIncomeLossUnitsUsd
            {
                Frame = frame,
                Val = val,
                Form = "10-K" // Typically needed for your logic, if relevant
            };
        }

        #endregion

    }
}
