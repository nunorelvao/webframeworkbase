using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FrameworkBaseData.Models;

namespace FrameworkBaseData.Builders
{
    public class PersonMap
    {
        public PersonMap(EntityTypeBuilder<Person> entityBuilder)
        {
            entityBuilder.Property(t => t.Firstname).HasMaxLength(50).IsRequired();
            entityBuilder.Property(t => t.Lastname).HasMaxLength(50).IsRequired();
            entityBuilder.Property(t => t.Middlename).HasMaxLength(50);
            //entityBuilder.HasOne(u => u.User).WithOne("User").HasForeignKey<Person>(u=>  u.Userid);

        }
    }
}