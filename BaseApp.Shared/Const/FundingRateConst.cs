namespace BaseApp.Shared.Const
{
    public static class FundingRateConst
    {
        public const decimal TenBillionThreshold = 10_000_000_000m;
        public const decimal LargeIncomeMultiplier = 0.1233m;
        public const decimal SmallerIncomeMultiplier = 0.2151m;
        public const decimal CompanyNameWithVowelMultiplier = 0.15m;
        public const decimal CompanyNameWithDecreasingIncomeMultiplier = -0.25m;

    }
}
