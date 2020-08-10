using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FrameworkBaseData.Models;

namespace FrameworkBaseData.Builders
{
    public class PersonAddressMap
    {
        public PersonAddressMap(EntityTypeBuilder<PersonAddress> entityBuilder)
        {
            entityBuilder.Property(t => t.Adressline1).HasMaxLength(50).IsRequired();
            entityBuilder.Property(t => t.Adressline2).HasMaxLength(50);
            entityBuilder.Property(t => t.Adressline3).HasMaxLength(50);
            entityBuilder.Property(t => t.City).HasMaxLength(30);
            entityBuilder.Property(t => t.Region).HasMaxLength(30);
            entityBuilder.Property(t => t.State).HasMaxLength(30);
            entityBuilder.Property(t => t.Postalcode).HasMaxLength(10);
        }
    }
}