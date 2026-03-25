using BLL.Common.Exceptions;
using MODEL;

namespace BLL.Services;

/// <summary>
/// Serviço de aplicação dedicado ao ciclo de vida da foto do animal.
/// Orquestra persistência (<see cref="IAnimalRepository{TAnimal}"/>) e provider de storage (<see cref="IPhotoStorage"/>).
/// </summary>
/// <typeparam name="TAnimal">Tipo da entidade animal.</typeparam>
public sealed class AnimalPhotoService<TAnimal> : IAnimalPhotoService<TAnimal>
    where TAnimal : AnimalEntity
{
    private readonly IAnimalRepository<TAnimal> _repository;
    private readonly IPhotoStorage _photoStorage;

    public AnimalPhotoService(IAnimalRepository<TAnimal> repository, IPhotoStorage photoStorage)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _photoStorage = photoStorage ?? throw new ArgumentNullException(nameof(photoStorage));
    }

    public async Task<string> UploadPhotoAsync(
        Guid animalId,
        Stream content,
        string contentType,
        long? contentLength,
        string? fileName = null,
        string? suffix = null,
        CancellationToken ct = default)
    {
        if (animalId == Guid.Empty) throw new BusinessRuleException("O identificador informado é inválido.");
        if (content is null) throw new ArgumentNullException(nameof(content));
        if (string.IsNullOrWhiteSpace(contentType)) throw new BusinessRuleException("O tipo do arquivo da foto deve ser informado.");

        TAnimal entity = await GetRequiredAnimalAsync(animalId, ct).ConfigureAwait(false);

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

        if (!string.IsNullOrWhiteSpace(oldPhotoKey) && !string.Equals(oldPhotoKey, uploadResult.PhotoKey, StringComparison.Ordinal))
        {
            await _photoStorage.DeleteAsync(oldPhotoKey, ct).ConfigureAwait(false);
        }

        return uploadResult.PhotoKey;
    }

    public async Task<string?> GetPhotoUrlAsync(Guid animalId, TimeSpan? ttl = null, CancellationToken ct = default)
    {
        if (animalId == Guid.Empty) throw new BusinessRuleException("O identificador informado é inválido.");

        TAnimal entity = await GetRequiredAnimalAsync(animalId, ct).ConfigureAwait(false);

        if (string.IsNullOrWhiteSpace(entity.PhotoKey))
            return null;

        return await _photoStorage.GetReadUrlAsync(entity.PhotoKey, ttl, ct).ConfigureAwait(false);
    }

    public async Task RemovePhotoAsync(Guid animalId, CancellationToken ct = default)
    {
        if (animalId == Guid.Empty) throw new BusinessRuleException("O identificador informado é inválido.");

        TAnimal entity = await GetRequiredAnimalAsync(animalId, ct).ConfigureAwait(false);

        if (string.IsNullOrWhiteSpace(entity.PhotoKey))
            return;

        string photoKey = entity.PhotoKey;

        await _photoStorage.DeleteAsync(photoKey, ct).ConfigureAwait(false);

        entity.PhotoKey = null;

        await _repository.UpdateAsync(entity, ct).ConfigureAwait(false);
    }

    private async Task<TAnimal> GetRequiredAnimalAsync(Guid animalId, CancellationToken ct)
    {
        TAnimal? entity = await _repository.GetByIdAsync(animalId, ct).ConfigureAwait(false);

        if (entity is null)
            throw new NotFoundException("Registro não encontrado.");

        return entity;
    }
}