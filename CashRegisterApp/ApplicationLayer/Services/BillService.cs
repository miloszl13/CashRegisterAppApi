using ApplicationLayer.Interfaces;
using ApplicationLayer.Model;
using ApplicationLayer.ViewModels;
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
    public class BillService:IBillService
    {
        private readonly IBillRepository _billRepository;
        public BillService(IBillRepository billRepository)
        {
            _billRepository = billRepository;
        }
        public ActionResult<List<BillViewModel>> GetBills()
        {
            var bills=_billRepository.GetBills().ToList();
            //if there are 0 bills ,return error
            if(bills.Count()==0)
            {
                var errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = BillErrorMessages.empty_bills_db,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
                return new NotFoundObjectResult(errorResponse); 
            }
            //else return result
            var result = new List<BillViewModel>();
            foreach(var bill in bills)
            {
                var billfromDb=bills.FirstOrDefault(x=>x.Bill_number==bill.Bill_number);
                List<BillProductViewModel> billproducts = new List<BillProductViewModel>();

                foreach(BillProduct bp in billfromDb.Bill_Products)
                {
                    billproducts.Add(new BillProductViewModel
                    {
                        Bill_number = bp.Bill_number,
                        Product_id = bp.Product_id,
                        Product_quantity = bp.Product_quantity,
                        Products_cost = bp.Products_cost
                    });
                }
                result.Add(new BillViewModel
                {
                    Bill_number = bill.Bill_number,
                    Total_cost = bill.Total_cost,
                    Credit_card = bill.Credit_card,
                    Bill_Products = billproducts
                });
                    
            }
            return result;
        }
    }
}
