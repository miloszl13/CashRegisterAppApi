using ApplicationLayer.Interfaces;
using ApplicationLayer.Model;
using Domain.ErrorMessages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services
{
    public class CurrencyExchangeService : ICurrencyExchangeService
    {
        public ActionResult<double> Calculate(string currency, int amount, string currency1)
        {
            //check currency
            currency = currency.ToLower();
            if (currency != "rsd" && currency != "eur" && currency != "usd")
            {
                var errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = CurrencyExchangeErrorMessages.invalid_currency,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return new BadRequestObjectResult(errorResponse);
            }
            //check currency1
            currency1 = currency1.ToLower();
            if (currency1 != "rsd" && currency1 != "eur" && currency1 != "usd")
            {
                var errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = CurrencyExchangeErrorMessages.invalid_currency,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return new BadRequestObjectResult(errorResponse);
            }
            //check amounts
            if (amount <= 0)
            {
                var errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = CurrencyExchangeErrorMessages.invalid_amount,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return new BadRequestObjectResult(errorResponse);
            }
            double result = 0;

            switch (currency, currency1)
            {
                case ("rsd", "eur"):
                    result = (amount * 0.0085);
                    break;
                case ("rsd", "usd"):
                    result = (amount * 0.0094);
                    break;
                case ("eur", "rsd"):
                    result = (amount * 117.62);
                    break;
                case ("eur", "usd"):
                    result = (amount * 1.11);
                    break;
                case ("usd", "eur"):
                    result = (amount * 0.90);
                    break;
                case ("usd", "rsd"):
                    result = (amount * 106.19);
                    break;
            }
            return result;
        }
    }
}
