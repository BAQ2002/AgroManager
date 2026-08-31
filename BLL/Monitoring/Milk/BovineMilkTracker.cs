using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Monitoring.Milk
{
    /// <summary>
    /// Bovine-specific milk tracker backed by <see cref="IBovineMilkRepository"/>.
    /// </summary>
    public sealed class BovineMilkTracker : MilkTracker<BovineMilk>
    {
        private readonly IBovineMilkRepository _repository;

        public BovineMilkTracker(Guid bovineId, IBovineMilkRepository repository)
            : base(bovineId)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected override Task<IReadOnlyList<BovineMilk>> ReadEntriesAsync(Guid animalId, CancellationToken ct = default)
            => _repository.GetByAnimalIdAsync(animalId, ct);
    }
}
