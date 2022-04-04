using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBillProductRepository
    {
        IEnumerable<BillProduct> GetAllBillProducts();
        void Add(BillProduct billproduct);
        void Update(BillProduct billproduct);

    }
}
