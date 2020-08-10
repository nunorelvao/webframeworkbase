using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FrameworkBaseData.Models
{
    public class PersonAddress : BaseModel
    {
        [Column(Order = 2)]
        public string Adressline1 { get; set; }

        [Column(Order = 3)]
        public string Adressline2 { get; set; }

        [Column(Order = 4)]
        public string Adressline3 { get; set; }

        [Column(Order = 5)]
        public string City { get; set; }

        [Column(Order = 6)]
        public string Region { get; set; }

        [Column(Order = 7)]
        public string State { get; set; }

        [Column(Order = 8)]
        public string Postalcode { get; set; }

        [Column(Order = 9)]
        public int Countryid { get; set; }

        [Column(Order = 10)]
        public virtual Country Country { get; set; }

        [Column(Order = 11)]
        public int Personid { get; set; }

        [Column(Order = 12)]
        public virtual Person Person { get; set; }

        [Column(Order = 13)]
        public int Addresstypeid { get; set; }

        [Column(Order = 14)]
        public virtual AddressType Addresstype { get; set; }
    }
}