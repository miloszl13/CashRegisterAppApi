﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBillRepository
    {
        IEnumerable<Bill> GetBills();
        void Add(Bill bill);
        void Update(Bill bill, string id);
        void Delete(Bill bill);
        public Bill GetBillById(string id);

    }
}
