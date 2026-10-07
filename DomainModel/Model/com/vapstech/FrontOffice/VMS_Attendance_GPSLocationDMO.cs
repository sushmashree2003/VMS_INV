using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("VMS_Attendance_GPSLocation")]
    public class VMS_Attendance_GPSLocationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMSATGPSL_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long ISMSLE_Id { get; set; }
        public DateTime? VMSATGPSL_DateTime { get; set; }
        public string VMSATGPSL_Address { get; set; }
        public string VMSATGPSL_LATLONGValue { get; set; }
        public string VMSATGPSL_Remarks { get; set; }
        public bool VMSATGPSL_ActiveFlg { get; set; }
        public DateTime? VMSATGPSL_CreatedDate { get; set; }
        public DateTime? VMSATGPSL_UpdatedDate { get; set; }
        public long VMSATGPSL_CreatedBy { get; set; }
        public long VMSATGPSL_UpdatedBy { get; set; }
    }
}
