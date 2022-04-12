using ApplicationLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Interfaces
{
    public interface IBillService
    {
        ActionResult<List<BillViewModel>> GetBills();
        ActionResult<bool> Create(BillViewModel billViewModel);
        ActionResult<bool> Update(BillViewModel billViewModel);
        ActionResult<bool> Delete(string id);
        ActionResult<BillViewModel> GetByllByBillNumber(string id);
        ActionResult<BillViewModel> AddCreditCard(string cardNumber, string BillNumber);

    }
}
