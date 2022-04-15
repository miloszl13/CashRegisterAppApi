using InfrastructureData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace CashRegister.IntegrationTests
{
    public class TestingWebAppFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<BillsDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddDbContext<BillsDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryTestBase");
                });
                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<BillsDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<TestingWebAppFactory<Program>>>();

                    db.Database.EnsureDeleted();

                    db.Database.EnsureCreated();

                    try
                    {
                        Utilities.InitializeDbFotTests(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }
          
            });
        }
    }
}
