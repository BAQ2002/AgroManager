using BLL.Common.Exceptions;
using MODEL;

namespace BLL.Services;


/// <summary>Application contract for recording individual milkings.</summary>
public interface IMilkService<TMilk> where TMilk : MilkEntity
{
    Task<TMilk> CreateAsync(Guid animalId, DateTimeOffset occurredAt, float liters, CancellationToken ct = default);
    Task<TMilk> UpdateAsync(Guid recordId, DateTimeOffset occurredAt, float liters, CancellationToken ct = default);
    Task DeleteAsync(Guid recordId, CancellationToken ct = default);
}

/// <summary>
/// Template Method for recording milk. Species supply their own rules and animal association;
/// trackers remain responsible for querying measurement timelines.
/// </summary>
public abstract class MilkService<TAnimal, TMilk> : IMilkService<TMilk>
    where TAnimal : AnimalEntity
    where TMilk : MilkEntity
{
    private readonly IAnimalRepository<TAnimal> _animals;
    private readonly IMilkRepository<TMilk> _milk;
    private readonly TimeProvider _clock;

    protected MilkService(IAnimalRepository<TAnimal> animals, IMilkRepository<TMilk> milk, TimeProvider? clock = null)
    {
        _animals = animals ?? throw new ArgumentNullException(nameof(animals));
        _milk = milk ?? throw new ArgumentNullException(nameof(milk));
        _clock = clock ?? TimeProvider.System;
    }

    public async Task<TMilk> CreateAsync(Guid animalId, DateTimeOffset occurredAt, float liters, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        if (animalId == Guid.Empty)
            throw new BusinessRuleException("O identificador do animal é inválido.");
        var instant = ValidateMeasurement(occurredAt, liters);

        var animal = await _animals.GetByIdAsync(animalId, ct).ConfigureAwait(false)
            ?? throw new NotFoundException("Animal não encontrado.");

        ValidateSpecificRules(animal);

        var record = CreateRecord(animal);
        record.OccurredAt = instant;
        record.Liters = liters;
        await _milk.AddAsync(record, ct).ConfigureAwait(false);
        return record;
    }

    /// <summary>Corrects a measurement while preserving its animal, identifier and creation date.</summary>
    public async Task<TMilk> UpdateAsync(Guid recordId, DateTimeOffset occurredAt, float liters, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        ValidateRecordId(recordId);
        var instant = ValidateMeasurement(occurredAt, liters);
        var record = await _milk.GetByIdAsync(recordId, ct).ConfigureAwait(false)
            ?? throw new NotFoundException("Registro de leite não encontrado.");

        record.OccurredAt = instant;
        record.Liters = liters;
        record.UpdatedAt = _clock.GetUtcNow();
        await _milk.UpdateAsync(record, ct).ConfigureAwait(false);
        return record;
    }

    /// <summary>Deletes an existing measurement by identifier.</summary>
    public async Task DeleteAsync(Guid recordId, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        ValidateRecordId(recordId);
        var record = await _milk.GetByIdAsync(recordId, ct).ConfigureAwait(false)
            ?? throw new NotFoundException("Registro de leite não encontrado.");
        await _milk.DeleteAsync(record, ct).ConfigureAwait(false);
    }

    private static void ValidateRecordId(Guid recordId)
    {
        if (recordId == Guid.Empty)
            throw new BusinessRuleException("O identificador do registro de leite é inválido.");
    }

    private DateTimeOffset ValidateMeasurement(DateTimeOffset occurredAt, float liters)
    {
        if (!float.IsFinite(liters) || liters <= 0)
            throw new BusinessRuleException("O volume de leite deve ser positivo e finito.");
        if (occurredAt == default)
            throw new BusinessRuleException("O instante da ordenha deve ser informado.");

        var instant = occurredAt.ToUniversalTime();
        if (instant > _clock.GetUtcNow())
            throw new BusinessRuleException("O instante da ordenha não pode ser futuro.");
        return instant;
    }

    /// <summary>Validates eligibility according to species-specific business rules.</summary>
    protected abstract void ValidateSpecificRules(TAnimal animal);

    /// <summary>Creates a measurement with its species-specific animal foreign key.</summary>
    protected abstract TMilk CreateRecord(TAnimal animal);
}
