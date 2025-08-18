using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public static class AnimalBatchRepository
    {

        public static void AddAnimalBatch(BatchAnimalEntity batchAnimal)
        {
            using (var dbContext = new CUsersAntonSourceReposAplicacaowebDalDatabaseDatabase1MdfContext())
            {
                dbContext.Add(batchAnimal);
                dbContext.SaveChanges();
            }
        }

        public static void RegisterAnimalExit(Guid animalId, DateTimeOffset dataSaida, string motivoSaida)
        {
            int index = _animalsList.FindIndex(item => item.animalId == animalId && item.dataSaida == null); //Encontra 

            if (index >= 0)
            {
                var atual = _animalsList[index];
                _animalsList[index] =
                    (
                    atual.animalId,
                    atual.dataEntrada,
                    dataSaida,
                    motivoSaida
                );
            }
        }
    }
}
