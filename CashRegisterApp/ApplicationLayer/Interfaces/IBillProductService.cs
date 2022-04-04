using ApplicationLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Interfaces
{
    public interface IBillProductService
    {
        ActionResult<List<BillProductViewModel>> GetAllBillProduct();
        ActionResult<bool> AddProductToBillProduct(BillProductViewModel billProductViewModel);
        ActionResult<bool> Delete(string Bill_number, int Product_id, int quantity);

    }
}
