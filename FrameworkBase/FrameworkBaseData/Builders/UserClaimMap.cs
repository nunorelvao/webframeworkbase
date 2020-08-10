using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FrameworkBaseData.Models;

namespace FrameworkBaseData.Builders
{
    public class UserClaimMap
    {
        public UserClaimMap(EntityTypeBuilder<UserClaim> entityBuilder)
        {
            entityBuilder.Property(t => t.ClaimName).HasMaxLength(255).IsRequired();
            entityBuilder.Property(t => t.ClaimValue).HasMaxLength(500).IsRequired();
            entityBuilder.Property(t => t.ClaimType).HasMaxLength(255).IsRequired();
        }
    }
}