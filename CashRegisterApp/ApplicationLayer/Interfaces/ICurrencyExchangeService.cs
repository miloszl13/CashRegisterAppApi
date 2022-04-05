using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Interfaces
{
    public interface ICurrencyExchangeService
    {
        ActionResult<double> Calculate(string currency, int amount, string currency1);
    }
}
