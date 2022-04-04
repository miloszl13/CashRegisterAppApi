using Domain;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureData.Repositories
{
    public class ProductRepository:IProductRepository
    {
        private BillsDbContext _db;
        public ProductRepository(BillsDbContext context)
        {
            _db = context;
        }
        public IEnumerable<Product> GetProducts()
        {
            return _db.Products;
        }
        public void Delete(Product product)
        {
            _db.Remove(product);
            _db.SaveChanges();
        }
        public void Add(Product product)
        {
            _db.Add(product);
            _db.SaveChanges();
        }
    }
}
