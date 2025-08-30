using Microsoft.EntityFrameworkCore;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace INFRA
{
    public sealed class BovinePastureBatchRepositoryEF : IBovinePastureBatchRepository
    {
        private readonly IDbContextFactory<AgroManagerDbContext> _factory;
        public BovinePastureBatchRepositoryEF(IDbContextFactory<AgroManagerDbContext> factory) => _factory = factory;
        public async Task AddAsync(BovinePastureBatch entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.BovinePastureBatchs.Add(entity);
            await db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(BovinePastureBatch entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.BovinePastureBatchs.Remove(entity);
            await db.SaveChangesAsync(ct);
        }
        public async Task<BovinePastureBatch?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.BovinePastureBatchs.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
        }
        public async Task<BovinePastureBatch?> GetByPastureIdAsync(Guid pastureId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.BovinePastureBatchs.AsNoTracking().SingleOrDefaultAsync(x => x.PastureId == pastureId, ct);
        }
    
    }
}
