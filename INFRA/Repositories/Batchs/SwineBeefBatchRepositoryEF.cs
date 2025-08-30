using Microsoft.EntityFrameworkCore;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace INFRA
{
    public sealed class SwineBeefBatchRepositoryEF : ISwineBeefBatchRepository
    {
        private readonly IDbContextFactory<AgroManagerDbContext> _factory;
        public SwineBeefBatchRepositoryEF(IDbContextFactory<AgroManagerDbContext> factory) => _factory = factory;
        public async Task AddAsync(SwineBeefBatch entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.SwineBeefBatchs.Add(entity);
            await db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(SwineBeefBatch entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.SwineBeefBatchs.Remove(entity);
            await db.SaveChangesAsync(ct);
        }
        public async Task<SwineBeefBatch?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.SwineBeefBatchs.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
        }
    }
}
