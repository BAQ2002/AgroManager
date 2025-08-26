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
    public class SwineBeefMemberConfig : IEntityTypeConfiguration<SwineBeefMember>
    {
        public void Configure(EntityTypeBuilder<SwineBeefMember> entityBuilder)
        {
            entityBuilder.ToTable("SwineBeefMembers");
        }
    }
}
