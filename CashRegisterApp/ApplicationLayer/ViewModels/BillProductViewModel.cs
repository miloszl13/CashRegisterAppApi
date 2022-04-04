using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.ViewModels
{
    public class BillProductViewModel
    {
        [ForeignKey("Bill")]
        public string Bill_number { get; set; }
        [ForeignKey("Product")]
        public int Product_id { get; set; }

        public int Product_quantity { get; set; }
        public int? Products_cost { get; set; }
    }
}
