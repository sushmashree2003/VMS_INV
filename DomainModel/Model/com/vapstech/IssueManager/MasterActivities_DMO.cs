using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Master_Activity")]
    public class MasterActivities_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public long ISMMAC_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMMAC_ActivityName { get; set; }
        public long HRMD_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public string ISMMAC_Desc { get; set; }
        public bool ISMMAC_ActiveFlag { get; set; }
        public long? ISMMAC_CreatedBy { get; set; }
        public long? ISMMAC_UpdatedBy { get; set; }

    }
}
