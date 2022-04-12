using ApplicationLayer.Model;
using ApplicationLayer.Services;
using Domain.ErrorMessages;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;


namespace CashRegisterApp.UnitTests.ServicesTests
{
    public class CurrencyExchangeServiceTests
    {
        //variable for currency exchange object
        private CurrencyExchangeService currencyExchange;
        //variables for values
        string ValidCurrencyRsd;
        string ValidCurrencyrsd;
        string ValidCurrencyUsd;
        string ValidCurrencyusd;
        string ValidCurrencyEur;
        string ValidCurremcyeur;
        int negativeAmount;
        int positiveAmount;
        int zeroAmount;
        string InvalidCurrency1;
        string InvalidCurrency2;
        //variable for errorResponse model
        ErrorResponseModel errorCurrencyResponse;
        ErrorResponseModel errorAmountResponse;
        [SetUp]
        public void SetUp()
        {
            //currencyexchange
            currencyExchange = new CurrencyExchangeService();
            //values
            ValidCurrencyRsd = "Rsd";
            ValidCurrencyrsd = "rsd";
            ValidCurrencyUsd = "Usd";
            ValidCurrencyusd = "usd";
            ValidCurrencyEur = "Eur";
            ValidCurremcyeur = "eur";
            
            negativeAmount = -1000;
            positiveAmount = 1000;
            zeroAmount = 0;

            InvalidCurrency1 = "test";
            InvalidCurrency2 = "test1";
            //error
            errorAmountResponse = new ErrorResponseModel()
            {
                ErrorMessage = CurrencyExchangeErrorMessages.invalidAmount,
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };
            errorCurrencyResponse = new ErrorResponseModel()
            {
                ErrorMessage = CurrencyExchangeErrorMessages.invalidCurrency,
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };
        }
        [Test]
        public void Calculate_ValidRsdToEurUpperCase_ReturnsAmount()
        {
            //arrange
            ActionResult<double> expected = positiveAmount * 0.0085;
            //act
            var result = currencyExchange.Calculate(ValidCurrencyRsd, positiveAmount, ValidCurrencyEur);
            //assert
            result.Should().BeEquivalentTo(expected);
        }
        [Test]
        public void Calculate_ValidUsdToEurLowerCase_ReturnsAmount()
        {
            //arrange
            ActionResult<double> expected = positiveAmount * 0.90;
            //act
            var result = currencyExchange.Calculate(ValidCurrencyusd, positiveAmount, ValidCurremcyeur);
            //assert
            result.Should().BeEquivalentTo(expected);
        }
        [Test]
        public void Calculate_ExchangeWhenAmountIsZero_ReturnsBadRequestObjectResult()
        {
            //arrange
            ActionResult<double> expected=new BadRequestObjectResult(errorAmountResponse);
            //act
            var result = currencyExchange.Calculate(ValidCurrencyRsd, zeroAmount, ValidCurremcyeur);
            //assert
            result.Should().BeEquivalentTo(expected);
        }
        [Test]
        public void Calculate_ExchangeWhenAmountIsLessThanZero_ReturnsBadRequestObjectResult()
        {
            //arrange
            ActionResult<double> expected = new BadRequestObjectResult(errorAmountResponse);
            //act
            var result = currencyExchange.Calculate(ValidCurrencyRsd, negativeAmount, ValidCurremcyeur);
            //assert
            result.Should().BeEquivalentTo(expected);
        }
        [Test]
        public void Calculate_ExchangeInInvalidCurrencies_ReturnsBadRequestObjectResult()
        {
            //arrange
            ActionResult<double> expected = new BadRequestObjectResult(errorCurrencyResponse);
            //act
            var result = currencyExchange.Calculate(InvalidCurrency1, positiveAmount, InvalidCurrency2);
            //assert
            result.Should().BeEquivalentTo(expected);
        }
        [Test]
        public void Calculate_ValidCurrency1AndInvalidCurrency2_ReturnsBadRequestObject()
        {
            //arrange
            ActionResult<double> expected = new BadRequestObjectResult(errorCurrencyResponse);
            //act
            var result = currencyExchange.Calculate(ValidCurrencyEur, positiveAmount, InvalidCurrency2);
            //assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}