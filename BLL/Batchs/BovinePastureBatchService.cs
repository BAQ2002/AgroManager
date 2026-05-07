using BLL.Common.Exceptions;
using MODEL;

namespace BLL.Batchs;

public interface IBovinePastureBatchService : IBatchService<BovinePastureBatch>
{
}

public sealed class BovinePastureBatchService : BatchService<BovinePastureBatch, IBovinePastureBatchRepository>, IBovinePastureBatchService
{
    public BovinePastureBatchService(IBovinePastureBatchRepository repository)
        : base(repository)
    {
    }

    protected override void ValidateSpecificRules(BovinePastureBatch entity)
    {
        if (entity.PastureId == Guid.Empty)
            throw new BusinessRuleException("A pastagem do lote deve ser informada.");
    }
}