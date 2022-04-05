using ApplicationLayer.Interfaces;
using ApplicationLayer.Model;
using ApplicationLayer.ViewModels;
using AutoMapper;
using Domain.Commands.ProductCommands;
using Domain.ErrorMessages;
using Domain.Interfaces;
using DomainCore.Bus;
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
        private readonly IMediatorHandler _bus;
        private readonly IMapper _autoMapper;

        public ProductService(IProductRepository productRepository,IMediatorHandler mediatorHandler,IMapper mapper)
        {
            _productRepository = productRepository;
            _bus = mediatorHandler;
            _autoMapper=mapper;
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
                result.Add(_autoMapper.Map<ProductViewModel>(product));

            }
            return result;
        }
        public ActionResult<bool> Delete(int id)
        {

            var product = _productRepository.GetProducts().FirstOrDefault(x => x.Product_Id == id);
            if (product == null)
            {
                var errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = ProductErrorMessages.product_doesnt_exist,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
                return new NotFoundObjectResult(errorResponse);
            }
            _productRepository.Delete(product);
            return true;


        }

        public ActionResult<bool> Create(ProductViewModel productViewModel)
        {
            var Task = _bus.SendCommand(_autoMapper.Map<CreateProductCommand>(productViewModel));
            if (Task == Task.FromResult(false))
            {
                var errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = ProductErrorMessages.product_already_exist,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return new BadRequestObjectResult(errorResponse);
            }
            return true;
        }
        public ActionResult<bool> Update(ProductViewModel productViewModel)
        {
            var Task = _bus.SendCommand(_autoMapper.Map<UpdateProductCommand>(productViewModel));
            if (Task == Task.FromResult(false))
            {
                var errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = ProductErrorMessages.product_doesnt_exist,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
                return new NotFoundObjectResult(errorResponse);
            }
            return true;


        }

    }
}
