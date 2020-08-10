using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FrameworkBaseData.Models;

namespace FrameworkBaseData.Builders
{
    public class DocumentTypeMap
    {
        public DocumentTypeMap(EntityTypeBuilder<DocumentType> entityBuilder)
        {
            entityBuilder.Property(t => t.Code).HasMaxLength(255).IsRequired();
            entityBuilder.Property(t => t.Name).HasMaxLength(255).IsRequired();
        }
    }
}