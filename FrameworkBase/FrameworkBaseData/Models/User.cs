using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FrameworkBaseData.Models
{
    public class User : BaseModel
    {
        [Column(Order = 2)]
        public string Username { get; set; }

        [Column(Order = 3)]
        public string Userpassword { get; set; }

        [Column(Order = 4)]
        public string Userpasswordhash { get; set; }

        [Column(Order = 5)]
        public int Roleid { get; set; }

        [Column(Order = 6)]
        public virtual Role Role { get; set; }

        [Column(Order = 7)]
        [ForeignKey("Person")]
        public int? Personid { get; set; }

        [Column(Order = 8)]
        public virtual Person Person { get; set; }

        [Column(Order = 9)]
        public virtual IList<UserSetting> UserSettings { get; set; }
    }
}