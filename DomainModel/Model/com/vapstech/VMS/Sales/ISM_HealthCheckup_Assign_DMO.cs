using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_HealthCheckup_assign")]
    public class ISM_HealthCheckup_Assign_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMHCA_Id { get; set; }
        public long ISMMCLTPR_Id { get; set; }
        public string ISMHCA_Quarter { get; set; }
        public bool ISMHCA_ActiveFlag { get; set; }
        public long ISMHCA_CreatedBy { get; set; }
        public long ISMHCA_UpdatedBy { get; set; }
        public DateTime? ISMHCA_CreatedDate { get; set; }
        public DateTime? ISMHCA_UpdatedDate { get; set; }
        public DateTime? ISMHCA_HealthCheckupDate { get; set; }
        public long? ISMHCA_AcademicYear { get; set; }
    }
}
