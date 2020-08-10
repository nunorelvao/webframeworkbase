using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FrameworkBaseData.Models
{
    public class Currency : BaseModel
    {
        [Column(Order = 2)]
        public string Code { get; set; }

        [Column(Order = 3)]
        public string Name { get; set; }

        [Column(Order = 4)]
        public string Symbol { get; set; }

        [Column(Order = 5)]
        public string SymbolNative { get; set; }

        [Column(Order = 6)]
        public int DecimalDigits { get; set; }

        [Column(Order = 7, TypeName = "decimal(18,2)")]
        public decimal Rounding { get; set; }
    }
}