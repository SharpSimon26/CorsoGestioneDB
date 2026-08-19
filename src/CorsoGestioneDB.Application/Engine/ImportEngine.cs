using CorsoGestioneDB.Abstractions.Interfaces;

namespace CorsoGestioneDB.Application.Engine;

public class ImportEngine
{
    private readonly IStagingOrderRepository _stagingOrderRepository;
    private readonly ImportPipeline _pipeline;

    public ImportEngine(IStagingOrderRepository stagingOrderRepository, ImportPipeline pipeline)
    {
        _stagingOrderRepository = stagingOrderRepository;
        _pipeline = pipeline;
    }

    public async Task RunAsync()
    {
        var rows = await _stagingOrderRepository.GetAllAsync();
        var importContexts = rows.Select(m => new ImportContext(m)).ToList();
        if (importContexts.Any())
        {
            await _pipeline.ExecuteAsync(importContexts);
        }
    }
}
