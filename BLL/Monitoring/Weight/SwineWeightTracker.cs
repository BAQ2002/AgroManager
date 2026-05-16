using MODEL;

namespace BLL 
{
    /// <summary>
    /// Swine-specific tracker that delegates persistence reads to <see cref="ISwineWeightRepository"/>
    /// while reusing transversal timeline rules from <see cref="WeightTrackerBase{TWeight}"/>.
    /// </summary>
    public class SwineWeightTracker : WeightTrackerBase<SwineWeight>, ISwineWeightTracker
    {
        private readonly ISwineWeightRepository _repository;

        /// <summary>
        /// Creates a tracker bound to one swine id.
        /// </summary>
        /// <param name="animalId">Swine identifier used for all read operations.</param>
        /// <param name="repository">Repository adapter responsible for swine weight persistence.</param>
        public SwineWeightTracker(Guid animalId, ISwineWeightRepository repository) : base(animalId)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <inheritdoc />
        protected override Task<IReadOnlyList<SwineWeight>> ReadEntriesAsync(Guid animalId, CancellationToken ct = default)
            => _repository.GetByAnimalIdAsync(animalId, ct);
    }
}


