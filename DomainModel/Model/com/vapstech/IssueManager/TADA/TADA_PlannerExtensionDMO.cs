using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("TADA_PlannerExtension")]
    public class TADA_PlannerExtensionDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TADAPLE_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime TADAPLE_FromDate { get; set; }
        public DateTime TADAPLE_ToDate { get; set; }
        public string TADAPLE_Reason { get; set; }
        public bool TADAPLE_ActiveFlag { get; set; }
        public string TADAPLE_Statusflag { get; set; }
        public long TADAPLE_CreatedBy { get; set; }
        public long TADAPLE_UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }      

    }
}
