using System.Text.RegularExpressions;

namespace POS.Application.Features.Clients
{
    public class Util
    {
        public static string StandarisizeCellphoneNumber(string cellphoneNumber)
        {
            if (string.IsNullOrWhiteSpace(cellphoneNumber))
            {
                return string.Empty;
            }

            return Regex.Replace(cellphoneNumber.Replace("+51", ""), @"[\s\+\(\)\-]+", "");
        }
    }
}
