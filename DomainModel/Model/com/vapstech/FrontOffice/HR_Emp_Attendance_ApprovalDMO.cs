using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("HR_Emp_Attendance_Approval")]
    public class HR_Emp_Attendance_ApprovalDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HREAA_Id { get; set; }
        public long VMSCRT_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public string HREAA_Remarks { get; set; }
        public DateTime HREAA_Date { get; set; }
        public long HREAA_SanctionLevel { get; set; }
        public bool HREAA_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long HREAA_CreatedBy { get; set; }
        public long HREAA_UpdatedBy { get; set; }
        public string HREAA_ApprovalFlg { get; set; }
        public decimal HREAA_LOP { get; set; }

    }
}
