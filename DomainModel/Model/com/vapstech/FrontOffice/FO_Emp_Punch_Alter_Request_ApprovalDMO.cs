using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("FO_Emp_Punch_Alter_Request_Approval", Schema = "FO")]
    public class FO_Emp_Punch_Alter_Request_ApprovalDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FOEPARA_Id { get; set; }
        public long FOEPAR_Id { get; set; }
        public long HRME_Id { get; set; }
        public string FOEPARA_ApprovedTime { get; set; }
        public string FOEPARA_Remarks { get; set; }
        public bool FOEPARA_ActiveFlg { get; set; }
        public bool? FOEPARA_ApprovalFlg { get; set; }
        public DateTime? FOEPARA_CreatedDate { get; set; }
        public DateTime? FOEPARA_UpdatedDate { get; set; }
        public long? FOEPARA_CreatedBy { get; set; }
        public long? FOEPARA_UpdatedBy { get; set; }
    }
}
