using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FrameworkBaseData.Models;

namespace FrameworkBaseData.Builders
{
    public class UserSettingMap
    {
        public UserSettingMap(EntityTypeBuilder<UserSetting> entityBuilder)
        {
            entityBuilder.Property(t => t.Key).HasMaxLength(255).IsRequired();
            entityBuilder.Property(t => t.Value).HasMaxLength(500).IsRequired();
            entityBuilder.HasIndex(t => new { t.Userid, t.Key }).IsUnique(true);
        }
    }
}