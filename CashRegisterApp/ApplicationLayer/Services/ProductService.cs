using ApplicationLayer.Interfaces;
using ApplicationLayer.Model;
using ApplicationLayer.ViewModels;
using Domain.ErrorMessages;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services
{
    public class ProductService:IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
         
        }
        public ActionResult<List<ProductViewModel>> GetProducts()
        {
            var products = _productRepository.GetProducts().ToList();
            if (products.Count() == 0)
            {
                var errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = ProductErrorMessages.empty_products_db,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
                return new NotFoundObjectResult(errorResponse);
            }
            var result = new List<ProductViewModel>();
            foreach (var product in products)
            {
                result.Add(new ProductViewModel
                {
                    Product_id = product.Product_Id,
                    Name = product.Name,
                    Cost = product.Cost
                });
            }
            return result;
        }
    }
}
