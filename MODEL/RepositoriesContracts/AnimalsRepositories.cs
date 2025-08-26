using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public interface IBovineRepository
    {
        Task AddAsync(BovineEntity entity, CancellationToken ct = default);
        Task DeleteAsync(BovineEntity entity, CancellationToken ct = default);
        Task<BovineEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<BovineEntity?> GetByNameAsync(string name, CancellationToken ct = default);
        Task<BovineEntity?> GetByGenderAsync(Gender gender, CancellationToken ct = default);
        Task<BovineEntity?> GetByGenderAsync(int gender, CancellationToken ct = default);
    }

    public interface ISwineRepository
    {
        Task AddAsync(SwineEntity entity, CancellationToken ct = default);
        Task DeleteAsync(SwineEntity entity, CancellationToken ct = default);
        Task<SwineEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<SwineEntity?> GetByNameAsync(string name, CancellationToken ct = default);
        Task<SwineEntity?> GetByGenderAsync(Gender gender, CancellationToken ct = default);
        Task<SwineEntity?> GetByGenderAsync(int gender, CancellationToken ct = default);
    }

}
