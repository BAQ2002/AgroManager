using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MODEL;

namespace INFRA.Persistences
{
    public class SwineBeefMemberConfig : IEntityTypeConfiguration<SwineBeefMember>
    {
        public void Configure(EntityTypeBuilder<SwineBeefMember> entityBuilder)
        {
            entityBuilder.ToTable("SwineBeefMembers");

        }
    }
}
