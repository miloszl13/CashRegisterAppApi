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
    public class BillProductService:IBillProductService
    {
        private readonly IBillProductRepository _billProductRepository;
        public BillProductService(IBillProductRepository billProductRepository)
        {
            _billProductRepository=billProductRepository;
        }
        public ActionResult<List<BillProductViewModel>> GetAllBillProduct()
        {
            var billProducts = _billProductRepository.GetAllBillProducts();
            if (billProducts.Count() == 0)
            {
                var errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = BillProductErrorMessages.empty_billsproducts_db,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
                return new NotFoundObjectResult(errorResponse);
            }
            var result = new List<BillProductViewModel>();
            if (billProducts != null)
            {
                foreach (var billproduct in billProducts)
                {
                    result.Add(new BillProductViewModel
                    {
                        Bill_number = billproduct.Bill_number,
                        Product_id = billproduct.Product_id,
                        Product_quantity = billproduct.Product_quantity,
                        Products_cost = billproduct.Products_cost
                    });
                }
            }
            return result;
        }
    }
}
