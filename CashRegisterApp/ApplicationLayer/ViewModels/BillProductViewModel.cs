using Domain;
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
        //ADDED public Bill Bill
        //and public Product Product
        [ForeignKey("Bill")]
        public string Bill_number { get; set; }
       // public Bill Bill { get; set; }
        [ForeignKey("Product")]
        public int Product_id { get; set; }
        //public Product Product { get; set; }

        public int Product_quantity { get; set; }
        public int? Products_cost { get; set; }
    }
}
