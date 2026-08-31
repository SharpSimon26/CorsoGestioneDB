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
        services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICachedCategoryRepository, CachedCategoryRepository>();
        services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();        
        services.AddScoped<ISalesChannelRepository, SalesChannelRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ILocationInfoRepository, LocationInfoRepository>();
        services.AddScoped<ICachedLocationInfoRepository, CachedLocationInfoRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderLineRepository, OrderLineRepository>();
        services.AddScoped<IOrderStatusRepository, OrderStatusRepository>();
        services.AddScoped<ICachedOrderStatusRepository, CachedOrderStatusRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IProvinceRepository, ProvinceRepository>();
        services.AddScoped<IRegionRepository, RegionRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IStagingOrderRepository, StagingOrderRepository>();

        return services;
    }
}
