using MODEL;

namespace BLL.Services;

public interface IBovineWeightService : IWeightService<BovineWeight> { }

public sealed class BovineWeightService : WeightService<BovineEntity, BovineWeight>, IBovineWeightService
{
    public BovineWeightService(IAnimalRepository<BovineEntity> animals, IBovineWeightRepository weights,
        TimeProvider? clock = null)
        : base(animals, weights, clock) { }

    protected override BovineWeight CreateRecord(BovineEntity animal) => new() { BovineId = animal.Id };
}
