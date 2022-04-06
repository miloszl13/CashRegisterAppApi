using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ErrorMessages
{
    public class BillErrorMessages
    {
        public const string empty_bills_db = "There are no bills in database!";
        public const string bill_already_exist = "Bill with that Bill_number already exist!";
        public const string bill_not_exist = "Bill with that Bill_number does not exist!";
        public const string OverCostLimit = "You cant add more products to bill because total cost is limitet to 20000";
        public const string NotValidCC = "The credit card you tried to enter is in invalid form!";

    }
}
