using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.RTO
{
    [Table("VMS_Vehicle_Keys")]
    public class VMS_Vehicle_KeysDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMSVKEYS_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long TRMV_Id { get; set; }
        public DateTime? VMSVKEYS_DateTime { get; set; }
        public string VMSVKEYS_KeyReceivedHandedOverFlg { get; set; }
        public bool VMSVKEYS_ActiveFlag { get; set; }
        public DateTime? VMSVKEYS_CreatedDate { get; set; }
        public DateTime? VMSVKEYS_UpdatedDate { get; set; }
        public long VMSVKEYS_CreatedBy { get; set; }
        public long VMSVKEYS_UpdatedBy { get; set; }
        public string VMSVKEYS_Time { get; set; }
        public string VMSVKEYS_RecivedTime { get; set; }
        public DateTime? VMSVKEYS_RecivedDate { get; set; }
        public string VMSVKEYS_Remarks { get; set; }
        public string VMSVKEYS_KeyHandedOverFlg { get; set; }
        public long? VMSVKEYS_OdometerReading { get; set; }

    }
}
