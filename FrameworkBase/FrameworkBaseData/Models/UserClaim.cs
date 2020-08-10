using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FrameworkBaseData.Models
{
    public class UserClaim : BaseModel
    {
        [Column(Order = 2)]
        public string ClaimName { get; set; }

        [Column(Order = 3)]
        public string ClaimValue { get; set; }

        [Column(Order = 4)]
        public string ClaimType { get; set; }

        [Column(Order = 5)]
        public int Userid { get; set; }

        [Column(Order = 6)]
        public virtual User User { get; set; }
    }
}