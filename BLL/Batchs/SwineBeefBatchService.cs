using MODEL;

namespace BLL.Batchs;

public interface ISwineBeefBatchService : IBatchService<SwineBeefBatch>
{
}

public sealed class SwineBeefBatchService : BatchService<SwineBeefBatch, ISwineBeefBatchRepository>, ISwineBeefBatchService
{
    public SwineBeefBatchService(ISwineBeefBatchRepository repository)
        : base(repository)
    {
    }

    protected override void ValidateSpecificRules(SwineBeefBatch entity)
    {
    }
}