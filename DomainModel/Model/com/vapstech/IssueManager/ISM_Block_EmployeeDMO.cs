using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Block_Employee")]
    public class ISM_Block_EmployeeDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMBE_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime? ISMBE_BlockDate { get; set; }
        public DateTime? ISEBE_UnblockDate { get; set; }
        public string ISMBE_Reason { get; set; }
        public bool ISMBE_BlockFlg { get; set; }
        public bool ISMBE_ActiveFlg { get; set; }
        public long ISMBE_CreatedBy { get; set; }
        public long ISMBE_UpdatedBy { get; set; }
        public long? ISMEMN_ID { get; set; }
        public bool ISM_PlannerBlockFlg { get; set; }
        

    }
}
