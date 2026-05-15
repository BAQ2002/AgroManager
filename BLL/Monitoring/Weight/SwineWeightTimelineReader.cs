using MODEL;

namespace BLL.Monitoring.Weight
{
    /// <summary>
    /// Swine-specific timeline reader backed by <see cref="ISwineWeightRepository"/>.
    /// </summary>
    public sealed class SwineWeightTimelineReader : ISwineWeightTimelineReader
    {
        private readonly ISwineWeightRepository _repository;

        public SwineWeightTimelineReader(ISwineWeightRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IReadOnlyList<WeightPoint>> GetHistoryAsync(Guid animalId, CancellationToken ct = default)
        {
            IReadOnlyList<SwineWeight> entries = await _repository.GetByAnimalIdAsync(animalId, ct).ConfigureAwait(false);
            return entries
                .OrderBy(w => w.OccurrenceDate)
                .Select(w => new WeightPoint(w.OccurrenceDate, w.Weight))
                .ToList();
        }

        public async Task<WeightPoint?> GetLatestAsync(Guid animalId, CancellationToken ct = default)
        {
            IReadOnlyList<SwineWeight> entries = await _repository.GetByAnimalIdAsync(animalId, ct).ConfigureAwait(false);
            SwineWeight? latest = entries
                .OrderByDescending(w => w.OccurrenceDate)
                .FirstOrDefault();

            return latest is null ? null : new WeightPoint(latest.OccurrenceDate, latest.Weight);
        }
    }
}
