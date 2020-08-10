using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FrameworkBaseData.Models;

namespace FrameworkBaseData.Builders
{
    public class AddressTypeMap
    {
        public AddressTypeMap(EntityTypeBuilder<AddressType> entityBuilder)
        {
            entityBuilder.Property(t => t.Code).HasMaxLength(10).IsRequired();
            entityBuilder.Property(t => t.Name).HasMaxLength(255).IsRequired();
        }
    }
}