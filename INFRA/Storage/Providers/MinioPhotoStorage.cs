using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using MODEL;

namespace INFRA;

public sealed class MinioPhotoStorage : IPhotoStorage
{
    private readonly IMinioClient _minio;
    private readonly PhotoStorageOptions _opt;

    public MinioPhotoStorage(IMinioClient minio, IOptions<PhotoStorageOptions> options)
    {
        _minio = minio;
        _opt = options.Value;
    }

    public async Task<PhotoUploadResult> UploadAsync(PhotoUploadRequest request, CancellationToken ct = default)
    {
        string ext = GetExtensionFromContentType(request.ContentType);
        string safeScope = request.Scope.Trim().ToLowerInvariant();
        string safeEntityId = request.EntityId.Trim().ToLowerInvariant();
        string suffix = string.IsNullOrWhiteSpace(request.Suffix) ? "original" : request.Suffix.Trim().ToLowerInvariant();

        string key = $"{safeScope}/{safeEntityId}/{suffix}/{Guid.NewGuid():N}{ext}";

        await EnsureBucketExistsAsync(ct);

        var putArgs = new PutObjectArgs()
            .WithBucket(_opt.Bucket)
            .WithObject(key)
            .WithStreamData(request.Content)
            .WithObjectSize(request.ContentLength ?? -1)
            .WithContentType(request.ContentType);

        await _minio.PutObjectAsync(putArgs, ct);

        return new PhotoUploadResult(
            PhotoKey: key,
            ContentType: request.ContentType,
            ContentLength: request.ContentLength);
    }

    public async Task<string?> GetReadUrlAsync(string photoKey, TimeSpan? ttl = null, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(photoKey))
            return null;

        if (_opt.PublicRead && !string.IsNullOrWhiteSpace(_opt.PublicBaseUrl))
            return $"{_opt.PublicBaseUrl.TrimEnd('/')}/{_opt.Bucket}/{photoKey}";

        int expirySeconds = (int)(ttl ?? TimeSpan.FromMinutes(15)).TotalSeconds;

        var presignedArgs = new PresignedGetObjectArgs()
            .WithBucket(_opt.Bucket)
            .WithObject(photoKey)
            .WithExpiry(expirySeconds);

        return await _minio.PresignedGetObjectAsync(presignedArgs);
    }

    public async Task DeleteAsync(string photoKey, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(photoKey))
            return;

        var rmArgs = new RemoveObjectArgs()
            .WithBucket(_opt.Bucket)
            .WithObject(photoKey);

        await _minio.RemoveObjectAsync(rmArgs, ct);
    }

    private async Task EnsureBucketExistsAsync(CancellationToken ct)
    {
        var exists = await _minio.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(_opt.Bucket), ct);

        if (!exists)
            await _minio.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(_opt.Bucket), ct);
    }

    private static string GetExtensionFromContentType(string contentType) =>
        contentType.ToLowerInvariant() switch
        {
            "image/jpeg" => ".jpg",
            "image/png" => ".png",
            "image/webp" => ".webp",
            _ => ".bin"
        };
}