using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.RTO
{
    [Table("VMS_FO_Vehicle_Observation")]
    public class VMS_FO_Vehicle_ObservationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMSFOVOB_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long TRMV_Id { get; set; }
        public DateTime? VMSFOVOB_DateTime { get; set; }
        public bool VMSFOVOB_AnyDamageFlg { get; set; }
        public string VMSFOVOB_DamageDescription { get; set; }
        public bool VMSFOVOB_ActiveFlag { get; set; }
        public DateTime? VMSFOVOB_CreatedDate { get; set; }
        public DateTime? VMSFOVOB_UpdatedDate { get; set; }
        public long VMSFOVOB_CreatedBy { get; set; }
        public long VMSFOVOB_UpdatedBy { get; set; }
        public  List<VMS_FO_Vehicle_Observation_FilesDMO> VMS_FO_Vehicle_Observation_FilesDMO { get; set; }
    }
}
