using System;
using System.Threading;
using System.Threading.Tasks;
using BLL.Common.Exceptions;
using MODEL;

namespace BLL.Services;

/// <summary>
/// Implementa o fluxo padrão de operações para animais (Template Method),
/// centralizando regras comuns e delegando regras específicas para classes derivadas.
/// </summary>
/// <typeparam name="TAnimal">
/// Tipo da entidade animal. Deve herdar de AnimalEntity.
/// </typeparam>
public abstract class AnimalServiceBase<TAnimal> : IAnimalService<TAnimal>
    where TAnimal : AnimalEntity
{
    private readonly IAnimalRepository<TAnimal> _repository;

    /// <summary>
    /// Inicializa o serviço base com o port (repositório genérico adaptado).
    /// </summary>
    /// <param name="repository">Port de repositório para o tipo de animal.</param>
    protected AnimalServiceBase(IAnimalRepository<TAnimal> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <summary>
    /// Cria um novo animal após validações comuns e específicas.
    /// </summary>
    public virtual async Task<TAnimal> CreateAsync(TAnimal entity, CancellationToken ct = default)
    {
        // -------------------- Validação de entrada --------------------
        if (entity is null) throw new ArgumentNullException(nameof(entity));

        // -------------------- Regras comuns (AnimalEntity) --------------------
        ValidateCommonRules(entity);

        // -------------------- Regras específicas (bovino/suíno/...) --------------------
        ValidateSpecificRules(entity);

        // -------------------- Persistência --------------------
        await _repository.AddAsync(entity, ct).ConfigureAwait(false);

        return entity;
    }

    public virtual async Task<TAnimal> UpdateAsync(TAnimal entity, CancellationToken ct = default)
    {
        // -------------------- Validação de entrada --------------------
        if (entity is null) throw new ArgumentNullException(nameof(entity));

        // -------------------- Id obrigatório para update --------------------
        if (entity.Id == Guid.Empty) throw new BusinessRuleException("O identificador informado é inválido.");

        // -------------------- Garante existência --------------------
        TAnimal? existing = await _repository.GetByIdAsync(entity.Id, ct).ConfigureAwait(false);

        // ----------------lançar InvalidOperationException ----------------
        if (existing is null) throw new InvalidOperationException("Registro não encontrado.");

        // -------------------- Regras comuns (AnimalEntity) --------------------
        ValidateCommonRules(entity);

        // -------------------- Regras específicas (bovino/suíno/...) --------------------
        ValidateSpecificRules(entity);

        // -------------------- Persistência --------------------
        await _repository.UpdateAsync(entity, ct).ConfigureAwait(false);

        return entity;
    }

    /// <summary>
    /// Remove um animal a partir do id, aplicando regras comuns e específicas de exclusão.
    /// </summary>
    public virtual async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        // -------------------- Validação de entrada --------------------
        if (id == Guid.Empty) throw new BusinessRuleException("O identificador informado é inválido.");

        // -------------------- Recupera entidade para validar exclusão --------------------
        TAnimal? entity = await _repository.GetByIdAsync(id, ct).ConfigureAwait(false);

        if (entity is null) throw new NotFoundException("Registro não encontrado.");

        // -------------------- Regras comuns de exclusão --------------------
        ValidateDeleteCommonRules(entity);

        // -------------------- Regras específicas de exclusão --------------------
        await ValidateDeleteSpecificRulesAsync(entity, ct).ConfigureAwait(false);

        // -------------------- Persistência --------------------
        await _repository.DeleteAsync(entity, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Obtém um animal pelo id.
    /// </summary>
    public virtual Task<TAnimal?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        // -------------------- Validação de entrada --------------------
        if (id == Guid.Empty) return Task.FromResult<TAnimal?>(null);

        return _repository.GetByIdAsync(id, ct);
    }

    // =====================================================================================================
    // Regras Comuns (AnimalEntity)
    // =====================================================================================================

    /// <summary>
    /// Aplica validações comuns válidas para qualquer animal.
    /// </summary>
    /// <param name="entity">Entidade a ser validada.</param>
    protected virtual void ValidateCommonRules(TAnimal entity)
    {
        // -------------------- Data de referência (hoje) --------------------
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);

        // -------------------- Name (opcional) --------------------
        // Name é opcional no seu modelo. Aqui apenas normalizamos quando existir.
        if (!string.IsNullOrWhiteSpace(entity.Name))
        {
            entity.Name = entity.Name.Trim();
        }

        // -------------------- BirthDate --------------------
        if (entity.BirthDate.HasValue)
        {
            DateOnly birthDate = entity.BirthDate.Value;

            if (birthDate > today) throw new BusinessRuleException("A data de nascimento não pode ser futura.");
        }

        // -------------------- PurchaseDate --------------------
        if (entity.PurchaseDate.HasValue)
        {
            DateOnly purchaseDate = entity.PurchaseDate.Value;

            if (purchaseDate > today) throw new BusinessRuleException("A data de compra não pode ser futura.");
        }

        // -------------------- DeathDate --------------------
        if (entity.DeathDate.HasValue)
        {
            DateOnly deathDate = entity.DeathDate.Value;

            if (deathDate > today) throw new BusinessRuleException("A data de óbito não pode ser futura.");

            if (entity.BirthDate.HasValue && deathDate < entity.BirthDate.Value)
                throw new BusinessRuleException("A data de óbito não pode ser anterior à data de nascimento.");

            if (entity.PurchaseDate.HasValue && deathDate < entity.PurchaseDate.Value)
                throw new BusinessRuleException("A data de óbito não pode ser anterior à data de compra.");
        }

        // -------------------- Gender / Origin --------------------
        // Por padrão, não forçamos aqui porque seu modelo aceita Unknown.
        // Se você decidir regra real (ex.: impedir Unknown), fazemos isso na base ou em cada espécie.
    }

    /// <summary>
    /// Regras comuns para exclusão (válidas para qualquer animal).
    /// </summary>
    /// <param name="entity">Entidade que será excluída.</param>
    protected virtual void ValidateDeleteCommonRules(TAnimal entity)
    {
        // Aqui entram regras genéricas do tipo:
        // - não excluir se DeathDate já estiver preenchida (se quiser impedir histórico)
        // - não excluir se existir vínculo comum a todos os animais (batch/parentage), quando padronizarmos isso
    }

    // =====================================================================================================
    // Ganchos (Template Method)
    // =====================================================================================================

    /// <summary>
    /// Aplica validações específicas da espécie (bovino/suíno/...).
    /// Deve lançar BusinessRuleException quando regra de negócio falhar.
    /// </summary>
    protected abstract void ValidateSpecificRules(TAnimal entity);

    /// <summary>
    /// Aplica validações específicas de exclusão da espécie.
    /// Usado para cenários onde precisa consultar outros repositórios (Milk, Parentage, Batch, etc.).
    /// </summary>
    protected abstract Task ValidateDeleteSpecificRulesAsync(TAnimal entity, CancellationToken ct);
}
