using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FrameworkBaseData.Models;

namespace FrameworkBaseData.Builders
{
    public class UserMap
    {
        public UserMap(EntityTypeBuilder<User> entityBuilder)
        {
            entityBuilder.Property(t => t.Username).HasMaxLength(50).IsRequired();
            entityBuilder.Property(t => t.Userpassword).HasMaxLength(255).IsRequired();
        }
    }
}