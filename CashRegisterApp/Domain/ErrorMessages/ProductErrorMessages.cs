using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ErrorMessages
{
    public class ProductErrorMessages
    {
        public const string emptyProductDb = "There are 0 products in database!";
        public const string productDoesNotExist = "Product with that id doesnt exist!";
        public const string productAlreadyExist = "Product with that id already exist!";

    }
}
