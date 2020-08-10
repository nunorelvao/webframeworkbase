using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FrameworkBaseData.Models
{
    public class BaseModel
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(Order = 100, TypeName = "datetime")]
        public DateTime Base_Addeddate { get; set; }

        [Column(Order = 101, TypeName = "datetime")]
        public DateTime Base_Modifieddate { get; set; }

        [Column(Order = 102, TypeName = "nvarchar(50)")]
        public string Base_Username { get; set; }

        [Column(Order = 103)]
        [MaxLength(255)]
        public string Base_Ipaddress { get; set; }

        [Column(Order = 104)]
        public bool? Base_Enabled { get; set; }
    }
}