using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FrameworkBaseData.Models;

namespace FrameworkBaseData.Builders
{
    public class PersonDocumentMap
    {
        public PersonDocumentMap(EntityTypeBuilder<PersonDocument> entityBuilder)
        {
            entityBuilder.Property(t => t.Value).HasMaxLength(255).IsRequired();
        }
    }
}