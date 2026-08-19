using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Infrastructure.Cache;
using CorsoGestioneDB.Infrastructure.Database;
using CorsoGestioneDB.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CorsoGestioneDB.Infrastructure.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Registrazione dei repository che accedono al database
        services.AddSingleton<DbConnectionFactory>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ILocationInfoRepository, LocationInfoRepository>();
        services.AddScoped<ICachedLocationInfoRepository, CachedLocationInfoRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderLineRepository, OrderLineRepository>();
        services.AddScoped<IOrderStatusRepository, OrderStatusRepository>();
        services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProvinceRepository, ProvinceRepository>();
        services.AddScoped<IRegionRepository, RegionRepository>();
        services.AddScoped<IStagingOrderRepository, StagingOrderRepository>();

        return services;
    }
}
