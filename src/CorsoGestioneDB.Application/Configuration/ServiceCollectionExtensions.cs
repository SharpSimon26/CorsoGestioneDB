using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Pipeline;
using CorsoGestioneDB.Application.Pipeline.Rules.Reconstruction;
using CorsoGestioneDB.Application.Pipeline.Rules.Resolution;
using CorsoGestioneDB.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CorsoGestioneDB.Application.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Registrazione dei repository dell'applicazione
        services.AddScoped<ImportEngine>();
        services.AddScoped<ImportPipeline>();

        // Servizi Applicazione
        services.AddScoped<IProductCodeResolverService, ProductCodeResolverService>();
        services.AddScoped<ILocationReconstructorService, LocationReconstructorService>();

        // Regole di ricostruzione dei dati
        services.AddScoped<IReconstructionRule, ReconstructOrderStatusRule>();
        services.AddScoped<IReconstructionRule, ReconstructOrderDateRule>();
        services.AddScoped<IReconstructionRule, ReconstructDeliveryDateRule>();
        services.AddScoped<IReconstructionRule, ReconstructUnitPriceRule>();
        services.AddScoped<IReconstructionRule, ReconstructQuantityRule>();
        services.AddScoped<IReconstructionRule, ReconstructDiscountPctRule>();
        services.AddScoped<IReconstructionRule, ReconstructRevenueRule>();
        services.AddScoped<IReconstructionRule, ReconstructRoundingAdjustmentRule>();
        services.AddScoped<IReconstructionRule, ReconstructLocationRule>();

        // Regole di risoluzione delle foreign key
        services.AddScoped<IResolutionRule, ResolveProductCodeRule>();
        services.AddScoped<IResolutionRule, ResolveOrderStatusRule>();
        services.AddScoped<IResolutionRule, ResolveCategoryIdRule>();
        services.AddScoped<IResolutionRule, ResolveLocationRule>();

        // Stadi della pipeline
        services.AddScoped<NormalizeStage>();
        services.AddScoped<DuplicateStage>();
        services.AddScoped<ConvertStage>();
        services.AddScoped<ReconstructStage>();
        services.AddScoped<ResolveStage>();
        services.AddScoped<ValidateStage>();
        services.AddScoped<ImportStage>();
        services.AddScoped<LogStage>();

        return services;
    }
}
