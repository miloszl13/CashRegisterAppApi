using Domain;
using InfrastructureData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.IntegrationTests
{
    static class Utilities
    {
        public static void InitializeDbFotTests(BillsDbContext db)
        {
            //billproduct
            var billProduct = new BillProduct()
            {
                Bill_number = "200000000007540220",
                Product_id = 1,
                Product_quantity = 3,
                Products_cost = 60
            };
            db.BillProducts.Add(billProduct);
           
            //billproducts
            var billProducts = new List<BillProduct>() { billProduct };
           
            //products
            var product1 = new Product()
            {
                Product_Id = 1,
                Name = "pen",
                Cost = 20,
                Bill_Products= billProducts
            };
            var product2 = new Product()
            {
                Product_Id = 2,
                Name = "coffee",
                Cost = 30
            };
            db.Products.Add(product1);
            db.Products.Add(product2);

           //bill
            var bill = new Bill()
            {
                Bill_number = "200000000007540220",
                Total_cost = 150,
                Credit_card = "4003600000000014",
                Bill_Products = billProducts
            };
            db.Bills.Add(bill);
            db.SaveChanges();
        }



    }
}
