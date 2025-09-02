using Microsoft.EntityFrameworkCore;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace INFRA
{
    /// <summary>
    /// Classe que implementa os métodos de interação com o banco de dados
    /// </summary>
    public sealed class BovinePastureMemberRepositoryEF : IBovinePastureMemberRepository
    {
        private readonly IDbContextFactory<AgroManagerDbContext> _factory;
        public BovinePastureMemberRepositoryEF(IDbContextFactory<AgroManagerDbContext> factory) => _factory = factory;

        public async Task AddAsync(BovinePastureMember entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.BovinePastureMembers.Add(entity);
            await db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(BovinePastureMember entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.BovinePastureMembers.Remove(entity);
            await db.SaveChangesAsync(ct);
        }

        public async Task<BovinePastureMember?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.BovinePastureMembers.AsNoTracking().SingleOrDefaultAsync(bA => bA.Id == id, ct);
        }

        public async Task<BovinePastureMember?> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.BovinePastureMembers.AsNoTracking().SingleOrDefaultAsync(bA => bA.AnimalId == animalId, ct);
        }
        public async Task<BovinePastureMember?> GetByBatchIdAsync(Guid batchId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.BovinePastureMembers.AsNoTracking().SingleOrDefaultAsync(bA => bA.BatchId == batchId, ct);
        }
    }
}

