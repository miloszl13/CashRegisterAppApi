using ApplicationLayer.Interfaces;
using ApplicationLayer.Model;
using ApplicationLayer.ViewModels;
using CashRegisterApp.Controllers;
using Domain;
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
    public class BillProductControllerTests
    {
        private BillProductController _controller;
        private Mock<IBillProductService> _service;
        //domain entities
        private BillProduct BillProduct;
        private List<BillProduct> BillProducts;
        //View models
        private BillProductViewModel BillProductVM;
        private List<BillProductViewModel> BillProductVMS;
        //ErrorResponseModels
        private ErrorResponseModel EmptyBillProductDb;
        private ErrorResponseModel ProductNotExist;
        private ErrorResponseModel BillProductNotExist;
        private ErrorResponseModel TooManyProducts;
        private ErrorResponseModel OverCostLimit;
        [SetUp]
        public void SetUp()
        {
            _service = new Mock<IBillProductService>();
            _controller = new BillProductController(_service.Object);
            //domain entities
            BillProduct = new BillProduct()
            {
                Bill_number = "200000000007540220",
                Product_id = 1,
                Product_quantity = 2,
                Products_cost = 100
            };
            BillProducts = new List<BillProduct>();
            BillProducts.Add(BillProduct);
            //View models
            BillProductVM=new  BillProductViewModel();
            BillProductVMS=new List<BillProductViewModel>();
            BillProductVMS.Add(BillProductVM);
            //ErrorResponseModels
            EmptyBillProductDb = new ErrorResponseModel()
            {
                ErrorMessage = BillProductErrorMessages.emptyBillProductDb,
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
            ProductNotExist = new ErrorResponseModel()
            {
                ErrorMessage = ProductErrorMessages.productDoesNotExist,
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
            OverCostLimit = new ErrorResponseModel()
            {
                ErrorMessage = BillErrorMessages.overcostLimit,
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
            BillProductNotExist = new ErrorResponseModel()
            {
                ErrorMessage = BillProductErrorMessages.billProductNotExist,
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
            TooManyProducts = new ErrorResponseModel()
            {
                ErrorMessage = BillProductErrorMessages.tooManyProducts,
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }
    //GetAllBillProduct tests
        [Test]
        public void GetAllBillProduct_IfBillProductsDbIsNotEmpty_ReturnsBillProductViewModels()
        {
            //arrange
            _service.Setup(service => service.GetAllBillProduct()).Returns(BillProductVMS);
            //act
            var result = _controller.GetAllBillProduct();
            //assert
            result.Value.Should().BeEquivalentTo(BillProductVMS);
        }
        [Test]
        public void GetAllBillProduct_IfBillProductsDbIsEmpty_ReturnsNotFoundObjectResult()
        {
            //arrange
            _service.Setup(service => service.GetAllBillProduct()).Returns(new NotFoundObjectResult(EmptyBillProductDb));
            //act
            var result = _controller.GetAllBillProduct();
            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        //AddProductToBillProduct tests
        [Test]
        public void AddProductToBillProduct_ValidBillViewModel_ReturnsTrue()
        {
            //arrange
            _service.Setup(service => service.AddProductToBillProduct(It.IsAny<BillProductViewModel>())).Returns(true);
            //act
            var result = _controller.AddProductToBillProduct(BillProductVM);
            //assert
            result.Value.Should().Be(true);
        }
        [Test]
        public void AddProductToBillProduct_IfProductWithThatIdDoesntExist_ReturnsNotFoundObjectResult()
        {
            //arrange
            _service.Setup(service => service.AddProductToBillProduct(It.IsAny<BillProductViewModel>())).Returns(new NotFoundObjectResult(ProductNotExist));
            //act
            var result = _controller.AddProductToBillProduct(BillProductVM);
            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
        [Test]
        public void AddProductToBillProduct_IfBillsTotalCostGoesOver20000_ReturnsBadRequestObjectResult()
        {
            //arrange
            _service.Setup(service => service.AddProductToBillProduct(It.IsAny<BillProductViewModel>())).Returns(new BadRequestObjectResult(OverCostLimit));
            //act
            var result = _controller.AddProductToBillProduct(BillProductVM);
            //assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        //DeleteBillProduct tests
        [Test]
        public void DeleteBillProduct_IfBillProductDoesnNotExist_ReturnsTrue()
        {
            //arrange
            _service.Setup(service => service.Delete(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new NotFoundObjectResult(BillProductNotExist));
            //act
            var result = _controller.DeleteBillProduct("1",1,1);
            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
        [Test]
        public void DeleteBillProduct_TryToDeleteMoreProductsFromBillProductThanTheyAreInDb_ReturnsBadRequestObjectResult()
        {
            //arrange
            _service.Setup(service => service.Delete(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new BadRequestObjectResult(TooManyProducts));
            //act
            var result = _controller.DeleteBillProduct("200000000007540220", 1, 5);
            //assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }
        [Test]
        public void DeleteBillProduct_PassingValidValues_ReturnsTrue()
        {
            //arrange
            _service.Setup(service => service.Delete(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            //act
            var result = _controller.DeleteBillProduct("200000000007540220", 1, 5);
            //assert
            result.Value.Should().Be(true);
        }
    }
}
