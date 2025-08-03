using MODEL;

namespace BLL
{
    public static class BatchRepository
    {
        public static void AddLote(BatchEntity batch)
        {
            using (var dbContext = new CUsersAntonSourceReposAplicacaowebDalDatabaseDatabase1MdfContext())
            {
                BatchStruct batchStruct = new BatchStruct()
                {
                    Id = batch.Id,
                    Name = batch.Name,
                    Description = batch.Description,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null
                };

                dbContext.Add(batchStruct);

                foreach (BatchAnimalStruct batchAnimalStruct in batch.AnimalsList) 
                {
                    AddAnimalBatch(batchAnimalStruct);
                }
                dbContext.SaveChanges();
            }
        }

        public static void AddAnimalBatch(BatchAnimalStruct animalEntity, BatchEntity batch, DateTimeOffset batchEntryDate)
        {
            using (var dbContext = new CUsersAntonSourceReposAplicacaowebDalDatabaseDatabase1MdfContext())
            {
                var batchAnimalStruct = new BatchAnimalStruct()
                {
                    Id = Guid.NewGuid(),
                    BatchId = batch.Id,
                    AnimalId = animalEntity.Id,
                    BatchEntryDate = batchEntryDate,
                    BatchExitDate = null,
                    ExitReason = null
                };

                dbContext.Add(batchAnimalStruct);
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
