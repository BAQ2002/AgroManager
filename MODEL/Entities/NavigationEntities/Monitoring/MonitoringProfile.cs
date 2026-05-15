using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    /// <summary>
    /// Logical monitoring profile containing capabilities available for a specific animal instance.
    /// </summary>
    public interface IMonitoringProfile
    {
        Guid AnimalId { get; }
        Type AnimalType { get; }

        bool TryGetCapability<TCapability>(out TCapability? capability) where TCapability : class, IMonitoringCapability;
        WeightMonitoringCapability? Weight { get; }
    }

    /// <summary>
    /// Default implementation of <see cref="IMonitoringProfile"/> backed by a capability dictionary keyed by capability type.
    /// </summary>
    public sealed class MonitoringProfile : IMonitoringProfile
    {
        private readonly Dictionary<Type, IMonitoringCapability> _capabilities;
        public Guid AnimalId { get; }
        public Type AnimalType { get; }

        public MonitoringProfile(Guid animalId, Type animalType, IEnumerable<IMonitoringCapability> capabilities)
        {
            AnimalId = animalId;
            AnimalType = animalType ?? throw new ArgumentNullException(nameof(animalType));

            _capabilities = (capabilities ?? throw new ArgumentNullException(nameof(capabilities)))
                .ToDictionary(c => c.GetType(), c => c);
        }

        public bool TryGetCapability<TCapability>(out TCapability? capability) where TCapability : class, IMonitoringCapability
        {
            if (_capabilities.TryGetValue(typeof(TCapability), out IMonitoringCapability? raw) && raw is TCapability typed)
            {
                capability = typed;
                return true;
            }

            capability = null;
            return false;
        }

        public WeightMonitoringCapability? Weight
            => TryGetCapability<WeightMonitoringCapability>(out var capability)
                ? capability
                : throw new InvalidOperationException($"Weight capability is not available for animal type '{AnimalType.Name}' and id '{AnimalId}'.");
    }

    /// <summary>
    /// Strategy contract responsible for assembling capabilities for a concrete animal type.
    /// </summary>
    public interface IMonitoringAssemblyStrategy
    {
        Type AnimalType { get; }
        bool CanHandle(AnimalEntity animal);
        IEnumerable<IMonitoringCapability> BuildCapabilities(AnimalEntity animal);
    }

    /// <summary>
    /// Factory contract responsible for creating a monitoring profile from animal entities.
    /// </summary>
    public interface IMonitoringProfileFactory
    {
        IMonitoringProfile CreateForAnimal(AnimalEntity animal);
        IMonitoringProfile CreateForBovine(BovineEntity bovine);
        IMonitoringProfile CreateForSwine(SwineEntity swine);
    }
}
