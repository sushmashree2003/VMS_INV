using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("FO_Emp_Punch_Alter_Request", Schema = "FO")]
    public class FO_Emp_Punch_Alter_Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FOEPAR_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime? FOEPAR_PunchDate { get; set; }
        public string FOEPAR_Remarks { get; set; }
        public bool FOEPAR_ActiveFlg { get; set; }
        public string FOEPAR_ApprovalFlg { get; set; }
        public DateTime? FOEPAR_CreatedDate { get; set; }
        public DateTime? FOEPAR_UpdatedDate { get; set; }
        public long? FOEPAR_CreatedBy { get; set; }
        public long? FOEPAR_UpdatedBy { get; set; }
        public List<FO_Emp_Punch_Alter_Request_DetailsDMO> FO_Emp_Punch_Alter_Request_DetailsDMO { get; set; }

    }
}
