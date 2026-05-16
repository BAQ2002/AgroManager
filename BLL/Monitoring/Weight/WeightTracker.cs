using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /// <summary>
    /// Base tracker implementation that centralizes transversal read rules for weight timelines,
    /// including deterministic history ordering and latest-point extraction.
    /// </summary>
    /// <typeparam name="TWeight">Concrete weight entity type associated with the animal species.</typeparam>
    public abstract class WeightTrackerBase<TWeight> : IWeightTracker
        where TWeight : WeightEntity
    {
        private readonly Guid _animalId;

        /// <summary>
        /// Initializes the tracker bound to a single animal identifier.
        /// </summary>
        /// <param name="animalId">Unique identifier of the animal whose timeline will be queried.</param>
        protected WeightTrackerBase(Guid animalId)
        {
            _animalId = animalId;
        }

        /// <summary>
        /// Reads raw weight entries from the species-specific repository.
        /// </summary>
        /// <param name="animalId">Animal identifier used by repository filters.</param>
        /// <param name="ct">Cancellation token for async flow control.</param>
        /// <returns>Raw weight entries returned by the underlying persistence adapter.</returns>
        protected abstract Task<IReadOnlyList<TWeight>> ReadEntriesAsync(Guid animalId, CancellationToken ct = default);

        /// <summary>
        /// Returns the ordered historical timeline from oldest to newest point.
        /// </summary>
        /// <param name="ct">Cancellation token for async flow control.</param>
        /// <returns>Normalized timeline points sorted by occurrence date ascending.</returns>
        public async Task<IReadOnlyList<WeightPoint>> GetHistoryAsync(CancellationToken ct = default)
        {
            IReadOnlyList<TWeight> entries = await ReadEntriesAsync(_animalId, ct).ConfigureAwait(false);
            return entries
                .OrderBy(w => w.OccurrenceDate)
                .Select(w => new WeightPoint(w.OccurrenceDate, w.Weight))
                .ToList();
        }

        /// <summary>
        /// Returns the most recent weight point for the bound animal.
        /// </summary>
        /// <param name="ct">Cancellation token for async flow control.</param>
        /// <returns>The latest point when available; otherwise <see langword="null"/>.</returns>
        public async Task<WeightPoint?> GetLatestAsync(CancellationToken ct = default)
        {
            IReadOnlyList<TWeight> entries = await ReadEntriesAsync(_animalId, ct).ConfigureAwait(false);
            TWeight? latest = entries
                .OrderByDescending(w => w.OccurrenceDate)
                .FirstOrDefault();

            return latest is null ? null : new WeightPoint(latest.OccurrenceDate, latest.Weight);
        }
    }
}
