using ApplicationLayer.Interfaces;
using ApplicationLayer.Services;
using Domain.CommandHandlers.BillCommandHandlers;
using Domain.CommandHandlers.ProductCommandHandlers;
using Domain.Commands.BillCommands;
using Domain.Commands.ProductCommands;
using Domain.Interfaces;
using DomainCore.Bus;
using InfraBus;
using InfrastructureData;
using InfrastructureData.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InversionOfControl
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //Domain InMemoryBus MediatR
            services.AddScoped<IMediatorHandler, InMemoryBus>();

            //Domain Handlers
            services.AddScoped<IRequestHandler<CreateBillCommand, bool>, BillCreateCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateBillCommand, bool>, BillUpdateCommandHandler>();
            services.AddScoped<IRequestHandler<CreateProductCommand, bool>, CreateProductCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateProductCommand, bool>, UpdateProductCommandHandler>();
            //Application layer
            services.AddScoped<IBillService, BillService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBillProductService, BillProductService>();
            services.AddScoped<ICurrencyExchangeService, CurrencyExchangeService>();
            //InfraData Layer
            services.AddScoped<IBillRepository, BillRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBillProductRepository, BillProductRepository>();


            services.AddScoped<BillsDbContext>();
        }
    }
}
