using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{

    public interface IMilkRepository
    {
        Task AddAsync(MilkEntity entity, CancellationToken ct = default);
        Task DeleteAsync(MilkEntity entity, CancellationToken ct = default);
        Task<MilkEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<MilkEntity?> GetByBovineIdAsync(Guid animalId, CancellationToken ct = default);
    }

}


