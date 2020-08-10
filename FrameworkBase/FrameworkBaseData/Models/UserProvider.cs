using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FrameworkBaseData.Models
{
    public class UserProvider : BaseModel
    {
        [Column(Order = 2)]
        public string ProviderName { get; set; }

        [Column(Order = 3)]
        public string ProviderDisplayName { get; set; }

        [Column(Order = 4)]
        public string ProviderKey { get; set; }

        [Column(Order = 5)]
        public int Userid { get; set; }

        [Column(Order = 6)]
        public virtual User User { get; set; }
    }
}