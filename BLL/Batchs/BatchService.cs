using BLL.Common.Exceptions;
using MODEL;

namespace BLL.Batchs
{
    public interface IBatchService<TBatch>
      where TBatch : BatchEntity
    {
        Task<TBatch> CreateAsync(TBatch entity, CancellationToken ct = default);
        Task<TBatch> GetByIdAsync(Guid id, CancellationToken ct = default);
    }

    public abstract class BatchService<TBatch, TRepository> : IBatchService<TBatch>
    where TBatch : BatchEntity
    where TRepository : IBatchRepository<TBatch>
    {
        private readonly TRepository _repository;

        protected BatchService(TRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<TBatch> CreateAsync(TBatch entity, CancellationToken ct = default)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));

            ValidateCommonRules(entity);
            ValidateSpecificRules(entity);

            await _repository.AddAsync(entity, ct).ConfigureAwait(false);
            return entity;
        }

        public async Task<TBatch> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty) throw new BusinessRuleException("O identificador do lote é inválido.");

            TBatch? batch = await _repository.GetByIdAsync(id, ct).ConfigureAwait(false);
            if (batch is null) throw new NotFoundException("Lote não encontrado.");

            return batch;
        }

        protected virtual void ValidateCommonRules(TBatch entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
                throw new BusinessRuleException("O nome do lote deve ser informado.");

            entity.Name = entity.Name.Trim();
        }

        protected abstract void ValidateSpecificRules(TBatch entity);
    }
}
