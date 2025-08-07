using MODEL;
using MODEL.DataBaseStructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public static class AnimalRepository
    {
        public static void AddAnimal(AnimalEntity animal)
        {
            using (var dbContext = new CUsersAntonSourceReposAplicacaowebDalDatabaseDatabase1MdfContext())
            {
                AnimalStruct animalStruct = new AnimalStruct()
                {
                    Id = animal.Id,
                    Name = animal.Name == null ? null : animal.Name,
                    Origin = (int)animal.Origin,
                    Gender = (int)animal.Gender,
                    BirthDate = animal.BirthDate,
                    PurchaseDate = animal.PurchaseDate,
                    DeathDate = animal.DeathDate,
                    Filiation 
                    Description = animal.Description,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null
                };

                dbContext.Add(batchStruct);        
                dbContext.SaveChanges();
            }
        }
    }
}
