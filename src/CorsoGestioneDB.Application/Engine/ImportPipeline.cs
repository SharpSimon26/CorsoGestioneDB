using CorsoGestioneDB.Application.Pipeline;

namespace CorsoGestioneDB.Application.Engine;

public class ImportPipeline
{
    private readonly NormalizeStage _normalize;
    private readonly DuplicateStage _duplicate;    
    private readonly ConvertStage _convert;
    private readonly ReconstructStage _reconstruct;
    private readonly ResolveStage _resolve;
    private readonly ValidateStage _validate;
    private readonly ImportStage _import;
    private readonly LogStage _log;

    public ImportPipeline(NormalizeStage normalize, DuplicateStage duplicate, ConvertStage convert,
        ReconstructStage reconstruct, ResolveStage resolve, ValidateStage validate, ImportStage import, LogStage log)
    {
        _normalize = normalize;
        _duplicate = duplicate;
        _convert = convert;
        _reconstruct = reconstruct;
        _resolve = resolve;
        _validate = validate;
        _import = import;
        _log = log;
    }

    public async Task ExecuteAsync(IEnumerable<ImportContext> contexts)
    {
        // 1. Normalize
        await _normalize.ExecuteAsync(contexts);

        // 2. Duplicate
        await _duplicate.ExecuteAsync(contexts);

        // 3. Convert
        await _convert.ExecuteAsync(contexts);

        // 4. Reconstruct
        await _reconstruct.ExecuteAsync(contexts);

        // 5. Resolve
        await _resolve.ExecuteAsync(contexts);

        // 6. Validate
        await _validate.ExecuteAsync(contexts);

        // 7. Import
        await _import.ExecuteAsync(contexts);

        // 8. Log
        await _log.ExecuteAsync(contexts);
    }
}
