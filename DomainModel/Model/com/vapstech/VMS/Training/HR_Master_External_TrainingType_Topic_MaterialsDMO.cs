using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("HR_Master_External_TrainingType_Topic_Materials")]
   public class HR_Master_External_TrainingType_Topic_MaterialsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRMETRTYTOPM_Id { get; set; }
        public long HRMTT_Id { get; set; }
        public string HRMETRTYTOPM_StudyMaterials { get; set; }
        public string HRMETRTYTOPM_StudyMaterialsDesc { get; set; }
        public bool HRMETRTYTOPM_ActiveFlag { get; set; }
        public long HRMETRTYTOPM_CreatedBy { get; set; }
        public long HRMETRTYTOPM_UpdatedBy { get; set; }
        public DateTime HRMETRTYTOPM_CreatedDate { get; set; }
        public DateTime HRMETRTYTOPM_UpdatedDate { get; set; }
    }
}
