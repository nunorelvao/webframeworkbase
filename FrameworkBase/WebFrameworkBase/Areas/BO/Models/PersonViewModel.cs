using FrameworkBaseData.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebFrameworkBase.Areas.BO.Models
{
    public class PersonViewModel
    {
        public PersonViewModel()
        {
            LanguagesSelectItems = new List<SelectListItem>();
        }

        [Display(Name = "ID")]
        public int PersonId { get; set; }

        [Display(Name = "First Name")]
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [MaxLength(50)]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        [MaxLength(152)]
        public string FullName { get; set; }

        [Display(Name = "Borndate")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}", NullDisplayText = "no value")]
        public DateTime? Borndate { get; set; }

        [Display(Name = "Language Code")]
        [MaxLength(152)]
        public string LanguageCode { get; set; }

        [Display(Name = "LanguageID")]
        public int? LanguageId { get; set; }

        public List<SelectListItem> LanguagesSelectItems { get; set; }

        [Display(Name = "User Color BG")]
        [MaxLength(50)]
        public string UserColorBG { get; set; }

        //[Display(Name = "Adresses")]
        //public List<PersonAddressViewModel> PersonaddressesList { get; set; }

        //[Display(Name = "Documents")]
        //public List<PersonDocumentViewModel> PersondocumentsList { get; set; }



        //########### USER PART ###########
        [Display(Name = "UserID")]
        public int? UserId { get; set; }

    }
}