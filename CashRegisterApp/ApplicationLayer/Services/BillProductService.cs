using ApplicationLayer.Interfaces;
using ApplicationLayer.Model;
using ApplicationLayer.ViewModels;
using AutoMapper;
using Domain;
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
        private readonly IProductRepository _productRepository;
        private readonly IBillRepository _billrepository;
        private readonly IMapper _autoMapper;

        public BillProductService(IBillProductRepository billProductRepository,IProductRepository productRepository,IBillRepository billRepository,IMapper mapper)
        {
            _billProductRepository=billProductRepository;
            _productRepository=productRepository;
            _billrepository=billRepository;
            _autoMapper=mapper;
        }
        public ActionResult<List<BillProductViewModel>> GetAllBillProduct()
        {
            var billProducts = _billProductRepository.GetAllBillProducts();
            if (billProducts.Count() == 0)
            {
                var errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = BillProductErrorMessages.emptyBillProductDb,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
                return new NotFoundObjectResult(errorResponse);
            }
            var result = new List<BillProductViewModel>();
            if (billProducts != null)
            {
                foreach (var billproduct in billProducts)
                {                   
                    result.Add(_autoMapper.Map<BillProductViewModel>(billproduct));
                }
            }
            return result;
        }
        public ActionResult<bool> AddProductToBillProduct(BillProductViewModel billProductViewModel)
        {
            //find product with that prduct id
            var product = _productRepository.GetProducts().FirstOrDefault(x => x.Product_Id == billProductViewModel.Product_id);
            if (product == null)
            {
                var errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = ProductErrorMessages.productDoesNotExist,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
                return new NotFoundObjectResult(errorResponse);
            }
            //if bill product already exist

            var billproductfromDB = _billProductRepository.GetAllBillProducts()
                .FirstOrDefault(x => x.Bill_number == billProductViewModel.Bill_number && x.Product_id == billProductViewModel.Product_id);
            if (billproductfromDB != null)
            {
                int bpquantity = billproductfromDB.Product_quantity + billProductViewModel.Product_quantity;                        
                billproductfromDB.Bill_number = billProductViewModel.Bill_number;
                billproductfromDB.Product_id = billProductViewModel.Product_id;
                billproductfromDB.Product_quantity = bpquantity;
                billproductfromDB.Products_cost = (product.Cost * bpquantity);


                var bill = _billrepository.GetBillById(billproductfromDB.Bill_number);
                if (bill.Total_cost + billproductfromDB.Products_cost > 20000)
                {
                    var errorResponse = new ErrorResponseModel()
                    {
                        ErrorMessage = BillErrorMessages.overcostLimit,
                        StatusCode = System.Net.HttpStatusCode.BadRequest
                    };
                    return new BadRequestObjectResult(errorResponse);
                }
                else
                {
                    _billProductRepository.Update(billproductfromDB);
                    _billrepository.IncreaseTotalCost((billProductViewModel.Product_quantity * product.Cost), billproductfromDB.Bill_number);
                    return true;
                }
            }
            //if billproduct doesnt exist in db create new
            billProductViewModel.Products_cost = product.Cost * billProductViewModel.Product_quantity; 
            
            var billProduct = _autoMapper.Map<BillProduct>(billProductViewModel);
            var billDb = _billrepository.GetBillById(billProductViewModel.Bill_number);
            if (billDb.Total_cost + billProduct.Products_cost > 20000)
            {
                var errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = BillErrorMessages.overcostLimit,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return new BadRequestObjectResult(errorResponse);

            }
            else
            {
                _billrepository.IncreaseTotalCost(billProduct.Products_cost, billProduct.Bill_number);
                _billProductRepository.Add(billProduct);

                return true;
            }
        }
        public ActionResult<bool> Delete(string Bill_number, int Product_id, int quantity)
        {
            //store all billproducts from db
            List<BillProduct> bill_product = _billProductRepository.GetAllBillProducts().ToList();
            //find billproduct with given billnumber and product id
            var billProductdb = bill_product.FirstOrDefault(x => x.Bill_number == Bill_number && x.Product_id == Product_id);
            if (billProductdb == null)
            {
                var errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = BillProductErrorMessages.billProductNotExist,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
                return new NotFoundObjectResult(errorResponse);
            }


            if (quantity == billProductdb.Product_quantity)
            {
                _billrepository.DecreaseTotalCost(billProductdb.Products_cost, billProductdb.Bill_number);
                _billProductRepository.Delete(billProductdb);
            }
            else if (quantity > billProductdb.Product_quantity)
            {
                var errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = BillProductErrorMessages.tooManyProducts,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return new BadRequestObjectResult(errorResponse);

            }
            else
            {
                var product = _productRepository.GetProducts().FirstOrDefault(x => x.Product_Id == billProductdb.Product_id);
                if (product == null)
                {
                    var errorResponse = new ErrorResponseModel()
                    {
                        ErrorMessage = ProductErrorMessages.productDoesNotExist,
                        StatusCode = System.Net.HttpStatusCode.NotFound
                    };
                    return new NotFoundObjectResult(errorResponse);
                }
                var RightQuantity = billProductdb.Product_quantity - quantity;
                billProductdb.Product_quantity = RightQuantity;
                billProductdb.Products_cost = (product.Cost * RightQuantity);
                _billProductRepository.Update(billProductdb);
                _billrepository.DecreaseTotalCost((product.Cost * quantity), billProductdb.Bill_number);
            }

            return true;
        }

    }
}
