using FrameworkBaseData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebFrameworkBase.Areas.BO.Models
{
    public class CountryViewModel
    {
        [Display(Name = "ID")]
        public int CountryId { get; set; }
        [Display(Name = "Name")]
        public string CountryName { get; set; }
        [Display(Name = "Iso")]
        public string CountryIsoCode { get; set; }
        [Display(Name = "Iso 3")]
        public string CountryIsoCode3 { get; set; }
    }
}