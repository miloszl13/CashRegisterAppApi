using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Commands.ProductCommands
{
    public class CreateProductCommand:ProductCommand
    {
        public CreateProductCommand(int id, string name, int cost)
        {
            Product_id = id;
            Name = name;
            Cost = cost;
        }
    }
}
