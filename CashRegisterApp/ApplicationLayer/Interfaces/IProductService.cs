using ApplicationLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Interfaces
{
    public interface IProductService
    {
        ActionResult<List<ProductViewModel>> GetProducts();
        ActionResult<bool> Delete(int id);
        ActionResult<bool> Create(ProductViewModel productViewModel);
        ActionResult<bool> Update(ProductViewModel productViewModel);

    }
}
