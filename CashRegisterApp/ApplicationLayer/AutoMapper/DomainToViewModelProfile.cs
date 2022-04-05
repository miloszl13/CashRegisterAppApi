using ApplicationLayer.ViewModels;
using AutoMapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.AutoMapper
{
    public class DomainToViewModelProfile:Profile
    {
        public DomainToViewModelProfile()
        {
            CreateMap<Bill, BillViewModel>();
            CreateMap<Product,ProductViewModel>();
            CreateMap<BillProduct, ProductViewModel>();
        }
    }
}
