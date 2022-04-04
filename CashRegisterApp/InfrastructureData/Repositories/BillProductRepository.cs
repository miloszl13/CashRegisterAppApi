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
        public void Add(BillProduct billproduct)
        {

            _db.Add(billproduct);
            _db.SaveChanges();
        }
        public void Update(BillProduct billProduct)
        {
            _db.Update(billProduct);
            _db.SaveChanges();
        }
        //Delete bill_product
        public void Delete(BillProduct billproduct)
        {
            _db.Remove(billproduct);
            _db.SaveChanges();
        }
    }
}
