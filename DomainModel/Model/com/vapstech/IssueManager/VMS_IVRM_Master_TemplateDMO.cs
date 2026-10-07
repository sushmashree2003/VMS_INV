using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("VMS_IVRM_Master_Template")]
    public class VMS_IVRM_Master_TemplateDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         public long VMS_IVRMTemplate_Id { get; set; }
         public string VMS_IVRMTemplate_Name { get; set; }
         public bool? VMS_IVRMTemplate_ActiveFlag { get; set; }
    }
}
