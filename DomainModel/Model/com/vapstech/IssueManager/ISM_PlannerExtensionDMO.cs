using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_PlannerExtension")]
    public class ISM_PlannerExtensionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMPLE_Id { get; set; }
        public long HRME_Id { get; set; }
        public string ISMPLE_Reason { get; set; }
        public DateTime ISMPLE_FromDate { get; set; }    
        public DateTime ISMPLE_ToDate { get; set; }    
        public bool ISMPLE_ActiveFlg { get; set; }
        public long ISMPLE_CreatedBy { get; set; }
        public long ISMPLE_UpdatedBy { get; set; }
    }
}
