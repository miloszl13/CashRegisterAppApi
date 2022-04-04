using ApplicationLayer.Interfaces;
using ApplicationLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CashRegisterApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillProductController : ControllerBase
    {
        private readonly IBillProductService _billProductService;
        public BillProductController(IBillProductService billProductService)
        {
            _billProductService = billProductService;
        }
        //Get all billProducts
        [HttpGet("GetAllBillProducts")]
        public ActionResult<List<BillProductViewModel>> GetAllBillProduct()
        {
            var billProducts = _billProductService.GetAllBillProduct();
            return billProducts;
        }
    }
}
