using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Product
    {
        [Key]
        public int Product_Id { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public List<BillProduct> Bill_Products { get; set; }
    }
}
