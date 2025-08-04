using MODEL;

namespace BLL
{
    public static class BatchRepository
    {
        public static void AddBatch(BatchEntity batch)
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
                    AnimalBatchRepository.AddAnimalBatch(batchAnimalStruct);
                }
                dbContext.SaveChanges();
            }
        }

        
    }
}
