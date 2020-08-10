using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FrameworkBaseData.Models
{
    public class Country : BaseModel
    {
        [Column(Order = 2)]
        public string Code { get; set; }

        [Column(Order = 3)]
        public string Code3 { get; set; }

        [Column(Order = 4)]
        public string Extcode { get; set; }

        [Column(Order = 5)]
        public int Number { get; set; }

        [Column(Order = 6)]
        public string Name { get; set; }

        [Column(Order = 7)]
        public string Domain { get; set; }

        [Column(Order = 8)]
        public int? Languageid { get; set; }

        [Column(Order = 9)]
        public virtual Language Language { get; set; }

        [Column(Order = 10)]
        public int? Currencyid { get; set; }

        [Column(Order = 11)]
        public virtual Currency Currency { get; set; }
    }
}