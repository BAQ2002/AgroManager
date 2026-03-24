using MODEL;

namespace BLL.Services
{
    public sealed class SwineParentageService : AnimalParentageService<SwineEntity, SwineParentageEntity>, ISwineParentageService
    {
        public SwineParentageService(
            IAnimalRepository<SwineEntity> animalRepository,
            ISwineParentageRepository parentageRepository)
            : base(animalRepository, parentageRepository)
        {
        }
    }
}