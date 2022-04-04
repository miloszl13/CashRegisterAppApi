using Domain;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureData.Repositories
{
    public class BillProductRepository:IBillProductRepository
    {
        private BillsDbContext _db;
        public BillProductRepository(BillsDbContext db)
        {
            _db = db;
        }
        //get all bill products
        public IEnumerable<BillProduct> GetAllBillProducts()
        {
            return _db.BillProducts;
        }
    }
}
