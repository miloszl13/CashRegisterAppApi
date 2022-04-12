using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ErrorMessages
{
    public class BillErrorMessages
    {
        public const string emptyBillsDb = "There are no bills in database!";
        public const string billAlreadyExist = "Bill with that Bill_number already exist!";
        public const string billNotExist = "Bill with that Bill_number does not exist!";
        public const string overcostLimit = "You cant add more products to bill because total cost is limitet to 20000";
        public const string notValidCC = "The credit card you tried to enter is in invalid form!";

    }
}
