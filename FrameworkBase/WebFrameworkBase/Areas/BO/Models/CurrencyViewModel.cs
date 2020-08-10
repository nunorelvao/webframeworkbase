using FrameworkBaseData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebFrameworkBase.Areas.BO.Models
{
    public class CurrencyViewModel
    {
        [Display(Name = "ID")]
        public int CurrencyId { get; set; }

        [Display(Name = "Code")]
        public string CurrencyCode { get; set; }

        [Display(Name = "Name")]
        public string CurrencyName { get; set; }

        [Display(Name = "Symbol")]
        public string CurrencySymbol { get; set; }

        [Display(Name = "Native Symbol")]
        public string CurrencyNativeSymbol { get; set; }

        [Display(Name = "Decimal Digits")]
        public string CurrencyDecimalDigits { get; set; }

        [Display(Name = "Rounding")]
        public string CurrencyRounding { get; set; }
    }
}