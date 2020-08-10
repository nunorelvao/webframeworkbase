using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FrameworkBaseData.Models;

namespace FrameworkBaseData.Builders
{
    public class UserProviderMap
    {
        public UserProviderMap(EntityTypeBuilder<UserProvider> entityBuilder)
        {
            entityBuilder.Property(t => t.ProviderName).HasMaxLength(255).IsRequired();
            entityBuilder.Property(t => t.ProviderDisplayName).HasMaxLength(255).IsRequired();
            entityBuilder.Property(t => t.ProviderKey).HasMaxLength(500).IsRequired();
        }
    }
}