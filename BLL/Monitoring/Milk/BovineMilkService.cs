using BLL.Common.Exceptions;
using MODEL;

namespace BLL.Services;

public interface IBovineMilkService : IMilkService<BovineMilk> { }

/// <summary>Bovine rules and association for the common milk recording pipeline.</summary>
public sealed class BovineMilkService : MilkService<BovineEntity, BovineMilk>, IBovineMilkService
{
    public BovineMilkService(IAnimalRepository<BovineEntity> animals, IBovineMilkRepository milk, TimeProvider? clock = null)
        : base(animals, milk, clock) { }

    protected override void ValidateSpecificRules(BovineEntity animal)
    {
        if (animal.Gender != Gender.Female)
            throw new BusinessRuleException("O lançamento de leite exige um bovino do sexo feminino.");
    }

    protected override BovineMilk CreateRecord(BovineEntity animal) => new() { BovineId = animal.Id };
}
