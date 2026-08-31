using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Monitoring.Milk
{
    /// <summary>
    /// Persistence-backed milk tracker bound only to an opaque animal identifier.
    /// </summary>
    public abstract class MilkTracker<TMilk> : IMilkTracker
        where TMilk : MilkEntity
    {
        private readonly Guid _animalId;
        private readonly IMilkRepository<TMilk> _repository;

        public MilkTracker(Guid animalId, IMilkRepository<TMilk> repository)
        {
            if (animalId == Guid.Empty)
                throw new ArgumentException("An animal identifier is required.", nameof(animalId));

            _animalId = animalId;

            


            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        protected abstract Task<IReadOnlyList<TMilk>> ReadEntriesAsync(Guid animalId, CancellationToken ct = default);


        public Task<IReadOnlyList<MilkPoint>> GetHistoryAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<MilkPoint?> GetLatestAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
