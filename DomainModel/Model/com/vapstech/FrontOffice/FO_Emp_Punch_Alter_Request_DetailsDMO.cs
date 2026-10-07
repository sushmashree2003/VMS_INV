using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("FO_Emp_Punch_Alter_Request_Details", Schema = "FO")]
    public class FO_Emp_Punch_Alter_Request_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FOEPARD_Id { get; set; }
        public long FOEPAR_Id { get; set; }
        public string FOEPARD_Time { get; set; }
        public string FOEPARD_ApprovedTime { get; set; }
        public string FOEPARD_InOutFlg { get; set; }
        public string FOEPARD_Remarks { get; set; }
        public bool? FOEPARD_ActiveFlg { get; set; }
        public bool? FOEPARD_ApprvedFlg { get; set; }
        public string FOEPARD_FilePath { get; set; }
        public DateTime? FOEPARD_CreatedDate { get; set; }
        public DateTime? FOEPARD_UpdatedDate { get; set; }
        public long? FOEPARD_CreatedBy { get; set; }
        public long? FOEPARD_UpdatedBy { get; set; }

    }
}
