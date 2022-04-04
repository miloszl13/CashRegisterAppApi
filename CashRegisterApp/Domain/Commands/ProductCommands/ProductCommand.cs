using DomainCore.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Commands.ProductCommands
{
    public class ProductCommand:Command
    {
        public int Product_id { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
    }
}
