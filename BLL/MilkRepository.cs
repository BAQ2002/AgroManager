using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public static class MilkRepository
    {
        public static void AddMilk(MilkEntity milkEntity)
        {
            using (var dbContext = new CUsersAntonSourceReposAplicacaowebDalDatabaseDatabase1MdfContext())
            {
                dbContext.Add(milkEntity);
                dbContext.SaveChanges();
            }
        }

    }
}
