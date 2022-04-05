using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ErrorMessages
{
    public class CurrencyExchangeErrorMessages
    {
        public const string invalid_currency = "You did not enter the currency correctly!";
        public const string invalid_amount = "You cant exchange amount if it is less than or ecual zero!";
    }
}
