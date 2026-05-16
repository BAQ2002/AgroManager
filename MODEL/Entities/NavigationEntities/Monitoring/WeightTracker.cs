using System;

namespace MODEL
{
    /// <summary>
    /// Immutable weight measurement point (date + value) used by monitoring timelines.
    /// </summary>
    public sealed record WeightRecord(DateOnly Date, float Value);

    /// <summary>
    /// Classe de navegação não presistente que delega a leitura
    /// para um <see cref="IWeightTimelineReader"/> a partir de
    /// um <see cref="AnimalEntity"/>.Id.
    /// </summary>
    public sealed class WeightTracker : IWeightTracker
    {
        private readonly Guid _animalId;
        private readonly IWeightTimelineReader _reader;

        public WeightTracker(Guid animalId, IWeightTimelineReader reader)
        {
            _animalId = animalId;
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        public Task<IReadOnlyList<WeightRecord>> GetHistoryAsync(CancellationToken ct = default)
        => _reader.GetHistoryAsync(_animalId, ct);

        public Task<WeightRecord?> GetLatestAsync(CancellationToken ct = default)
        => _reader.GetLatestAsync(_animalId, ct);
    }

}
