using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRA.Persistences
{
    public class BovinePastureMemberConfig : IEntityTypeConfiguration<BovinePastureMember>
    {
        public void Configure(EntityTypeBuilder<BovinePastureMember> entityBuilder)
        {
            entityBuilder.ToTable("BovinePastureMembers");

            entityBuilder.HasKey(x => x.Id); //Chave primaria
        }
    }
}
