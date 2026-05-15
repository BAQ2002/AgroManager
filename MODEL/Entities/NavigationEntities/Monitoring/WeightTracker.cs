using System;

namespace MODEL
{
    /// <summary>
    /// Immutable weight measurement point (date + value) used by monitoring timelines.
    /// </summary>
    public sealed record WeightPoint(DateOnly Date, float Value);

    #region ------------------ INTERFACES -------------------------------------------
    /// <summary>
    /// Reader contract for retrieving ordered weight timelines and latest weight points.
    /// </summary>
    public interface IWeightTimelineReader
    {
        Task<IReadOnlyList<WeightPoint>> GetHistoryAsync(Guid animalId, CancellationToken ct = default);
        Task<WeightPoint?> GetLatestAsync(Guid animalId, CancellationToken ct = default);
    }
    public interface IBovineWeightTimelineReader : IWeightTimelineReader
    {
    }
    public interface ISwineWeightTimelineReader : IWeightTimelineReader
    {
    }

    /// <summary>
    /// Logical tracker for querying an animal weight history and latest point.
    /// </summary>
    public interface IWeightTracker
    {
        Task<IReadOnlyList<WeightPoint>> GetHistoryAsync(CancellationToken ct = default);
        Task<WeightPoint?> GetLatestAsync(CancellationToken ct = default);
    }
    #endregion


    /// <summary>
    /// Non-persistent weight tracker that delegates reads to a timeline reader bound to an animal id.
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

        public Task<IReadOnlyList<WeightPoint>> GetHistoryAsync(CancellationToken ct = default)
        => _reader.GetHistoryAsync(_animalId, ct);

        public Task<WeightPoint?> GetLatestAsync(CancellationToken ct = default)
        => _reader.GetLatestAsync(_animalId, ct);
    }

}
