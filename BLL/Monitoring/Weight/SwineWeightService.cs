using MODEL;

namespace BLL.Services;

public interface ISwineWeightService : IWeightService<SwineWeight> { }

public sealed class SwineWeightService : WeightService<SwineEntity, SwineWeight>, ISwineWeightService
{
    public SwineWeightService(IAnimalRepository<SwineEntity> animals, ISwineWeightRepository weights,
        TimeProvider? clock = null)
        : base(animals, weights, clock) { }

    protected override SwineWeight CreateRecord(SwineEntity animal) => new() { SwineId = animal.Id };
}
