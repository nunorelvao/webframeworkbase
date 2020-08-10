using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FrameworkBaseData.Models;

namespace FrameworkBaseData.Builders
{
    public class CurrencyMap
    {
        public CurrencyMap(EntityTypeBuilder<Currency> entityBuilder)
        {
            entityBuilder.Property(t => t.Code).HasMaxLength(3).IsRequired();
            entityBuilder.Property(t => t.Name).HasMaxLength(255).IsRequired();
            entityBuilder.Property(t => t.Symbol).HasMaxLength(3).IsRequired();
            entityBuilder.Property(t => t.SymbolNative).HasMaxLength(10);
            entityBuilder.Property(t => t.Rounding).IsRequired();
            entityBuilder.Property(t => t.DecimalDigits).IsRequired();
        }
    }
}