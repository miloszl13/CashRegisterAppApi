using ApplicationLayer.Interfaces;
using ApplicationLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CashRegisterApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillController : ControllerBase
    {
        private readonly IBillService _billService;
        public BillController(IBillService billService)
        {
            _billService=billService;
        }
        //get all bills
        [HttpGet("GetAllBills")]
        public ActionResult<List<BillViewModel>> GetBills()
        {
            var BillsFromDb = _billService.GetBills();
            return BillsFromDb;
        }
        //Create new bill
        [HttpPost("CreateNewBill")]
        public ActionResult<bool> CreateBill([FromBody] BillViewModel billViewModel)
        {
            var CreatedBill = _billService.Create(billViewModel);
            return CreatedBill;
        }
        //update existing  bill
        [HttpPut("UpdateBill")]
        public ActionResult<bool> EditBill([FromBody] BillViewModel bill)
        {
            var UpdatedBill = _billService.Update(bill);
            return UpdatedBill;
        }
    }
}
