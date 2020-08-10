using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FrameworkBaseData.Models
{
    public class UserSetting : BaseModel
    {
        [Column(Order = 2)]
        public string Key { get; set; }

        [Column(Order = 3)]
        public string Value { get; set; }

        [Column(Order = 5)]
        public int Userid { get; set; }

        [Column(Order = 6)]
        public virtual User User { get; set; }
    }
}