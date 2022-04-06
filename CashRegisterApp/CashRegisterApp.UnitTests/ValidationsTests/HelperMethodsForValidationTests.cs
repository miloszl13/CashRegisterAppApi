
using ApplicationLayer.Services;
using NUnit.Framework;

namespace CashRegisterApp.UnitTests.ValidationsTests
{
    public class HelperMethodsForValidationTests
    {
        private HelperMethodsForValidation validator;
        [SetUp]
        public void SetUp()
        {
            validator= new HelperMethodsForValidation();
        }
        [Test]
        public void IsValidCreditCard_invalidcard_ReturnsFalse()
        {
            //arrange
            string creditCard = "12345";
            //Act
            var result = validator.isValidCreditCard(creditCard);
            //Assert
            Assert.IsFalse(result);
        }
        [Test]
        public void IsValidCreditCard_validcard_ReturnsTrue()
        {
            //arange
            string creditCard = "371449635398431";
            //Act
            var result = validator.isValidCreditCard(creditCard);
            //Assert
            Assert.IsTrue(result);
        }
    }
}
