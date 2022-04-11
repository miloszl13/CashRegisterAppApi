using ApplicationLayer.Interfaces;
using ApplicationLayer.Model;
using CashRegisterApp.Controllers;
using Domain.ErrorMessages;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterApp.UnitTests.ControllerTests
{
    public class CurrencyExchangeControllerTests
    {
        private CurrencyExchangeController _controller;
        private Mock<ICurrencyExchangeService> _service;
        //Errors
        private ErrorResponseModel InvalidCurrency;
        private ErrorResponseModel InvalidAmount;
        [SetUp] 
        public void SetUp()
        {
            _service = new Mock<ICurrencyExchangeService>();
            _controller = new CurrencyExchangeController(_service.Object);
            //Error messages
            InvalidCurrency = new ErrorResponseModel()
            {
                ErrorMessage = CurrencyExchangeErrorMessages.invalid_currency,
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };
            InvalidAmount = new ErrorResponseModel()
            {
                ErrorMessage = CurrencyExchangeErrorMessages.invalid_amount,
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };
        }
        //Calculate tests
        [Test]
        public void Exchange_OneOfCurrenciesIsInvalid_ReturnBadRequestObjectResult()
        {
            //arrange
            _service.Setup(service => service.Calculate(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>())).Returns(new BadRequestObjectResult(InvalidCurrency));
            //act
            var result = _controller.Exchange("rsd",1,"usd");
            //assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }
        [Test]
        public void Exchange_ValidCurrenciesButAmountIsLessThanOrEqualToZero_ReturnBadRequestObjectResult()
        {
            //arrange
            _service.Setup(service => service.Calculate(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>())).Returns(new BadRequestObjectResult(InvalidAmount));
            //act
            var result = _controller.Exchange("rsd", 0, "usd");
            //assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }
        [Test]
        public void Exchange_ValidRsdToEur_ReturnResult()
        {
            //arrange
            _service.Setup(service => service.Calculate(It.IsAny<string>(), 1200, It.IsAny<string>())).Returns(1200 * 0.0085);
            var expected = 1200 * 0.0085;
            //act
            var result = _controller.Exchange("rsd", 1200, "eur");
            //assert
            result.Value.Should().Be(expected);
        }
        [Test]
        public void Exchange_ValidRsdToUsd_ReturnResult()
        {
            //arrange
            _service.Setup(service => service.Calculate(It.IsAny<string>(), 1200, It.IsAny<string>())).Returns(1200 * 0.0094);
            var expected = 1200 * 0.0094;
            //act
            var result = _controller.Exchange("rsd", 1200, "usd");
            //assert
            result.Value.Should().Be(expected);
        }
        [Test]
        public void Exchange_ValidEurToRsd_ReturnResult()
        {
            //arrange
            _service.Setup(service => service.Calculate(It.IsAny<string>(), 1200, It.IsAny<string>())).Returns(1200 * 117.62);
            var expected = 1200 * 117.62;
            //act
            var result = _controller.Exchange("eur", 1200, "rsd");
            //assert
            result.Value.Should().Be(expected);
        }
        [Test]
        public void Exchange_ValidEurToUsd_ReturnResult()
        {
            //arrange
            _service.Setup(service => service.Calculate(It.IsAny<string>(), 1200, It.IsAny<string>())).Returns(1200 * 1.11);
            var expected = 1200 * 1.11;
            //act
            var result = _controller.Exchange("eur", 1200, "usd");
            //assert
            result.Value.Should().Be(expected);
        }
        [Test]
        public void Exchange_ValidUsdToEur_ReturnResult()
        {
            //arrange
            _service.Setup(service => service.Calculate(It.IsAny<string>(), 1200, It.IsAny<string>())).Returns(1200 * 0.90);
            var expected = 1200 * 0.90;
            //act
            var result = _controller.Exchange("usd", 1200, "eur");
            //assert
            result.Value.Should().Be(expected);
        }
        [Test]
        public void Exchange_ValidUsdToRsd_ReturnResult()
        {
            //arrange
            _service.Setup(service => service.Calculate(It.IsAny<string>(), 1200, It.IsAny<string>())).Returns(1200 * 106.19);
            var expected = 1200 * 106.19;
            //act
            var result = _controller.Exchange("usd", 1200, "rsd");
            //assert
            result.Value.Should().Be(expected);
        }
    }
}
