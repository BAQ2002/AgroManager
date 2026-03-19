using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IAnimalPhotoService<TAnimal>
       where TAnimal : AnimalEntity
    {
        Task<string> UploadPhotoAsync(
            Guid animalId,
            Stream content,
            string contentType,
            long? contentLength,
            string? fileName = null,
            string? suffix = null,
            CancellationToken ct = default);

        Task<string?> GetPhotoUrlAsync(Guid animalId, TimeSpan? ttl = null, CancellationToken ct = default);

        Task RemovePhotoAsync(Guid animalId, CancellationToken ct = default);
    }
}
