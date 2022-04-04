using ApplicationLayer.Interfaces;
using ApplicationLayer.Services;
using Domain.Interfaces;
using InfrastructureData;
using InfrastructureData.Repositories;
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
            //Application layer
            services.AddScoped<IBillService, BillService>();
            
            //InfraData Layer
            services.AddScoped<IBillRepository, BillRepository>();
    
            services.AddScoped<BillsDbContext>();
        }
    }
}
