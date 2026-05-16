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
    public sealed class SwineBeefMemberRepositoryEF : ISwineBeefMemberRepository
    {
        private readonly IDbContextFactory<AgroManagerDbContext> _factory;
        public SwineBeefMemberRepositoryEF(IDbContextFactory<AgroManagerDbContext> factory) => _factory = factory;

        public async Task AddAsync(SwineBeefMember entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.SwineBeefMembers.Add(entity);
            await db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(SwineBeefMember entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.SwineBeefMembers.Remove(entity);
            await db.SaveChangesAsync(ct);
        }

        public async Task<SwineBeefMember?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.SwineBeefMembers.AsNoTracking().SingleOrDefaultAsync(bA => bA.Id == id, ct);
        }

        public async Task<SwineBeefMember?> GetCurrentByAnimalIdAsync(Guid animalId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.SwineBeefMembers
                .AsNoTracking()
                .Where(bA => bA.AnimalId == animalId)
                .OrderBy(bA => bA.BatchExitDate.HasValue)
                .ThenByDescending(bA => bA.BatchEntryDate)
                .FirstOrDefaultAsync(ct);
        }
    
        public async Task<IReadOnlyList<SwineBeefMember>> GetHistoryByAnimalIdAsync(Guid animalId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.SwineBeefMembers
                .AsNoTracking()
                .Where(bA => bA.AnimalId == animalId)
                .OrderByDescending(bA => bA.BatchEntryDate)
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<SwineBeefMember>> GetHistoryByBatchIdAsync(Guid batchId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.SwineBeefMembers
                .AsNoTracking()
                .Where(bA => bA.BatchId == batchId)
                .OrderByDescending(bA => bA.BatchEntryDate)
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<SwineBeefMember>> ListActiveByBatchIdAsync(Guid batchId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.SwineBeefMembers
                .AsNoTracking()
                .Where(bA => bA.BatchId == batchId && bA.BatchExitDate == null)
                .OrderByDescending(bA => bA.BatchEntryDate)
                .ToListAsync(ct);
        }

        public async Task<bool> ExistsActiveByAnimalIdAsync(Guid animalId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.SwineBeefMembers
                .AsNoTracking()
                .AnyAsync(bA => bA.AnimalId == animalId && bA.BatchExitDate == null, ct);
        }

        public async Task<bool> CloseActiveMembershipAsync(Guid animalId, DateTimeOffset batchExitDate, string? exitReason, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);

            SwineBeefMember? activeMembership = await db.SwineBeefMembers
                .Where(bA => bA.AnimalId == animalId && bA.BatchExitDate == null)
                .OrderByDescending(bA => bA.BatchEntryDate)
                .FirstOrDefaultAsync(ct);

            if (activeMembership is null)
            {
                return false;
            }

            activeMembership.BatchExitDate = batchExitDate;
            activeMembership.ExitReason = exitReason;

            await db.SaveChangesAsync(ct);
            return true;
        }
    }
}

