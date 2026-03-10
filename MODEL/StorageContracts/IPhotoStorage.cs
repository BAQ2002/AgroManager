using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL;

public interface IPhotoStorage
{
    Task<PhotoUploadResult> UploadAsync( PhotoUploadRequest request, CancellationToken ct = default);

    Task<string?> GetReadUrlAsync(string photoKey, TimeSpan? ttl = null, CancellationToken ct = default);

    Task DeleteAsync( string photoKey, CancellationToken ct = default);
}

public sealed record PhotoUploadRequest(
    Stream Content,
    string ContentType,
    long? ContentLength,
    string? FileName,
    string Scope,          // ex: "animals"
    string EntityId,       // ex: animalId
    string? Suffix = null  // ex: "profile"
);

public sealed record PhotoUploadResult(
    string PhotoKey,
    string ContentType,
    long? ContentLength,
    string? ETag = null
);