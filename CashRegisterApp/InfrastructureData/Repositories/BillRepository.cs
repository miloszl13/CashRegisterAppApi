using Domain;
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
    }
}
