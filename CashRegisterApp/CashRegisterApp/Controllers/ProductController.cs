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
        public ActionResult<List<ProductViewModel>> GetProducts()
        {
            var productsDb = _productService.GetProducts();
            return productsDb;
        }

        [HttpDelete("DeleteProduct/{id:int}")]
        public ActionResult<bool> Delete([FromRoute] int id)
        {
            var deleteProduct = _productService.Delete(id);
            return deleteProduct;
        }
        [HttpPost("CreateProducts")]
        public ActionResult<bool> Create([FromBody] ProductViewModel productViewModel)
        {
            var createProduct = _productService.Create(productViewModel);
            return createProduct;
        }
        //update existing  product
        [HttpPut("UpdateProduct")]
        public ActionResult<bool> EditProduct([FromBody] ProductViewModel productViewModel)
        {
            var UpdatedProduct = _productService.Update(productViewModel);
            return UpdatedProduct;
        }
    }
}
