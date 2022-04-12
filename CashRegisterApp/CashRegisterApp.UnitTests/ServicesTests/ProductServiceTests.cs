using ApplicationLayer.AutoMapper;
using ApplicationLayer.Services;
using ApplicationLayer.ViewModels;
using AutoMapper;
using Domain;
using Domain.Commands.ProductCommands;
using Domain.Interfaces;
using DomainCore.Bus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterApp.UnitTests.ServicesTests
{
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _productRepository;
        private Mock<IMediatorHandler> _mediator;
        private ProductService _service;
        //domain entities
        private Product Product;
        private List<Product> Products;
        //view models
        private ProductViewModel ProductVM;
        private List<ProductViewModel> ProductVMS;
        [SetUp]
        public void SetUp()
        {
            _productRepository = new Mock<IProductRepository>();        
            _mediator = new Mock<IMediatorHandler>();
            var mapperConfiguration=  new MapperConfiguration(cfg =>
              {
                  cfg.AddProfile(new DomainToViewModelProfile());
                  cfg.AddProfile(new ViewModelToDomainProfile());
              });
            var mapper =new Mapper(mapperConfiguration);
            _service=new ProductService(_productRepository.Object,_mediator.Object,mapper);
            //Domain entities
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
                Cost = 100,
            };
            ProductVMS = new List<ProductViewModel>();
            ProductVMS.Add(ProductVM);
        }

        //GetProducts tests
        [Test]
        public void GetProducts_IfProductDbIsEmpty_ReturnsNotFoundObjectResult()
        {
            //arrange
            _productRepository.Setup(repo => repo.GetProducts()).Returns(new List<Product>());
            //act
            var result=_service.GetProducts();
            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
        [Test]
        public void GetProducts_IfProductDbIsNotEmpty_ReturnsListOfProductViewModels()
        {
            //arrange
            _productRepository.Setup(repo => repo.GetProducts()).Returns(Products);
            //act
            var result = _service.GetProducts();
            //assert
            result.Value.Should().BeEquivalentTo(ProductVMS);
        }
        //Delete product tests
        [Test]
        public void Delete_IfProductDoesNotExist_ReturnsNotFoundObjectResult()
        {
            //arrange
            _productRepository.Setup(repo => repo.GetProducts()).Returns(new List<Product>());
            _productRepository.Setup(repo => repo.Delete(It.IsAny<Product>()));
            //act
            var result = _service.Delete(1);
            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
        [Test]
        public void Delete_IfProductExist_ReturnsTrue()
        {
            //arrange
            _productRepository.Setup(repo => repo.GetProducts()).Returns(Products);
            _productRepository.Setup(repo => repo.Delete(It.IsAny<Product>()));
            //act
            var result = _service.Delete(1);
            //assert
            result.Value.Should().Be(true);
        }
        //create product tests
        [Test]
        public void Create_IfProductDoesNotExist_ReturnsTrue()
        {
            //arrange
            _mediator.Setup(med => med.SendCommand(It.IsAny<CreateProductCommand>())).Returns(Task.FromResult(true));
            //act
            var result = _service.Create(ProductVM);
            //assert
            result.Value.Should().Be(true);
        }
        [Test]
        public void Create_IfProductExist_ReturnsBadRequestObjectResult()
        {
            //arrange
            _mediator.Setup(med => med.SendCommand(It.IsAny<CreateProductCommand>())).Returns(Task.FromResult(false));
            //act
            var result = _service.Create(ProductVM);
            //assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }
        //update product tests
        [Test]
        public void Update_IfProductDoesNotExist_ReturnsNotFountObjectResult()
        {
            //arrange
            _mediator.Setup(med => med.SendCommand(It.IsAny<UpdateProductCommand>())).Returns(Task.FromResult(false));
            //act
            var result = _service.Update(ProductVM);
            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
        [Test]
        public void Update_IfProductExist_ReturnsTrue()
        {
            //arrange
            _mediator.Setup(med => med.SendCommand(It.IsAny<UpdateProductCommand>())).Returns(Task.FromResult(true));
            //act
            var result = _service.Update(ProductVM);
            //assert
            result.Value.Should().Be(true);
        }

    }
}
