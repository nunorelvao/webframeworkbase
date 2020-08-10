using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FrameworkBaseData.Models
{
    public class PersonDocument : BaseModel
    {
        [Column(Order = 2)]
        public string Value { get; set; }

        [Column(Order = 3)]
        public DateTime Issuedate { get; set; }

        [Column(Order = 4)]
        public DateTime Expiredate { get; set; }

        [Column(Order = 5)]
        public string Issuelocation { get; set; }

        [Column(Order = 6)]
        public int Personid { get; set; }

        [Column(Order = 7)]
        public virtual Person Person { get; set; }

        [Column(Order = 8)]
        public int Documenttypeid { get; set; }

        [Column(Order = 9)]
        public virtual DocumentType Documenttype { get; set; }
    }
}