using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebFrameworkBase.Models.HomeModels
{
    public class ContactModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email To")]
        public string EmailTo { get; set; }

        [Required]
        [Display(Name = "Body Text")]
        public string BodyText { get; set; }
    }
}