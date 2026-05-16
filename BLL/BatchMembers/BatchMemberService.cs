using BLL.Common.Exceptions;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.BatchMembers
{

    public interface IBatchMemberService<TMember>
        where TMember : BatchMemberEntity
    {
        Task<TMember> AddMemberAsync(TMember entity, CancellationToken ct = default);
        Task<bool> CloseActiveMembershipAsync(Guid animalId, DateTimeOffset batchExitDate, string? exitReason, CancellationToken ct = default);
        Task<IReadOnlyList<TMember>> ListActiveByBatchIdAsync(Guid batchId, CancellationToken ct = default);
        Task<IReadOnlyList<TMember>> GetHistoryByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
        Task<IReadOnlyList<TMember>> GetHistoryByBatchIdAsync(Guid batchId, CancellationToken ct = default);
    }

    public abstract class BatchMemberService<TMember, TRepository> : IBatchMemberService<TMember>
        where TMember : BatchMemberEntity
        where TRepository : IBatchMemberRepository<TMember>
    {
        private readonly TRepository _repository;

        protected BatchMemberService(TRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<TMember> AddMemberAsync(TMember entity, CancellationToken ct = default)
        {
            ValidateNewMembership(entity);

            bool hasActiveMembership = await _repository.ExistsActiveByAnimalIdAsync(entity.AnimalId, ct).ConfigureAwait(false);
            if (hasActiveMembership)
                throw new BusinessRuleException("O animal já possui vínculo ativo em outro lote.");

            await _repository.AddAsync(entity, ct).ConfigureAwait(false);
            return entity;
        }

        public async Task<bool> CloseActiveMembershipAsync(Guid animalId, DateTimeOffset batchExitDate, string? exitReason, CancellationToken ct = default)
        {
            if (animalId == Guid.Empty) throw new BusinessRuleException("O identificador do animal é inválido.");

            TMember? activeMembership = await _repository.GetCurrentByAnimalIdAsync(animalId, ct).ConfigureAwait(false);
            if (activeMembership is null)
                throw new NotFoundException("Não existe vínculo ativo para o animal informado.");

            ValidateExitDate(activeMembership.BatchEntryDate, batchExitDate);

            return await _repository.CloseActiveMembershipAsync(animalId, batchExitDate, exitReason?.Trim(), ct).ConfigureAwait(false);
        }

        public Task<IReadOnlyList<TMember>> ListActiveByBatchIdAsync(Guid batchId, CancellationToken ct = default)
        {
            if (batchId == Guid.Empty) throw new BusinessRuleException("O identificador do lote é inválido.");
            return _repository.ListActiveByBatchIdAsync(batchId, ct);
        }

        public Task<IReadOnlyList<TMember>> GetHistoryByAnimalIdAsync(Guid animalId, CancellationToken ct = default)
        {
            if (animalId == Guid.Empty) throw new BusinessRuleException("O identificador do animal é inválido.");
            return _repository.GetHistoryByAnimalIdAsync(animalId, ct);
        }

        public Task<IReadOnlyList<TMember>> GetHistoryByBatchIdAsync(Guid batchId, CancellationToken ct = default)
        {
            if (batchId == Guid.Empty) throw new BusinessRuleException("O identificador do lote é inválido.");
            return _repository.GetHistoryByBatchIdAsync(batchId, ct);
        }


        private static void ValidateNewMembership(TMember entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            if (entity.AnimalId == Guid.Empty) throw new BusinessRuleException("O identificador do animal é inválido.");
            if (entity.BatchId == Guid.Empty) throw new BusinessRuleException("O identificador do lote é inválido.");

            DateTimeOffset entryDate = entity.BatchEntryDate;
            if (entryDate == default)
                throw new BusinessRuleException("A data de entrada do vínculo deve ser informada.");

            if (entity.BatchExitDate.HasValue)
                ValidateExitDate(entryDate, entity.BatchExitDate.Value);
        }

        private static void ValidateExitDate(DateTimeOffset entryDate, DateTimeOffset exitDate)
        {
            if (exitDate < entryDate)
                throw new BusinessRuleException("A data de saída não pode ser anterior à data de entrada.");
        }
    }



}
