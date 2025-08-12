using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    public static class BovineRepository
    {

        public static void AddAnimal(BovineEntity bovineAnimal)
        {
            using (var dbContext = new CUsersAntonSourceReposAplicacaowebDalDatabaseDatabase1MdfContext())
            { 
                dbContext.Add(bovineAnimal);
                dbContext.SaveChanges();
            }
        }

        public static BovineEntity GetById(Guid id)
        {
            using (var dbContext = new CUsersAntonSourceReposAplicacaowebDalDatabaseDatabase1MdfContext())
            {
                var _tbMatch = dbContext.TbMatches.Single(P => P.Id == id);

                BovineEntity match = MatchBuilder(_tbMatch);

                return match;

            }
        }
    }
}
