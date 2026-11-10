using BLL.Common.Exceptions;
using MODEL;

namespace BLL.Services;

public interface IWeightService<TWeight> where TWeight : WeightEntity
{
    Task<TWeight> CreateAsync(Guid animalId, DateOnly occurrenceDate, float weight, CancellationToken ct = default);
    Task<TWeight> UpdateAsync(Guid recordId, DateOnly occurrenceDate, float weight, CancellationToken ct = default);
    Task DeleteAsync(Guid recordId, CancellationToken ct = default);
}

/// <summary>Common weight recording pipeline; species provide the animal association.</summary>
public abstract class WeightService<TAnimal, TWeight> : IWeightService<TWeight>
    where TAnimal : AnimalEntity
    where TWeight : WeightEntity
{
    private readonly IAnimalRepository<TAnimal> _animals;
    private readonly IWeightRepository<TWeight> _weights;
    private readonly TimeProvider _clock;

    protected WeightService(IAnimalRepository<TAnimal> animals, IWeightRepository<TWeight> weights,
        TimeProvider? clock = null)
    {
        _animals = animals ?? throw new ArgumentNullException(nameof(animals));
        _weights = weights ?? throw new ArgumentNullException(nameof(weights));
        _clock = clock ?? TimeProvider.System;
    }

    public async Task<TWeight> CreateAsync(Guid animalId, DateOnly occurrenceDate, float weight, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        if (animalId == Guid.Empty)
            throw new BusinessRuleException("O identificador do animal é inválido.");
        ValidateMeasurement(occurrenceDate, weight);
        var animal = await _animals.GetByIdAsync(animalId, ct).ConfigureAwait(false)
            ?? throw new NotFoundException("Animal não encontrado.");
        ValidateSpecificRules(animal);
        var record = CreateRecord(animal);
        record.OccurrenceDate = occurrenceDate;
        record.Weight = weight;
        await _weights.AddAsync(record, ct).ConfigureAwait(false);
        return record;
    }

    /// <summary>Corrects a measurement without changing its animal or creation metadata.</summary>
    public async Task<TWeight> UpdateAsync(Guid recordId, DateOnly occurrenceDate, float weight, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        ValidateRecordId(recordId);
        ValidateMeasurement(occurrenceDate, weight);
        var record = await _weights.GetByIdAsync(recordId, ct).ConfigureAwait(false)
            ?? throw new NotFoundException("Registro de peso não encontrado.");
        record.OccurrenceDate = occurrenceDate;
        record.Weight = weight;
        record.UpdatedAt = _clock.GetUtcNow();
        await _weights.UpdateAsync(record, ct).ConfigureAwait(false);
        return record;
    }

    public async Task DeleteAsync(Guid recordId, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        ValidateRecordId(recordId);
        var record = await _weights.GetByIdAsync(recordId, ct).ConfigureAwait(false)
            ?? throw new NotFoundException("Registro de peso não encontrado.");
        await _weights.DeleteAsync(record, ct).ConfigureAwait(false);
    }

    private static void ValidateRecordId(Guid recordId)
    {
        if (recordId == Guid.Empty)
            throw new BusinessRuleException("O identificador do registro de peso é inválido.");
    }

    private void ValidateMeasurement(DateOnly occurrenceDate, float weight)
    {
        if (!float.IsFinite(weight) || weight <= 0)
            throw new BusinessRuleException("O peso deve ser positivo e finito.");
        if (occurrenceDate == default)
            throw new BusinessRuleException("A data da pesagem deve ser informada.");
        var today = DateOnly.FromDateTime(_clock.GetLocalNow().DateTime);
        if (occurrenceDate > today)
            throw new BusinessRuleException("A data da pesagem não pode ser futura.");
    }

    protected virtual void ValidateSpecificRules(TAnimal animal) { }
    protected abstract TWeight CreateRecord(TAnimal animal);
}
