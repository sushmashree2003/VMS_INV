using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_PUSH_NOTIFICATION_USER")]
    public class ISM_PUSH_NOTIFICATION_USERDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMPNU_ID { get; set; }
        public long ISMPNU_Userid { get; set; }
        public long MI_ID { get; set; }         
        public string ISMPNU_Tocken { get; set; }
    }
}
