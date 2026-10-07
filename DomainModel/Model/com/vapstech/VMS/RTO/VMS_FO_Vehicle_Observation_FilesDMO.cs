using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.RTO
{
    [Table("VMS_FO_Vehicle_Observation_Files")]
    public class VMS_FO_Vehicle_Observation_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMSFOVOBFL_Id { get; set; }
        public long VMSFOVOB_Id { get; set; }
        public string VMSFOVOBFL_FileName { get; set; }
        public string VMSFOVOBFL_FilePath { get; set; }
        public bool VMSFOVOBFL_ActiveFlag { get; set; }
        public DateTime? VMSFOVOBFL_CreatedDate { get; set; }
        public DateTime? VMSFOVOBFL_UpdatedDate { get; set; }
        public long VMSFOVOBFL_CreatedBy { get; set; }
        public long VMSFOVOBFL_UpdatedBy { get; set; }
        public string VMSFOVOBFL_Filedesc { get; set; }

    }
}
