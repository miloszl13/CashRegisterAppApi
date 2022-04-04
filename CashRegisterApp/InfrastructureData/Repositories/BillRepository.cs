﻿using Domain;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureData.Repositories
{
    public class BillRepository:IBillRepository
    {
        private BillsDbContext _db;
        public BillRepository(BillsDbContext context)
        {
           _db= context;
        }

        public IEnumerable<Bill> GetBills()
        {
            return _db.Bills.Include(x => x.Bill_Products);
        }
        //add new bill
        public void Add(Bill bill)
        {
            _db.Add(bill);
            _db.SaveChanges();
        }
        //update bill
        public void Update(Bill bill, string id)
        {
            var billfromdb = GetBills().FirstOrDefault(x => x.Bill_number == id);
            if (billfromdb != null)
            {
                billfromdb.Total_cost = bill.Total_cost;
                billfromdb.Credit_card = bill.Credit_card;
            }
            _db.SaveChanges();
        }
        //Delete bill
        public void Delete(Bill bill)
        {
            _db.Remove(bill);
            _db.SaveChanges();
        }
        public Bill GetBillById(string id)
        {

            var bill = _db.Bills.Include(x => x.Bill_Products).FirstOrDefault(x => x.Bill_number == id);
            return bill;
        }
        //update bill total cost
        public void IncreaseTotalCost(int? TotalCost, string id)
        {
            var billfromdb = GetBills().FirstOrDefault(x => x.Bill_number == id);

            if (billfromdb != null)
            {
                if (billfromdb.Total_cost == null)
                {
                    billfromdb.Total_cost = 0;
                }
                billfromdb.Total_cost = billfromdb.Total_cost + TotalCost;
            }
            _db.SaveChanges();
        }
        public void DecreaseTotalCost(int? TotalCost, string id)
        {
            var billfromdb = GetBills().FirstOrDefault(x => x.Bill_number == id);

            if (billfromdb != null)
            {

                billfromdb.Total_cost = billfromdb.Total_cost - TotalCost;
            }
            _db.SaveChanges();
        }
    }
}
