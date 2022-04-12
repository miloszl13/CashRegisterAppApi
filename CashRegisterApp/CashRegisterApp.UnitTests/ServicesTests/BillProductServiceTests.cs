using ApplicationLayer.AutoMapper;
using ApplicationLayer.Services;
using ApplicationLayer.ViewModels;
using AutoMapper;
using Domain;
using Domain.Interfaces;
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
    public class BillProductServiceTests
    {
        private Mock<IBillProductRepository> _billProductRepository;
        private Mock<IProductRepository> _productRepository;
        private Mock<IBillRepository> _billrepository;
        private BillProductService _service;

        //domain entities
        private BillProduct BillProduct;
        private List<BillProduct> BillProducts;
        private Product Product;
        private List<Product> Products;
        private Bill Bill;
        //View Models
        private BillProductViewModel BillProductVM;
        private List<BillProductViewModel> BillProductVMS;


        [SetUp]
        public void SetUp()
        {
            _billProductRepository = new Mock<IBillProductRepository>();
            _productRepository = new Mock<IProductRepository>();
            _billrepository= new Mock<IBillRepository>();
            var mapperConfiguration = new MapperConfiguration(cfg =>
              {
                  cfg.AddProfile(new DomainToViewModelProfile());
                  cfg.AddProfile(new ViewModelToDomainProfile());
              });
            var mapper=new Mapper(mapperConfiguration);
            _service=new BillProductService(_billProductRepository.Object,_productRepository.Object,_billrepository.Object,mapper);

            //Domain entities
            //BillProduct
            BillProduct = new BillProduct()
            {
                Bill_number = "200000000007540220",
                Product_id = 1,
                Product_quantity = 3,
                Products_cost = 150
            };
            BillProducts = new List<BillProduct>();
            BillProducts.Add(BillProduct);
            //Product
            Product = new Product()
            {
                Product_Id = 1,
                Name = "test",
                Cost = 50,
                Bill_Products = BillProducts
            };
            Products=new List<Product>();
            Products.Add(Product);
            //Bill
            Bill = new Bill()
            {
                Bill_number = "200000000007540220",
                Total_cost = null,
                Credit_card = "4003600000000014",
                Bill_Products = BillProducts
            };
            //View Models
            BillProductVM = new BillProductViewModel()
            {
                Bill_number = "200000000007540220",
                Product_id = 1,
                Product_quantity = 3,
                Products_cost = 150
            };
            BillProductVMS = new List<BillProductViewModel>();
            BillProductVMS.Add(BillProductVM);  
        
        }
        //GetAllBillProducts tests
        [Test]
        public void GetAllBillProducts_IfBillProducsDbIsEmpty_ReturnsNotFoundObjectResult()
        {
            //arrange
            _billProductRepository.Setup(repo => repo.GetAllBillProducts()).Returns(new List<BillProduct>());
            //act
            var result = _service.GetAllBillProduct();
            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
        [Test]
        public void GetAllBillProducts_IfBillProducsDbIsNotEmpty_ReturnListOfBillProductViewModels()
        {
            //arrange
            _billProductRepository.Setup(repo => repo.GetAllBillProducts()).Returns(BillProducts);
            //act
            var result = _service.GetAllBillProduct();
            //assert
            result.Value.Should().BeEquivalentTo(BillProductVMS);
        }
        //AddProductToBillProduct tests
        [Test]
        public void AddProductToBillProduct_IfProductWithPassedIdDoesNotExist_ReturnsNotFoundObjectResult()
        {
            //arrange
            _productRepository.Setup(repo => repo.GetProducts()).Returns(new List<Product>());
            //act
            var result = _service.AddProductToBillProduct(BillProductVM);
            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
        [Test]
        public void AddProductToBillProduct_BillProductExistAndBillsTotalCostIsLessThanOrEqual20000_ReturnsTrue()
        {
            //arrange
            _productRepository.Setup(repo => repo.GetProducts()).Returns(Products);
            var BillProductVM1 = new BillProductViewModel()
            {
                Bill_number = "200000000007540220",
                Product_id = 1,
                Product_quantity = 1,
                Products_cost = null
            };
            _billProductRepository.Setup(repo => repo.GetAllBillProducts()).Returns(BillProducts);
            _billrepository.Setup(repo => repo.GetBillById(It.IsAny<string>())).Returns(Bill);
            _billProductRepository.Setup(repo => repo.Update(It.IsAny<BillProduct>()));
            _billrepository.Setup(repo => repo.IncreaseTotalCost(It.IsAny<int>(), It.IsAny<string>()));
            //act
            var result = _service.AddProductToBillProduct(BillProductVM1);
            //assert
            result.Value.Should().Be(true);
        }
        [Test]
        public void AddProductToBillProduct_BillProductExistButBillsTotalCostGoesOver20000_ReturnsBadRequestObjectResult()
        {
            //arrange
            _productRepository.Setup(repo => repo.GetProducts()).Returns(Products);
            var Bill1 = new Bill()
            {
                Bill_number = "200000000007540220",
                Total_cost = 19900,
                Credit_card = "4003600000000014",
                Bill_Products = BillProducts
            };
            _billProductRepository.Setup(repo => repo.GetAllBillProducts()).Returns(BillProducts);
            _billrepository.Setup(repo => repo.GetBillById(It.IsAny<string>())).Returns(Bill1);
            _billProductRepository.Setup(repo => repo.Update(It.IsAny<BillProduct>()));
            _billrepository.Setup(repo => repo.IncreaseTotalCost(It.IsAny<int>(), It.IsAny<string>()));
            //act
            var result = _service.AddProductToBillProduct(BillProductVM);
            //assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }
        //Delete tests
        [Test]
        public void Delete_BillProductDoesNotExist_ReturnNotFoundObjectResult()
        {
            //arrange
            _billProductRepository.Setup(repo => repo.GetAllBillProducts()).Returns(new List<BillProduct>());         
            //act
            var result = _service.Delete("200000000007540220",1,5);
            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
        [Test]
        public void Delete_PassedQuantityIsEqualToProductQuantityInThatBillProduct_ReturnsTrue()
        {
            //arrange
            _billProductRepository.Setup(repo => repo.GetAllBillProducts()).Returns(BillProducts);
            _billrepository.Setup(repo => repo.DecreaseTotalCost(It.IsAny<int>(), It.IsAny<string>()));
            _billProductRepository.Setup(repo => repo.Delete(It.IsAny<BillProduct>()));
            //act
            var result = _service.Delete("200000000007540220", 1, 3);
            //assert
            result.Value.Should().Be(true);
        }
        [Test]
        public void Delete_PassedQuantityIsGreaterThanProductQuantityInThatBillProduct_ReturnsBadRequestObjectResult()
        {
            //arrange
            _billProductRepository.Setup(repo => repo.GetAllBillProducts()).Returns(BillProducts);
            //act
            var result = _service.Delete("200000000007540220", 1, 5);
            //assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }
        [Test]
        public void Delete_QuantityIsLessThanProductQuantityInThatBillProduct_ReturnsTrue()
        {
            //arrange
            _billProductRepository.Setup(repo => repo.GetAllBillProducts()).Returns(BillProducts);
            _productRepository.Setup(repo => repo.GetProducts()).Returns(Products);
            _billProductRepository.Setup(repo => repo.Update(It.IsAny<BillProduct>()));
            _billrepository.Setup(repo => repo.DecreaseTotalCost(It.IsAny<int>(), It.IsAny<string>()));
            //act
            var result = _service.Delete("200000000007540220", 1, 1);
            //assert
            result.Value.Should().Be(true);
        }
    }
}
