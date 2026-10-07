using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.LeaveManagement
{
    [Table("HR_Leave_Restriction_Dates")]
    public class HR_Leave_Restriction_DatesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long  HRLRD_Id { get; set; }
        public long? HRMD_Id { get; set; }
        public long? HRME_Id { get; set; }
        public DateTime HRLRD_FromDate { get; set; }
        public DateTime HRLRD_ToDate { get; set; }
        public string HRLRD_Remarks { get; set; }
        public bool HRLRD_ActiveFlg { get; set; }
        public long  HRLRD_CreatedBy{ get; set; }
        public long HRLRD_UpdatedBy { get; set; }
        public DateTime HRLRD_CreatedDate { get; set; }
        public DateTime HRLRD_UpdatedDate { get; set; }
    }
}
