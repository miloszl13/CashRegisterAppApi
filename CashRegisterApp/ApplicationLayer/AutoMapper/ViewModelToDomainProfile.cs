using ApplicationLayer.ViewModels;
using AutoMapper;
using Domain;
using Domain.Commands.BillCommands;
using Domain.Commands.ProductCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.AutoMapper
{
    public class ViewModelToDomainProfile:Profile
    {
        public ViewModelToDomainProfile()
        {
            //create methods
            CreateMap<BillViewModel, CreateBillCommand>()
                .ConstructUsing(b => new CreateBillCommand(b.Bill_number, b.Total_cost, b.Credit_card));
            CreateMap<ProductViewModel, CreateProductCommand>()
                .ConstructUsing(p => new CreateProductCommand(p.Product_id, p.Name, p.Cost));
            //update methods
            CreateMap<BillViewModel, UpdateBillCommand>()
              .ConstructUsing(b => new UpdateBillCommand(b.Bill_number, b.Total_cost, b.Credit_card));
            CreateMap<ProductViewModel, UpdateProductCommand>()
                .ConstructUsing(p => new UpdateProductCommand(p.Product_id, p.Name, p.Cost));
            //BillProductViewModel to BillProduct
            CreateMap<BillProductViewModel, BillProduct>();
        }
    }
}
