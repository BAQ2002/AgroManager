using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{

    public interface IMilkRepository<TMilk>
        where TMilk : MilkEntity

    {
        Task AddAsync(TMilk entity, CancellationToken ct = default);
        Task DeleteAsync(TMilk entity, CancellationToken ct = default);
        Task<TMilk?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<TMilk>> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
    }

    public interface IBovineMilkRepository : IMilkRepository<BovineMilk>
    {
    }



}


