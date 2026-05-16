using System;

namespace MODEL
{
    /// <summary>
    /// Immutable weight measurement point (date + value) used by monitoring timelines.
    /// </summary>
    public sealed record WeightPoint(DateOnly Date, float Value);

    #region ------------------ INTERFACES -------------------------------------------
    /// <summary>
    /// Logical tracker for querying an animal weight history and latest point.
    /// </summary>
    public interface IWeightTracker
    {
        Task<IReadOnlyList<WeightPoint>> GetHistoryAsync(CancellationToken ct = default);
        Task<WeightPoint?> GetLatestAsync(CancellationToken ct = default);
    }

    /// <summary>
    /// Marker contract for bovine weight trackers.
    /// </summary>
    public interface IBovineWeightTracker : IWeightTracker
    {
    }

    /// <summary>
    /// Marker contract for swine weight trackers.
    /// </summary>
    public interface ISwineWeightTracker : IWeightTracker
    {
    }
    #endregion
}
