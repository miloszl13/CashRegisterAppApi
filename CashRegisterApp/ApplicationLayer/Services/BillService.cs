﻿using ApplicationLayer.Interfaces;
using ApplicationLayer.Model;
using ApplicationLayer.ViewModels;
using AutoMapper;
using Domain;
using Domain.Commands.BillCommands;
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
    public class BillService:IBillService
    {
        private readonly IBillRepository _billRepository;
        private readonly IMediatorHandler _bus;
        private readonly IMapper _autoMapper;

        public BillService(IBillRepository billRepository,IMediatorHandler mediatorHandler,IMapper mapper)
        {
            _billRepository = billRepository;
            _bus = mediatorHandler;
            _autoMapper = mapper;   
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
                    billproducts.Add(_autoMapper.Map<BillProductViewModel>(bp));
                }
                result.Add(_autoMapper.Map<BillViewModel>(bill));
                    
            }
            return result;
        }
        public ActionResult<bool> Create(BillViewModel billViewModel)
        {
            var Task = _bus.SendCommand(_autoMapper.Map<CreateBillCommand>(billViewModel));
            if (Task == Task.FromResult(false))
            {
                var errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = BillErrorMessages.bill_already_exist,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return new BadRequestObjectResult(errorResponse);
            }
            return true;
        }
        public ActionResult<bool> Update(BillViewModel billViewModel)
        {
            var Task = _bus.SendCommand(_autoMapper.Map<UpdateBillCommand>(billViewModel));
            if (Task == Task.FromResult(false))
            {
                var errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = BillErrorMessages.bill_not_exist,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
                return new NotFoundObjectResult(errorResponse);
            }
            return true;


        }
        //DELETE BILL FROM DB
        public ActionResult<bool> Delete(string id)
        {

            var billfromdb = _billRepository.GetBills().FirstOrDefault(x => x.Bill_number == id);
            if (billfromdb == null)
            {
                var errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = BillErrorMessages.bill_not_exist,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
                return new NotFoundObjectResult(errorResponse);
            }
            _billRepository.Delete(billfromdb);
            return true;


        }
        //GET BILL BY ID
        public ActionResult<BillViewModel> GetBillById(string id)
        {
            var billfromdb = _billRepository.GetBillById(id);

            if (billfromdb == null)
            {
                var errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = BillErrorMessages.bill_not_exist,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
                return new NotFoundObjectResult(errorResponse);
            }


            List<BillProductViewModel> billProducts = new List<BillProductViewModel>();
            foreach (BillProduct bp in billfromdb.Bill_Products)
            {
                billProducts.Add(_autoMapper.Map<BillProductViewModel>(bp));
            }
            var result= _autoMapper.Map<BillViewModel>(billfromdb);
            return result;
        }
        public ActionResult<BillViewModel> AddCreditCard(string cardNumber, string BillNumber)
        {
            var bill = _billRepository.GetBillById(BillNumber);
            if (bill == null)
            {
                var errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = BillErrorMessages.bill_not_exist,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
                return new NotFoundObjectResult(errorResponse);

            }
            //bill.Credit_card = cardNumber.ToString();
            //_billRepository.Update(bill, BillNumber);
            var billvm = _autoMapper.Map<BillViewModel>(bill);
            billvm.Credit_card = cardNumber.ToString();
            var Task = _bus.SendCommand(_autoMapper.Map<UpdateBillCommand>(billvm));
            if (Task == Task.FromResult(false))
            {
                var errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = BillErrorMessages.bill_not_exist,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return new BadRequestObjectResult(errorResponse);
            }
           


            List<BillProductViewModel> billProducts = new List<BillProductViewModel>();
            foreach (BillProduct bp in bill.Bill_Products)
            {
                billProducts.Add(_autoMapper.Map<BillProductViewModel>(bp));
            }
            var result = _autoMapper.Map<BillViewModel>(bill);
            return result;

        }

    }
}
