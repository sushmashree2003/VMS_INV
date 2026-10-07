using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_EMPLOYEE_MEMO_NOTICE_TASKS")]
    public class ISM_EMPLOYEE_MEMO_NOTICE_TASKS 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMEMNT_ID { get; set; }
        public long ISMTPLTA_Id { get; set; }
        public long ISMEMN_ID { get; set; }       
        public DateTime Createddate { get; set; }
        public DateTime Updateddate { get; set; } 

    }
}
