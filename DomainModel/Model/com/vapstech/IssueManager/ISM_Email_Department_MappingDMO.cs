using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Email_Department_Mapping")]
    public class ISM_Email_Department_MappingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISM_EId { get; set; }
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public string ISM_Email_CC { get; set; }
        public string ISM_Email_BCC { get; set; }
    }
}
