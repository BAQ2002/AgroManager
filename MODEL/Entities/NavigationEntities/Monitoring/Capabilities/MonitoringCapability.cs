
namespace MODEL
{
    /// <summary>
    /// Marker interface for monitoring capabilities that can be composed into a profile per animal.
    /// </summary>
    public interface IMonitoringCapability
    {
    }

    /// <summary>
    /// Capability wrapper that exposes weight tracking features for an animal profile.
    /// </summary>
    public sealed class WeightMonitoringCapability : IMonitoringCapability
    {
        public WeightMonitoringCapability(IWeightTracker tracker)
        {
            Tracker = tracker ?? throw new ArgumentNullException(nameof(tracker));
        }

        public IWeightTracker Tracker { get; }
    }

}
