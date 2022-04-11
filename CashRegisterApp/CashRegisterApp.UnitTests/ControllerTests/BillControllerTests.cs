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
    public class BillControllerTests
    {
        private BillController _controller;
        private Mock<IBillService> _service;
        //domain entities
        private Bill Bill;
        private List<Bill> Bills;
        //View models
        private BillViewModel BillVM;
        private List<BillViewModel> BillVMS;
        //ErrorResponseModels
        private ErrorResponseModel EmptyBillDb;
        private ErrorResponseModel BillNotExist;
        private ErrorResponseModel BillAlreadyExist;
        private ErrorResponseModel InvalidCreditCard;

        [SetUp]
        public void SetUp()
        {
            _service = new Mock<IBillService>();
            _controller = new BillController(_service.Object);
            //entities
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
            //ErrorResponseModels
            EmptyBillDb = new ErrorResponseModel()
            {
                ErrorMessage = BillErrorMessages.empty_bills_db,
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
            BillNotExist = new ErrorResponseModel()
            {
                ErrorMessage = BillErrorMessages.bill_not_exist,
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
            BillAlreadyExist = new ErrorResponseModel()
            {
                ErrorMessage = BillErrorMessages.bill_already_exist,
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };
            InvalidCreditCard = new ErrorResponseModel()
            {
                ErrorMessage = BillErrorMessages.NotValidCC,
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };
        }
        //Tests for GetBills method
        [Test]
        public void GetBills_IfBillsDbIsNotEmpty_ReturnsBillViewModels()
        {
            //arrange
            _service.Setup(service => service.GetBills()).Returns(BillVMS);
            //act
            var result = _controller.GetBills();
            //assert
            result.Value.Should().BeEquivalentTo(BillVMS);
        }
        [Test]
        public void GetBills_IfBillsDbIsEmpty_ReturnsNotFoundObjectResult()
        {
            //arrange
            _service.Setup(service => service.GetBills()).Returns(new NotFoundObjectResult(EmptyBillDb));
            //act
            var result = _controller.GetBills();
            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        //Tests for delete Bill method
        [Test]
        public void DeleteBill_IfBillExist_ReturnsTrue()
        {
            //arrange
            _service.Setup(service => service.Delete(It.IsAny<string>())).Returns(true);
            //act
            var result = _controller.DeleteBill("200000000007540220");
            //assert
            result.Value.Should().Be(true);
        }
        [Test]
        public void DeleteBill_IfBillDoesNotExist_ReturnsNotFoundObjectResult()
        {
            //arrange
            _service.Setup(service => service.Delete(It.IsAny<string>())).Returns(new NotFoundObjectResult(BillNotExist));
            //act
            var result = _controller.DeleteBill("200000000007540220");
            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
        //Tests for Create Bill method
        [Test]
        public void CreateBill_IfBillDoesNotExist_ReturnsTrue()
        {
            //arrange
            _service.Setup(service => service.Create(It.IsAny<BillViewModel>())).Returns(true);
            //act
            var result = _controller.CreateBill(BillVM);
            //assert
            result.Value.Should().Be(true);
        }
        [Test]
        public void CreateBill_IfBillAlreadyExist_ReturnsBadRequestObjectResult()
        {
            //arrange
            _service.Setup(service => service.Create(It.IsAny<BillViewModel>())).Returns(new BadRequestObjectResult(BillAlreadyExist));
            //act
            var result = _controller.CreateBill(BillVM);
            //assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        //Tests for update bill method
        [Test]
        public void EditBill_IfBillDoesNotExist_ReturnsNotFoundObjectResult()
        {
            //arrange
            _service.Setup(service => service.Update(It.IsAny<BillViewModel>())).Returns(new NotFoundObjectResult(BillNotExist));
            //act
            var result = _controller.EditBill(BillVM);
            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
        [Test]
        public void EditBill_IfProductExist_ReturnsTrue()
        {
            //arrange
            _service.Setup(service => service.Update(It.IsAny<BillViewModel>())).Returns(true);
            //act
            var result = _controller.EditBill(BillVM);
            //assert
            result.Value.Should().Be(true);
        }

        //Tests for GetBillById method
        [Test]
        public void GetBillById_IfBillDoesNotExist_ReturnsNotFoundObjectResult()
        {
            //arrange
            _service.Setup(service => service.GetBillById(It.IsAny<string>())).Returns(new NotFoundObjectResult(BillNotExist));
            //act
            var result = _controller.GetBillById("200000000007540220");
            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
        [Test]
        public void GetBillById_IfBillExist_ReturnsBillViewModel()
        {
            //arrange
            _service.Setup(service => service.GetBillById(It.IsAny<string>())).Returns(BillVM);
            //act
            var result = _controller.GetBillById("200000000007540220");
            //assert
            result.Value.Should().Be(BillVM);
        }


        //Tests for AddCreditCard method
        [Test]
        public void AddCreditCard_IfBillWithThatBillNumberDoesNotExist_ReturnsNotFoundObjectResult()
        {
            //arrange
            _service.Setup(service => service.AddCreditCard(It.IsAny<string>(), It.IsAny<string>())).Returns(new NotFoundObjectResult(BillNotExist));
            //act
            var result = _controller.AddCreditCard("1", "4003600000000014");
            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
        [Test]
        public void AddCreditCard_IfYouPassInvalidCreditCard_ReturnsBadRequestObjectResult()
        {
            //arrange
            _service.Setup(service => service.AddCreditCard(It.IsAny<string>(), It.IsAny<string>())).Returns(new BadRequestObjectResult(InvalidCreditCard));
            //act
            var result = _controller.AddCreditCard("200000000007540220","1");
            //assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }
        [Test]
        public void AddCreditCard_ValidBillNumberAndValidCreditCard_ReturnsBillViewModel()
        {
            //arrange
            _service.Setup(service => service.AddCreditCard(It.IsAny<string>(), It.IsAny<string>())).Returns(BillVM);
            //act
            var result = _controller.AddCreditCard("200000000007540220", "4003600000000014");
            //assert
            result.Value.Should().Be(BillVM);
        }
    }
}
