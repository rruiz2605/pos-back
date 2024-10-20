using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace POS.Application.Features.Clients
{
    public class Util
    {
        public static string StandarisizeCellphoneNumber(string cellphoneNumber)
        {
            return Regex.Replace(cellphoneNumber.Replace("+51", ""), @"[\s\+\(\)\-]+", "");
        }
    }
}
