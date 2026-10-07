using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("VMS_MODULE_FEEDBACK")]
    public class ModuleFeedbackDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long VMF_ID {  get; set; }
        public long IVRMM_Id {get; set; }
        public string VMF_ModuleName {get; set; }
        public long MI_Id {get; set; }
        public long HRME_Id {get; set; }
        public long VMF_Rating {get; set; }
          public string VMF_Remarks {get; set; }
          public bool VMF_ActiveFlag {get; set; }
          public DateTime VMF_CreatedDate {get; set; }
          public DateTime VMF_UpdatedDate  { get; set; }
          public long VMF_CreatedBy {get; set; }
          public long VMF_UpdatedBy {get; set; }
          public string ISMTCR_TaskNo { get; set; }
    }
}
