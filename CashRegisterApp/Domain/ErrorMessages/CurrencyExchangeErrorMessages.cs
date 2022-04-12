using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ErrorMessages
{
    public class CurrencyExchangeErrorMessages
    {
        public const string invalidCurrency = "You did not enter the currency correctly!";
        public const string invalidAmount = "You cant exchange amount if it is less than or ecual zero!";
    }
}
