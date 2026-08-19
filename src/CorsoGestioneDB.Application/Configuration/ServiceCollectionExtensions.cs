using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Pipeline;
using CorsoGestioneDB.Application.Pipeline.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace CorsoGestioneDB.Application.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Registrazione dei repository dell'applicazione
        services.AddScoped<ImportEngine>();
        services.AddScoped<ImportPipeline>();

        // Regole di ricostruzione dei dati
        services.AddScoped<IReconstructionRule, ReconstructUnitPriceRule>();
        services.AddScoped<IReconstructionRule, ReconstructQuantityRule>();

        // Stadi della pipeline
        services.AddScoped<NormalizeStage>();
        services.AddScoped<DuplicateStage>();
        services.AddScoped<ConvertStage>();
        services.AddScoped<ReconstructStage>();        
        services.AddScoped<ValidateStage>();
        services.AddScoped<ImportStage>();
        services.AddScoped<LogStage>();

        return services;
    }
}
