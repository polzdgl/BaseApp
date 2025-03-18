using BaseApp.Shared.Enums.Compnay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.Shared.Validations
{
    public static class CikFormatter
    {
        public static string ToPaddedCik(this string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            return input.Trim().PadLeft((int)CikPaddingEnum.PaddingNumber, (char)CikPaddingEnum.PaddingValue);
        }

        public static string ToPaddedCik(this int input)
        {
            return input.ToString().Trim().PadLeft((int)CikPaddingEnum.PaddingNumber, (char)CikPaddingEnum.PaddingValue);
        }
    }
}
