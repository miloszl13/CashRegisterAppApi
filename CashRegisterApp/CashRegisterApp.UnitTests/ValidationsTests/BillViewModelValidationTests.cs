
using ApplicationLayer.ViewModels;
using CashRegisterApp.Validation;
using FluentAssertions;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;

namespace CashRegisterApp.UnitTests.ValidationsTests
{
    public class BillViewModelValidationTests
    {
        private BillViewModelValidation validator;
        BillViewModel BVMWithValidBillNumberAndInvalidCreditCard;
        BillViewModel BVMWithValidBillNumberAndValidCreditCard;
        BillViewModel BVMWithInvalidBillNumberAndValidCreditCard;
        BillViewModel BVMWithInvalidBillNumberAndInvalidCreditCard;
        [SetUp]
        public void SetUp()
        {
            validator = new BillViewModelValidation();
            BVMWithValidBillNumberAndInvalidCreditCard = new BillViewModel();
            BVMWithValidBillNumberAndInvalidCreditCard.Bill_number = "200000000007540220";
            BVMWithValidBillNumberAndInvalidCreditCard.Total_cost = null;
            BVMWithValidBillNumberAndInvalidCreditCard.Credit_card = "123";

            BVMWithValidBillNumberAndValidCreditCard =new BillViewModel();
            BVMWithValidBillNumberAndValidCreditCard.Bill_number = "200000000007540220";
            BVMWithValidBillNumberAndValidCreditCard.Total_cost = null;
            BVMWithValidBillNumberAndValidCreditCard.Credit_card = "4003600000000014";

            BVMWithInvalidBillNumberAndValidCreditCard = new BillViewModel();
            BVMWithInvalidBillNumberAndValidCreditCard.Bill_number = "5";
            BVMWithInvalidBillNumberAndValidCreditCard.Total_cost = null;
            BVMWithInvalidBillNumberAndValidCreditCard.Credit_card = "4003600000000014";

            BVMWithInvalidBillNumberAndInvalidCreditCard=new BillViewModel();
            BVMWithInvalidBillNumberAndInvalidCreditCard.Bill_number = "1";
            BVMWithInvalidBillNumberAndInvalidCreditCard.Total_cost = null;
            BVMWithInvalidBillNumberAndInvalidCreditCard.Credit_card = "12";

        }
        [Test]
        public void Validate_ValidBillViewModel_ReturnsTrue()
        {
            
            //Act
            var result = validator.Validate(BVMWithValidBillNumberAndValidCreditCard);
            //Assert
            result.IsValid.Should().BeTrue();
        }
        [Test]
        public void Validate_InvalidBillNumberAndInvalidCreditCard_ReturnsFalse()
        {

            //Act
            var result = validator.Validate(BVMWithInvalidBillNumberAndInvalidCreditCard);
            //Assert
            result.IsValid.Should().BeFalse();
        }
        [Test]
        public void Validate_ValidBillNumberAndInvalidCreditCard_ReturnsFalse()
        {

            //Act
            var result = validator.Validate(BVMWithValidBillNumberAndInvalidCreditCard);
            //Assert
            result.IsValid.Should().BeFalse();
        }
        [Test]
        public void Validate_InvalidBillNumberAndValidCreditCard_ReturnsFalse()
        {

            //Act
            var result = validator.Validate(BVMWithInvalidBillNumberAndValidCreditCard);
            //Assert
            result.IsValid.Should().BeFalse();
        }

    }
}
