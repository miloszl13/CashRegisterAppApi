using ApplicationLayer.AutoMapper;
using ApplicationLayer.ViewModels;
using AutoMapper;
using Domain;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterApp.UnitTests.AutoMapperTests
{
    public class DomainToViewModelProfileTests
    {
        MapperConfiguration config;
        IMapper mapper;
        Product product;
        Bill bill;
        BillProduct billProduct;
        
        [SetUp]
        public void SetUp()
        {
            config = AutoMapperConfiguration.RegisterMappings();
            mapper = config.CreateMapper();
            //add values to Bill properties
            bill = new Bill();
            bill.Bill_number = "200000000007540220";
            bill.Total_cost = null;
            bill.Credit_card = "4003600000000014";
            //add values to Product properties
            product = new Product();
            product.Product_Id = 1;
            product.Name = "coffee";
            product.Cost = 50;
            //add values to BillProduct properties
            billProduct = new BillProduct();
            billProduct.Product_id = 1;
            billProduct.Bill_number = "200000000007540220";
            billProduct.Product_quantity = 1;
            billProduct.Products_cost = 50;

        }
        //Bill autoMapper tests
        [Test]
        public void CreateMap_BillToBillViewModel_MapAdmitAllFields()
        {
            var result = mapper.Map<BillViewModel>(bill);
            result.Should().BeOfType<BillViewModel>();
            result.Bill_number.Should().Be("200000000007540220");
            result.Total_cost.Should().Be(null);
            result.Credit_card.Should().Be("4003600000000014");
        }
        //Product commands autoMapper tests
        [Test]
        public void CreateMap_ProductToProductViewModel_MapAdmitAllFields()
        {
            var result = mapper.Map<ProductViewModel>(product);
            result.Should().BeOfType<ProductViewModel>();
            result.Product_id.Should().Be(1);
            result.Name.Should().Be("coffee");
            result.Cost.Should().Be(50);
        }
        //BillProduct to BillProductViewModel
        [Test]
        public void CreateMap_BillProductToBillProductViewModel_MapAdmitAllFields()
        {
            var result = mapper.Map<BillProductViewModel>(billProduct);
            result.Should().BeOfType<BillProductViewModel>();
            result.Product_id.Should().Be(1);
            result.Bill_number.Should().Be("200000000007540220");
            result.Product_quantity.Should().Be(1);
            result.Products_cost.Should().Be(50);
        }
    }
}
