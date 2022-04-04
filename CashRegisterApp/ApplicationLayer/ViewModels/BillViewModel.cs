using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.ViewModels
{
    public class BillViewModel
    {
        public string Bill_number { get; set; }
        public int? Total_cost { get; set; }
        public string? Credit_card { get; set; }
        public List<BillProductViewModel> Bill_Products { get; set; }
    }
}
