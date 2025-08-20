using Microsoft.EntityFrameworkCore;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRA.Repositories
{
    public sealed class MilkRepositoryEF : IMilkRepository
    {
        private readonly IDbContextFactory<AgroManagerDbContext> _factory;
        public MilkRepositoryEF(IDbContextFactory<AgroManagerDbContext> factory) => _factory = factory;

        public async Task AddAsync(MilkEntity entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.MilkRecords.Add(entity);
            await db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(MilkEntity entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.MilkRecords.Remove(entity);
            await db.SaveChangesAsync(ct);
        }

        public async Task<MilkEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.MilkRecords.AsNoTracking().SingleOrDefaultAsync(m => m.Id == id, ct);
        }

        public async Task<MilkEntity?> GetByBovineIdAsync(Guid animalId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.MilkRecords.AsNoTracking().SingleOrDefaultAsync(m => m.BovineId == animalId, ct);
        }
    }
}
