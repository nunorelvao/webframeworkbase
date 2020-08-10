using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FrameworkBaseData.Models;

namespace FrameworkBaseData.Builders
{
    public class CountryMap
    {
        public CountryMap(EntityTypeBuilder<Country> entityBuilder)
        {
            entityBuilder.Property(t => t.Code).HasMaxLength(2).IsRequired();
            entityBuilder.Property(t => t.Code3).HasMaxLength(3);
            entityBuilder.Property(t => t.Extcode).HasMaxLength(2);
            entityBuilder.Property(t => t.Domain).HasMaxLength(10);
            entityBuilder.Property(t => t.Number).HasMaxLength(3);
        }
    }
}