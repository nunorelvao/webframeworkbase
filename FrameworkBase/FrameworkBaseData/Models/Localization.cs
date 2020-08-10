using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FrameworkBaseData.Models
{
    public class Localization : BaseModel
    {
        [Column(Order = 2)]
        public string Localizationkey { get; set; }

        [Column(Order = 3)]
        public string Localizationvalue { get; set; }

        [Column(Order = 4)]
        public int Languageid { get; set; }

        [Column(Order = 5)]
        public virtual Language Language { get; set; }
    }
}