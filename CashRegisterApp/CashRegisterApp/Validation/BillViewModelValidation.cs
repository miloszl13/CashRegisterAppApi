using ApplicationLayer.ViewModels;
using FluentValidation;

namespace CashRegisterApp.Validation
{
    public class BillViewModelValidation:AbstractValidator<BillViewModel>
    {
        public BillViewModelValidation()
        {
            RuleFor(x => x.Bill_number).Must(IsValidBillNumber);
            RuleFor(x => x.Credit_card).Must(isValidCreditCard);
            RuleFor(x => x.Total_cost).LessThanOrEqualTo(20000);

        }
        private bool IsValidBillNumber(string billnumber)
        {
            if (billnumber.Length < 18)
            {
                return false;
            }
            int controlNumber = Convert.ToInt32(billnumber.Substring(billnumber.Length - 2));
            long firsttwoparts = Convert.ToInt64(billnumber.Substring(0, 16));
            long multiple = firsttwoparts * 100;
            long divide = multiple % 97;
            var result = 98 - divide;
            if (result == controlNumber)
            {
                return true;
            }
            return false;
        }
        private bool isValidCreditCard(string creditCard)
        {
            bool isValid = true;
            if(creditCard==null)
            {
                isValid = true;
                return isValid;
            }
            if(creditCard.Length != 13 && creditCard.Length != 15 && creditCard.Length != 16)
            {
                isValid = false;
            }
            else
            {
                if ((creditCard.Length == 13 || creditCard.Length == 16) && creditCard.StartsWith('4'))
                {
                    isValid = ValidateCreditCard(creditCard);

                }
                else if (creditCard.Length == 15 && (creditCard.StartsWith("34") || creditCard.StartsWith("37")))
                {
                    isValid = ValidateCreditCard(creditCard);
                }
                else if (creditCard.Length == 16 && (creditCard.StartsWith("51") || creditCard.StartsWith("52") || creditCard.StartsWith("53")
                    || creditCard.StartsWith("54") || creditCard.StartsWith("55")))
                {
                    isValid = ValidateCreditCard(creditCard);
                }

                else isValid = false;
            }
            return isValid;
        }
        private bool ValidateCreditCard(string card)
        {
            var cardReverse = card.Reverse();
            var reverseEveryOtherSecondToLast = new string(cardReverse.Where((ch, index) => index % 2 != 0).ToArray());
            string MultiplyDigitsByTwo = "";
            for (int i = 0; i < reverseEveryOtherSecondToLast.Length; i++)
            {
                int num = Int32.Parse((reverseEveryOtherSecondToLast[i].ToString()));
                num = num * 2;
                MultiplyDigitsByTwo = MultiplyDigitsByTwo + num.ToString();
            }
            int multipliedDigitsSummed = 0;
            for (int i = 0; i < MultiplyDigitsByTwo.Length; i++)
            {
                int num = Int32.Parse((MultiplyDigitsByTwo[i].ToString()));
                multipliedDigitsSummed = multipliedDigitsSummed + num;
            }
            var digitsWerentMultiplied = new string(cardReverse.Where((ch, index) => index % 2 == 0).ToArray());
            var digitsWerentMultipliedToDigits = Int32.Parse(digitsWerentMultiplied);
            int result = 0;
            for (int i = 0; i < digitsWerentMultiplied.Length; i++)
            {
                int num = Int32.Parse((digitsWerentMultiplied[i].ToString()));
                result = result + num;
            }
            int endResult = result + multipliedDigitsSummed;
            bool isValidCard = endResult % 10 == 0;
            return isValidCard;
        }
    }
}
