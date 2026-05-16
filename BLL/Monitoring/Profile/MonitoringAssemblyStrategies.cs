using MODEL;

namespace BLL.Monitoring.Profile
{
    /// <summary>
    /// Builds monitoring capabilities for <see cref="BovineEntity"/> animals.
    /// </summary>
    public sealed class BovineMonitoringAssemblyStrategy : IMonitoringAssemblyStrategy
    {
        private readonly IBovineWeightRepository _weightRepository;

        /// <summary>
        /// Initializes the strategy with the bovine weight persistence adapter.
        /// </summary>
        /// <param name="weightRepository">Repository used to create bovine-bound trackers.</param>
        public BovineMonitoringAssemblyStrategy(IBovineWeightRepository weightRepository)
        {
            _weightRepository = weightRepository ?? throw new ArgumentNullException(nameof(weightRepository));
        }

        public Type AnimalType => typeof(BovineEntity);

        public bool CanHandle(AnimalEntity animal) => animal is BovineEntity;

        public IEnumerable<IMonitoringCapability> BuildCapabilities(AnimalEntity animal)
        {
            if (animal is not BovineEntity bovine)
                throw new ArgumentException("Invalid animal type for bovine strategy.", nameof(animal));

            IBovineWeightTracker tracker = new BovineWeightTracker(bovine.Id, _weightRepository);
            yield return new WeightMonitoringCapability(tracker);
        }
    }

    /// <summary>
    /// Builds monitoring capabilities for <see cref="SwineEntity"/> animals.
    /// </summary>
    public sealed class SwineMonitoringAssemblyStrategy : IMonitoringAssemblyStrategy
    {
        private readonly ISwineWeightRepository _weightRepository;

        /// <summary>
        /// Initializes the strategy with the swine weight persistence adapter.
        /// </summary>
        /// <param name="weightRepository">Repository used to create swine-bound trackers.</param>
        public SwineMonitoringAssemblyStrategy(ISwineWeightRepository weightRepository)
        {
            _weightRepository = weightRepository ?? throw new ArgumentNullException(nameof(weightRepository));
        }

        public Type AnimalType => typeof(SwineEntity);

        public bool CanHandle(AnimalEntity animal) => animal is SwineEntity;

        public IEnumerable<IMonitoringCapability> BuildCapabilities(AnimalEntity animal)
        {
            if (animal is not SwineEntity swine)
                throw new ArgumentException("Invalid animal type for swine strategy.", nameof(animal));

            ISwineWeightTracker tracker = new SwineWeightTracker(swine.Id, _weightRepository);
            yield return new WeightMonitoringCapability(tracker);
        }
    }
}
