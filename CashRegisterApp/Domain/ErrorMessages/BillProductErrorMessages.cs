using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ErrorMessages
{
    public class BillProductErrorMessages
    {
        public const string emptyBillProductDb = "There are no products on any bill yet!";
        public const string billProductNotExist = "The products you have chosen to delete do not exist in that bill!";
        public const string tooManyProducts = "There aren't that many products to delete on this bill!";

    }
}
