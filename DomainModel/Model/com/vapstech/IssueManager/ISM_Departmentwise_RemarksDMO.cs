using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Departmentwise_Remarks")]
    public class ISM_Departmentwise_RemarksDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMDR_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public string ISMDR_Remarks { get; set; }
        public bool ISMDR_ActiveFlag { get; set; }
        public long ISMDR_CreatedBy { get; set; }
        public long ISMDR_UptadedBy { get; set; }
        public bool ISMDR_ReasonFlag { get; set; }

    }
}
