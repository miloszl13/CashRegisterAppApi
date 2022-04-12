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
    public class ProductControllerTests
    {
        private ProductController _controller;
        private Mock<IProductService> _service;
        //domain entities
        private Product Product;
        private List<Product> Products;
        //View models
        private ProductViewModel ProductVM;
        private List<ProductViewModel> ProductsVMS;
        //ErrorResponseModels
        private ErrorResponseModel EmptyProductDb;
        private ErrorResponseModel ProductNotExist;
        private ErrorResponseModel ProductAlreadyExist;
        [SetUp]
        public void SetUp()
        {
            _service = new Mock<IProductService>();
            _controller = new ProductController(_service.Object);
            //entities
            Product = new Product()
            {
                Product_Id = 1,
                Name = "test",
                Cost = 100,
                Bill_Products = new List<BillProduct>()
            };
            Products = new List<Product>();
            Products.Add(Product);
            //View models
            ProductVM = new ProductViewModel()
            {
                Product_id = 1,
                Name = "test",
                Cost = 100
            };
            ProductsVMS = new List<ProductViewModel>();
            ProductsVMS.Add(ProductVM);
            //ErrorResponseModels
            EmptyProductDb = new ErrorResponseModel()
            {
                ErrorMessage = ProductErrorMessages.emptyProductDb,
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
            ProductNotExist = new ErrorResponseModel()
            {
                ErrorMessage = ProductErrorMessages.productDoesNotExist,
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
            ProductAlreadyExist = new ErrorResponseModel()
            {
                ErrorMessage = ProductErrorMessages.productAlreadyExist,
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };
        }
        //Tests for GetProducts method
        [Test]
        public void GetProducts_IfProductDbIsNotEmpty_ReturnsProductViewModels()
        {
            //arrange
            _service.Setup(service => service.GetProducts()).Returns(ProductsVMS);
            //act
            var result = _controller.GetProducts();
            //assert
            result.Value.Should().BeEquivalentTo(ProductsVMS);
        }
        [Test]
        public void GetProducts_IfProductDbIstEmpty_ReturnsNotFoundObjectResult()
        {
            //arrange
            _service.Setup(service => service.GetProducts()).Returns(new NotFoundObjectResult(EmptyProductDb));
            //act
            var result = _controller.GetProducts();
            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        //Tests for delete product method
        [Test]
        public void Delete_IfProductExist_ReturnsTrue()
        {
            //arrange
            _service.Setup(service => service.Delete(It.IsAny<int>())).Returns(true);
            //act
            var result = _controller.Delete(1);
            //assert
            result.Value.Should().Be(true);
        }
        [Test]
        public void Delete_IfProductDoesNotExist_ReturnsNotFoundObjectResult()
        {
            //arrange
            _service.Setup(service => service.Delete(It.IsAny<int>())).Returns(new NotFoundObjectResult(ProductNotExist));
            //act
            var result = _controller.Delete(1);
            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        //Tests for Create product method
        [Test]
        public void Create_IfProductDoesNotExist_ReturnsTrue()
        {
            //arrange
            _service.Setup(service => service.Create(It.IsAny<ProductViewModel>())).Returns(true);
            //act
            var result = _controller.Create(ProductVM);
            //assert
            result.Value.Should().Be(true);
        }
        [Test]
        public void Create_IfProductAlreadyExist_ReturnsBadRequestObjectResult()
        {
            //arrange
            _service.Setup(service => service.Create(It.IsAny<ProductViewModel>())).Returns(new BadRequestObjectResult(ProductAlreadyExist));
            //act
            var result = _controller.Create(ProductVM);
            //assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        //Tests for EditProduct method
        [Test]
        public void EditProduct_IfProductDoesNotExist_ReturnsNotFoundObjectResult()
        {
            //arrange
            _service.Setup(service => service.Update(It.IsAny<ProductViewModel>())).Returns(new NotFoundObjectResult(ProductNotExist));
            //act
            var result = _controller.EditProduct(ProductVM);
            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
        [Test]
        public void EditProduct_IfProductExist_ReturnsTrue()
        {
            //arrange
            _service.Setup(service => service.Update(It.IsAny<ProductViewModel>())).Returns(true);
            //act
            var result = _controller.EditProduct(ProductVM);
            //assert
            result.Value.Should().Be(true);
        }
    }
}
