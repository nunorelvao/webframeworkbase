using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FrameworkBaseData.Models;

namespace FrameworkBaseData.Builders
{
    public class LocalizationMap
    {
        public LocalizationMap(EntityTypeBuilder<Localization> entityBuilder)
        {
            entityBuilder.Property(t => t.Localizationkey).HasMaxLength(255).IsRequired();
            entityBuilder.Property(t => t.Languageid).IsRequired().HasMaxLength(10);
            entityBuilder.Property(t => t.Localizationvalue);
        }
    }
}