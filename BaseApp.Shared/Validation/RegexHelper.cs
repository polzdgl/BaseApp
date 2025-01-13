using System.Text.RegularExpressions;

namespace BaseApp.Shared.Validation
{
    // Helper class to validate strings using regular expressions
    public static class RegexHelper
    {
        public const string Alpha = @"^[a-zA-Z]+$";

        public const string AlphaNumeric = @"([a-zA-Z0-9_\s]+)";

        public const string AlphaAndSpecialChar = @"([a-zA-Z()/,:]+)";

        public const string NumericAndSigns = @"([0-9()/_,;.:!?\-\s]+)";

        public const string AlphanNumericAndSigns = @"([a-zA-Z0-9()/_,;.:!?\-\s]+)";

        public const string AlphanumericAndSpecialSigns = @"([a-zA-Z0-9(),.$#€'""\-\s]+)";

        public const string HexaColor = @"(#[a-zA-Z0-9]+)";

        public const string LdapSafeguard = @"^[0-9a-zA-ZÀ-ž\.\-\@]+$";

        public static bool IsOnlyLettersAndSpecialChars(string inputString)
        {
            return Regex.IsMatch(inputString, AlphaAndSpecialChar);
        }

        public static bool IsOnlyNumbersAndSigns(string inputString)
        {
            return Regex.IsMatch(inputString, NumericAndSigns);
        }

        public static bool IsOnlyLettersNumbersAndSigns(string inputString)
        {
            return Regex.IsMatch(inputString, AlphanNumericAndSigns);
        }

        public static bool IsOnlyLettersNumbers(string inputString)
        {
            return Regex.IsMatch(inputString, AlphaNumeric);
        }
    }
}
