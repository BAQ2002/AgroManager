using BLL.Services;
using MODEL;

namespace BLL.Services
{
    public sealed class BovineParentageService : AnimalParentageService<BovineEntity, BovineParentageEntity>, IBovineParentageService
    {
        public BovineParentageService(
            IAnimalRepository<BovineEntity> animalRepository,
            IBovineParentageRepository parentageRepository)
            : base(animalRepository, parentageRepository)
        {
        }
    }
}