using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.QRcode_Activity
{

    [Table("HR_Master_Area")]
    public class HRMasterAreaDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRMAR_Id{get;set;}
        public long MI_Id { get; set; }
        public string HRMAR_AreaName { get; set; }
        public string HRMA_QRCODEPath { get; set; }
        public string HRMAR_AreaCode { get; set; }
        public long HRMB_Id { get; set; }
        public long HRMF_Id { get; set; }
        public String HRMAR_TypeFlag { get; set; }
        public string HRMAR_QRCode { get; set; }
        public bool HRMAR_ActiveFlag { get; set; }
        public DateTime HRMAR_CreatedDate { get; set; }
        public DateTime HRMAR_UpdatedDate { get; set; }
        public long HRMAR_CreatedBy { get; set; }
        public long HRMAR_UpdatedBy { get; set; }
        
    }
}
