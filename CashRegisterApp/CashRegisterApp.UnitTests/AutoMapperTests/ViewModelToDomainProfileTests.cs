using ApplicationLayer.AutoMapper;
using ApplicationLayer.ViewModels;
using AutoMapper;
using Domain;
using Domain.Commands.BillCommands;
using Domain.Commands.ProductCommands;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterApp.UnitTests.AutoMapperTests
{
    public class ViewModelToDomainProfileTests
    {
        MapperConfiguration config;
        IMapper mapper;
        BillViewModel billViewModel;
        ProductViewModel productViewModel;
        BillProductViewModel billProductViewModel;
        [SetUp]
        public void SetUp()
        {
            config = AutoMapperConfiguration.RegisterMappings();
            mapper = config.CreateMapper();
            //add values to billViewModel properties
            billViewModel=new BillViewModel();
            billViewModel.Bill_number = "200000000007540220";
            billViewModel.Total_cost = null;
            billViewModel.Credit_card = "4003600000000014";
            //add values to productViewModel properties
            productViewModel = new ProductViewModel();
            productViewModel.Product_id = 1;
            productViewModel.Name = "coffee";
            productViewModel.Cost = 50;
            //add values to billProductViewModel properties
            billProductViewModel=new BillProductViewModel();
            billProductViewModel.Product_id = 1;
            billProductViewModel.Bill_number= "200000000007540220";
            billProductViewModel.Product_quantity = 1;
            billProductViewModel.Products_cost = 50;

        }
        //BillCommand autoMapper tests
        [Test]
        public void CreateMap_BillViewModelToUpdateBillCommand_MapAdmitAllFields()
        {
            var result = mapper.Map<UpdateBillCommand>(billViewModel);
            result.Should().BeOfType<UpdateBillCommand>();
            result.Bill_number.Should().Be("200000000007540220");
            result.Total_cost.Should().Be(null);
            result.Credit_card.Should().Be("4003600000000014");
        }
        [Test]
        public void CreateMap_BillViewModelToCreateBillCommand_MapAdmitAllFields()
        {
            var result = mapper.Map<CreateBillCommand>(billViewModel);
            result.Should().BeOfType<CreateBillCommand>();
            result.Bill_number.Should().Be("200000000007540220");
            result.Total_cost.Should().Be(null);
            result.Credit_card.Should().Be("4003600000000014");
        }
        //Product commands autoMapper tests
        [Test]
        public void CreateMap_ProductViewModelToUpdateProductCommand_MapAdmitAllFields()
        {
            var result = mapper.Map<UpdateProductCommand>(productViewModel);
            result.Should().BeOfType<UpdateProductCommand>();
            result.Product_id.Should().Be(1);
            result.Name.Should().Be("coffee");
            result.Cost.Should().Be(50);
        }
        [Test]
        public void CreateMap_ProductViewModelToCreateProductCommand_MapAdmitAllFields()
        {
            var result = mapper.Map<CreateProductCommand>(productViewModel);
            result.Should().BeOfType<CreateProductCommand>();
            result.Product_id.Should().Be(1);
            result.Name.Should().Be("coffee");
            result.Cost.Should().Be(50);
        }
        //BillProductViewModel to BillProduct
        [Test]
        public void CreateMap_BillProductViewModelToBillProduct_MapAdmitAllFields()
        {
            var result = mapper.Map<BillProduct>(billProductViewModel);
            result.Should().BeOfType<BillProduct>();
            result.Product_id.Should().Be(1);
            result.Bill_number.Should().Be("200000000007540220");
            result.Product_quantity.Should().Be(1);
            result.Products_cost.Should().Be(50);
        }
    }
}

//_autoMapper.Map<BillProductViewModel>(bp)
