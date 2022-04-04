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

    }
}
