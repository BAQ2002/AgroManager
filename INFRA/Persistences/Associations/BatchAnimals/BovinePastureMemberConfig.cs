using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MODEL;

namespace INFRA.Persistences
{
    public class BovinePastureMemberConfig : IEntityTypeConfiguration<BovinePastureMember>
    {
        public void Configure(EntityTypeBuilder<BovinePastureMember> entityBuilder)
        {
            entityBuilder.ToTable("BovinePastureMembers");

        }
    }
}
