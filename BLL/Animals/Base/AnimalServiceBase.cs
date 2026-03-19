using BLL.Common.Exceptions;
using Minio.DataModel.Notification;
using MODEL;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Services;

/// <summary>
/// Implementa o fluxo padrão de operações para animais usando Template Method,
/// centralizando validações comuns e delegando validações específicas para classes derivadas.
/// </summary>
/// <typeparam name="TAnimal">Tipo da entidade animal. Deve herdar de <see cref="AnimalEntity"/>.</typeparam>
public abstract class AnimalServiceBase<TAnimal> : IAnimalService<TAnimal>
    where TAnimal : AnimalEntity
{
    private readonly IAnimalRepository<TAnimal> _repository;
    private readonly IPhotoStorage _photoStorage;

    /// <summary>
    /// Inicializa o serviço base com o port de persistência.
    /// </summary>
    /// <param name="repository">Repositório usado para leituras e gravações do tipo de animal.</param>
    /// <exception cref="ArgumentNullException">Lançada quando <paramref name="repository"/> é nulo.</exception>
    protected AnimalServiceBase(IAnimalRepository<TAnimal> repository, IPhotoStorage photoStorage)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _photoStorage = photoStorage ?? throw new ArgumentNullException(nameof(photoStorage)); ;
    }


    #region CRUD Methods --------------------------------------

    /// <summary>
    /// Cria um novo animal aplicando a sequência padrão do pipeline:
    /// valida entrada, executa <see cref="ValidateCommonRules(TAnimal)"/>, executa
    /// <see cref="ValidateSpecificRules(TAnimal)"/> e persiste chamando <see cref="IAnimalRepository{TAnimal}.AddAsync(TAnimal, CancellationToken)"/>.
    /// </summary>
    /// <param name="entity">Entidade a ser criada.</param>
    /// <param name="ct">Token de cancelamento para operações assíncronas.</param>
    /// <returns>A própria entidade recebida, após persistência.</returns>
    /// <exception cref="ArgumentNullException">Lançada quando <paramref name="entity"/> é nulo.</exception>
    public virtual async Task<TAnimal> CreateAsync(TAnimal entity, CancellationToken ct = default)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));

        ValidateCommonRules(entity);

        ValidateSpecificRules(entity);

        await _repository.AddAsync(entity, ct).ConfigureAwait(false);

        return entity;
    }

    /// <summary>
    /// Atualiza um animal existente seguindo o pipeline padrão:
    /// valida entrada e identificador, garante existência com <see cref="IAnimalRepository{TAnimal}.GetByIdAsync(Guid, CancellationToken)"/>,
    /// aplica regras comuns/específicas e persiste com <see cref="IAnimalRepository{TAnimal}.UpdateAsync(TAnimal, CancellationToken)"/>.
    /// </summary>
    /// <param name="entity">Entidade contendo os novos dados para atualização.</param>
    /// <param name="ct">Token de cancelamento para operações assíncronas.</param>
    /// <returns>A entidade recebida após atualização.</returns>
    /// <exception cref="ArgumentNullException">Lançada quando <paramref name="entity"/> é nulo.</exception>
    /// <exception cref="BusinessRuleException">Lançada quando o identificador é inválido.</exception>
    /// <exception cref="InvalidOperationException">Lançada quando o registro não é encontrado.</exception>
    public virtual async Task<TAnimal> UpdateAsync(TAnimal entity, CancellationToken ct = default)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));

        if (entity.Id == Guid.Empty) throw new BusinessRuleException("O identificador informado é inválido.");

        TAnimal? existing = await _repository.GetByIdAsync(entity.Id, ct).ConfigureAwait(false);

        if (existing is null) throw new InvalidOperationException("Registro não encontrado.");

        ValidateCommonRules(entity);

        ValidateSpecificRules(entity);

        await _repository.UpdateAsync(entity, ct).ConfigureAwait(false);

        return entity;
    }

    /// <summary>
    /// Remove um animal a partir do identificador.
    /// O fluxo valida o id, carrega a entidade via <see cref="IAnimalRepository{TAnimal}.GetByIdAsync(Guid, CancellationToken)"/>,
    /// executa <see cref="ValidateDeleteCommonRules(TAnimal)"/> e <see cref="ValidateDeleteSpecificRulesAsync(TAnimal, CancellationToken)"/>
    /// e finaliza chamando <see cref="IAnimalRepository{TAnimal}.DeleteAsync(TAnimal, CancellationToken)"/>.
    /// </summary>
    /// <param name="id">Identificador do registro a ser excluído.</param>
    /// <param name="ct">Token de cancelamento para operações assíncronas.</param>
    /// <exception cref="BusinessRuleException">Lançada quando o identificador é inválido.</exception>
    /// <exception cref="NotFoundException">Lançada quando não existe entidade para o identificador informado.</exception>
    public virtual async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty) throw new BusinessRuleException("O identificador informado é inválido.");

        TAnimal? entity = await _repository.GetByIdAsync(id, ct).ConfigureAwait(false);

        if (entity is null) throw new NotFoundException("Registro não encontrado.");

        ValidateDeleteCommonRules(entity);

        await ValidateDeleteSpecificRulesAsync(entity, ct).ConfigureAwait(false);

        await _repository.DeleteAsync(entity, ct).ConfigureAwait(false);
    }
    
    /// <summary>
    /// Obtém um animal por identificador.
    /// Quando o id é vazio, retorna <see langword="null"/> sem consultar o repositório;
    /// caso contrário, delega para <see cref="IAnimalRepository{TAnimal}.GetByIdAsync(Guid, CancellationToken)"/>.
    /// </summary>
    /// <param name="id">Identificador do animal.</param>
    /// <param name="ct">Token de cancelamento para operações assíncronas.</param>
    /// <returns>A entidade encontrada ou <see langword="null"/>.</returns>
    public virtual Task<TAnimal?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty) return Task.FromResult<TAnimal?>(null);

        return _repository.GetByIdAsync(id, ct) ?? throw new NotFoundException("Registro não encontrado.");
    }

    /// <summary>
    /// Lista todos os animais do tipo corrente delegando diretamente ao repositório.
    /// </summary>
    /// <param name="ct">Token de cancelamento para operações assíncronas.</param>
    /// <returns>Coleção somente leitura com os animais encontrados.</returns>
    public virtual Task<IReadOnlyList<TAnimal>> ListAsync(CancellationToken ct = default)
    {
        return _repository.ListAsync(ct);
    }

    /// <summary>
    /// Lista os animais do tipo corrente aplicando filtros comuns compartilhados entre espécies.
    /// Quando <paramref name="filters"/> é nulo, o método mantém o mesmo comportamento de <see cref="ListAsync(CancellationToken)"/>.
    /// </summary>
    /// <param name="filters">Modelo contendo critérios comuns de busca.</param>
    /// <param name="ct">Token de cancelamento para operações assíncronas.</param>
    /// <returns>Coleção somente leitura com os animais encontrados.</returns>
    /// <exception cref="ArgumentNullException">Lançada quando <paramref name="filters"/> é nulo.</exception>
    public virtual Task<IReadOnlyList<TAnimal>> ListAsync(AnimalFiltersModel filters, CancellationToken ct = default)
    {
        if (filters is null) throw new ArgumentNullException(nameof(filters));

        return _repository.ListAsync(filters, ct);
    }

    #endregion

    #region Storage Methods----------------------------

    public virtual async Task<string> UploadPhotoAsync(
         Guid animalId,
         Stream content,
         string contentType,
         long? contentLength,
         string? fileName = null,
         string? suffix = null,
         CancellationToken ct = default)
    {
        // Validações de entrada
        if (animalId == Guid.Empty) throw new BusinessRuleException("O identificador informado é inválido.");
        if (content is null) throw new ArgumentNullException(nameof(content));
        if (string.IsNullOrWhiteSpace(contentType)) throw new BusinessRuleException("O tipo do arquivo da foto deve ser informado.");

        // *talvez faca sentido criar um metodo quue retorne a fotoKey sem trazer a entidade completa, para otimizar esse fluxo. Por ora, mantem-se a consulta completa.
        TAnimal? entity = await GetByIdAsync(animalId, ct).ConfigureAwait(false);

        if (entity is null) throw new NotFoundException("Registro não encontrado.");

        string? oldPhotoKey = entity.PhotoKey;

        PhotoUploadResult uploadResult = await _photoStorage.UploadAsync(
            new PhotoUploadRequest(
                Content: content,
                ContentType: contentType,
                ContentLength: contentLength,
                FileName: fileName,
                Scope: "animals",
                EntityId: animalId.ToString("N"),
                Suffix: suffix),
            ct).ConfigureAwait(false);

        entity.PhotoKey = uploadResult.PhotoKey;

        await _repository.UpdateAsync(entity, ct).ConfigureAwait(false);

       //testar funcionamento do if, pode ser que o provedor ja subistitua o arquvo
        if (!string.IsNullOrWhiteSpace(oldPhotoKey) && !string.Equals(oldPhotoKey, uploadResult.PhotoKey, StringComparison.Ordinal))
        {
            await _photoStorage.DeleteAsync(oldPhotoKey, ct).ConfigureAwait(false);
        }

        return uploadResult.PhotoKey;
    }

    public virtual async Task<string?> GetPhotoUrlAsync(Guid animalId, TimeSpan? ttl = null, CancellationToken ct = default)
    {
        if (animalId == Guid.Empty) throw new BusinessRuleException("O identificador informado é inválido.");

        TAnimal? entity = await GetByIdAsync(animalId, ct).ConfigureAwait(false);

        if (entity is null) throw new NotFoundException("Registro não encontrado.");

        if (string.IsNullOrWhiteSpace(entity.PhotoKey))
            return null;

        return await _photoStorage.GetReadUrlAsync(entity.PhotoKey, ttl, ct).ConfigureAwait(false);
    }

    public virtual async Task RemovePhotoAsync(Guid animalId, CancellationToken ct = default)
    {
        if (animalId == Guid.Empty) throw new BusinessRuleException("O identificador informado é inválido.");

        TAnimal? entity = await GetByIdAsync(animalId, ct).ConfigureAwait(false);

        if (entity is null) throw new NotFoundException("Registro não encontrado.");

        if (string.IsNullOrWhiteSpace(entity.PhotoKey))
            return;

        string photoKey = entity.PhotoKey;

        await _photoStorage.DeleteAsync(photoKey, ct).ConfigureAwait(false);

        entity.PhotoKey = null;

        await _repository.UpdateAsync(entity, ct).ConfigureAwait(false);
    }

    #endregion


    #region Validation Methods --------------------------------------


    /// <summary>
    /// Aplica validações comuns válidas para qualquer animal.
    /// Normaliza o nome (quando informado), cria a referência temporal local
    /// com <see cref="DateOnly.FromDateTime(DateTime)"/> e valida coerência entre
    /// datas de nascimento, compra e óbito.
    /// </summary>
    /// <param name="entity">Entidade a ser validada.</param>
    /// <exception cref="BusinessRuleException">Lançada quando alguma regra temporal é violada.</exception>
    protected virtual void ValidateCommonRules(TAnimal entity)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);

        if (!string.IsNullOrWhiteSpace(entity.Name))
        {
            entity.Name = entity.Name.Trim();
        }

        if (entity.BirthDate.HasValue)
        {
            DateOnly birthDate = entity.BirthDate.Value;

            if (birthDate > today) throw new BusinessRuleException("A data de nascimento não pode ser futura.");
        }

        if (entity.PurchaseDate.HasValue)
        {
            DateOnly purchaseDate = entity.PurchaseDate.Value;

            if (purchaseDate > today) throw new BusinessRuleException("A data de compra não pode ser futura.");
        }

        if (entity.DeathDate.HasValue)
        {
            DateOnly deathDate = entity.DeathDate.Value;

            if (deathDate > today) throw new BusinessRuleException("A data de óbito não pode ser futura.");

            if (entity.BirthDate.HasValue && deathDate < entity.BirthDate.Value)
                throw new BusinessRuleException("A data de óbito não pode ser anterior à data de nascimento.");

            if (entity.PurchaseDate.HasValue && deathDate < entity.PurchaseDate.Value)
                throw new BusinessRuleException("A data de óbito não pode ser anterior à data de compra.");
        }
    }

    /// <summary>
    /// Executa validações comuns de exclusão válidas para todos os tipos de animais.
    /// Método virtual para permitir extensão em cenários de bloqueios genéricos de remoção.
    /// </summary>
    /// <param name="entity">Entidade que está no fluxo de exclusão.</param>
    protected virtual void ValidateDeleteCommonRules(TAnimal entity)
    {
    }

    /// <summary>
    /// Gancho do Template Method para validações de domínio específicas da espécie.
    /// É chamado pela base em operações de criação e atualização.
    /// </summary>
    /// <param name="entity">Entidade da espécie concreta a ser validada.</param>
    protected abstract void ValidateSpecificRules(TAnimal entity);

    /// <summary>
    /// Gancho do Template Method para validações específicas de exclusão.
    /// É chamado pela base antes da remoção no repositório.
    /// </summary>
    /// <param name="entity">Entidade da espécie concreta em processo de exclusão.</param>
    /// <param name="ct">Token de cancelamento para operações assíncronas.</param>
    protected abstract Task ValidateDeleteSpecificRulesAsync(TAnimal entity, CancellationToken ct);
    #endregion
}
