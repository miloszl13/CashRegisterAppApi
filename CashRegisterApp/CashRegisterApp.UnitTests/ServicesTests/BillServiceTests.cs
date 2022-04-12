using ApplicationLayer.AutoMapper;
using ApplicationLayer.Services;
using ApplicationLayer.ViewModels;
using AutoMapper;
using Domain;
using Domain.Commands.BillCommands;
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
    public class BillServiceTests
    {
        private Mock<IBillRepository> _billRepository;
        private Mock<IMediatorHandler> _mediator;
        private BillService _service;
        //domain entities
        private Bill Bill;
        private List<Bill> Bills;
        //view models
        private BillViewModel BillVM;
        private List<BillViewModel> BillVMS;
        [SetUp]
        public void SetUp()
        {
            _billRepository = new Mock<IBillRepository>();
            _mediator = new Mock<IMediatorHandler>();
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ViewModelToDomainProfile());
                cfg.AddProfile(new DomainToViewModelProfile());
            });
            var mapper = new Mapper(mapperConfiguration);
            _service = new BillService(_billRepository.Object, _mediator.Object, mapper);

            //Domain entities
            Bill = new Bill()
            {
                Bill_number = "200000000007540220",
                Total_cost = null,
                Credit_card = "4003600000000014",
                Bill_Products = new List<BillProduct>()
            };
            Bills = new List<Bill>();
            Bills.Add(Bill);
            //View models
            BillVM = new BillViewModel()
            {
                Bill_number = "200000000007540220",
                Total_cost = null,
                Credit_card = "4003600000000014",
                Bill_Products = new List<BillProductViewModel>()

            };
            BillVMS = new List<BillViewModel>();
            BillVMS.Add(BillVM);
        }
        //GetBills tests
        [Test]
        public void GetBills_IfBillsDbIsEmpty_ReturnsNotFoundObjectResult()
        {
            //arrange
            _billRepository.Setup(repo => repo.GetBills()).Returns(new List<Bill>());
            //act
            var result = _service.GetBills();
            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
        [Test]
        public void GetBills_IfBillsDbIsNotEmpty_ReturnBillViewModels()
        {
            //arrange
            _billRepository.Setup(repo => repo.GetBills()).Returns(Bills);
            //act
            var result = _service.GetBills();
            //assert
            result.Value.Should().BeEquivalentTo(BillVMS);
        }
        //create Bill tests
        [Test]
        public void Create_IfBillDoesNotExist_ReturnsTrue()
        {
            //arrange
            _mediator.Setup(med => med.SendCommand(It.IsAny<CreateBillCommand>())).Returns(Task.FromResult(true));
            //act
            var result = _service.Create(BillVM);
            //assert
            result.Value.Should().Be(true);
        }
        [Test]
        public void Create_IfBillAlreadyExist_ReturnsBadRequestObjectResult()
        {
            //arrange
            _mediator.Setup(med => med.SendCommand(It.IsAny<CreateBillCommand>())).Returns(Task.FromResult(false));
            //act
            var result = _service.Create(BillVM);
            //assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }
        //update Bill tests
        [Test]
        public void Update_IfBillDoesNotExist_ReturnsNotFountObjectResult()
        {
            //arrange
            _mediator.Setup(med => med.SendCommand(It.IsAny<UpdateBillCommand>())).Returns(Task.FromResult(false));
            //act
            var result = _service.Update(BillVM);
            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
        [Test]
        public void Update_IfProductExist_ReturnsTrue()
        {
            //arrange
            _mediator.Setup(med => med.SendCommand(It.IsAny<UpdateBillCommand>())).Returns(Task.FromResult(true));
            //act
            var result = _service.Update(BillVM);
            //assert
            result.Value.Should().Be(true);
        }
        //Delete Bill tests
        [Test]
        public void Delete_IfProductDoesNotExist_ReturnsNotFoundObjectResult()
        {
            //arrange
            _billRepository.Setup(repo => repo.GetBills()).Returns(new List<Bill>());
            _billRepository.Setup(repo => repo.Delete(It.IsAny<Bill>()));
            //act
            var result = _service.Delete("200000000007540220");
            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
        [Test]
        public void Delete_IfProductExist_ReturnsTrue()
        {
            //arrange
            _billRepository.Setup(repo => repo.GetBills()).Returns(Bills);
            _billRepository.Setup(repo => repo.Delete(It.IsAny<Bill>()));
            //act
            var result = _service.Delete("200000000007540220");
            //assert
            result.Value.Should().Be(true);
        }
        //GetBillById tests
        [Test]
        public void GetBillById_IfBillDoesNotExist_ReturnsNotFoundObjectResult()
        {
            //arrange
            Bill bill = null;
            _billRepository.Setup(repo => repo.GetBillById(It.IsAny<string>())).Returns(bill);
            //act
            var result = _service.GetByllByBillNumber("200000000007540220");
            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
        [Test]
        public void GetBillById_IfBillExist_ReturnsBillViewModel()
        {
            //arrange
            _billRepository.Setup(repo => repo.GetBillById(It.IsAny<string>())).Returns(Bill);
            //act
            var result = _service.GetByllByBillNumber("200000000007540220");
            //assert
            result.Value.Should().BeEquivalentTo(BillVM);
        }
        //AddCreditCard 
        [Test]
        public void AddCreditCard_IfThatBillDoesNotExist_ReturnsNotFoundObjectResult()
        {
            //arrange
            Bill bill = null;
            _billRepository.Setup(repo => repo.GetBillById(It.IsAny<string>())).Returns(bill);
            //act
            var result = _service.AddCreditCard("4003600000000014", "200000000007540220");
            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
        [Test]
        public void AddCreditCard_IfBillExistButCreditCardIsInvalid_ReturnsBadRequestObjectResult()
        {
            //arrange
            _billRepository.Setup(repo => repo.GetBillById(It.IsAny<string>())).Returns(Bill);
            //act
            var result = _service.AddCreditCard("1", "200000000007540220");
            //assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }
        [Test]
        public void AddCreditCard_ValidBillNumberAndValidCreditCard_ReturnsBillViewModel()
        {
            //arrange
            _billRepository.Setup(repo => repo.GetBillById(It.IsAny<string>())).Returns(Bill);
            //act
            var result = _service.AddCreditCard("4929654516189549", "200000000007540220");
            //assert
            result.Value.Should().BeEquivalentTo(BillVM);
        }
    }
}
