using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FrameworkBaseData.Models
{
    public class Person : BaseModel
    {
        [Column(Order = 2)]
        public string Firstname { get; set; }

        [Column(Order = 3)]
        public string Lastname { get; set; }

        [Column(Order = 4)]
        public string Middlename { get; set; }

        [Column(Order = 5)]
        public DateTime? Borndate { get; set; }

        [Column(Order = 6)]
        public int? Languageid { get; set; }

        [Column(Order = 7)]
        public virtual Language Language { get; set; }

        [Column(Order = 8)]
        [InverseProperty("Person")]
        public virtual User User { get; set; }

        [InverseProperty("Person")]
        public virtual IList<PersonAddress> Personaddresses { get; set; }

        [InverseProperty("Person")]
        public virtual IList<PersonDocument> Persondocuments { get; set; }

        [InverseProperty("Person")]
        public virtual IList<PersonContact> Personcontacts { get; set; }
    }
}