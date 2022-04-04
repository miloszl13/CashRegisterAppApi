using ApplicationLayer.Interfaces;
using ApplicationLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CashRegisterApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        //get all products
        [HttpGet]
        public ActionResult<List<ProductViewModel>> GetBills()
        {
            var productsDb = _productService.GetProducts();
            return productsDb;
        }
    }
}
