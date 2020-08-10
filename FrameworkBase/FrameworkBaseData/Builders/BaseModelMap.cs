using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using FrameworkBaseData.Models;

namespace FrameworkBaseData.Builders
{
    public class BaseModelMap
    {
        public BaseModelMap(EntityTypeBuilder<BaseModel> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Base_Ipaddress).HasMaxLength(255);
            entityBuilder.Property(t => t.Base_Username).HasMaxLength(50);
            entityBuilder.Property(t => t.Base_Enabled).IsRequired(true);
        }
    }
}