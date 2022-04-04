using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Commands.BillCommands
{
    public class UpdateBillCommand:BillCommand
    {
        public UpdateBillCommand(string bill_number, int? total_cost, string? credit_card)
        {
            Bill_number = bill_number;
            Total_cost = total_cost;
            Credit_card = credit_card;
        }
    }
}
