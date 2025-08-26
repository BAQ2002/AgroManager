using Microsoft.EntityFrameworkCore;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRA
{
    internal class SwineBeefMemberRepositoryEF : ISwineBeefMemberRepository
    {
        private readonly IDbContextFactory<AgroManagerDbContext> _factory;
        public ISwineBeefMemberRepository(IDbContextFactory<AgroManagerDbContext> factory) => _factory = factory;

        public async Task AddAsync(SwineBeefMember entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.BatchAnimals.Add(entity);
            await db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(SwineBeefMember entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.BatchAnimals.Remove(entity);
            await db.SaveChangesAsync(ct);
        }

        public async Task<SwineBeefMember?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.BatchAnimals.AsNoTracking().SingleOrDefaultAsync(bA => bA.Id == id, ct);
        }

        public async Task<SwineBeefMember?> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.BatchAnimals.AsNoTracking().SingleOrDefaultAsync(bA => bA.AnimalId == animalId, ct);
        }
        public async Task<SwineBeefMember?> GetByBatchIdAsync(Guid batchId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.BatchAnimals.AsNoTracking().SingleOrDefaultAsync(bA => bA.BatchId == batchId, ct);
        }
    }
}

