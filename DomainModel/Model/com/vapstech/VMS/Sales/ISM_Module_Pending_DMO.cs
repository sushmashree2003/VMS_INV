using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Module_Pending")]
    public class ISM_Module_Pending_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMMP_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long IVRMM_Id { get; set; }
        public bool ISMMP_ActiveFlag { get; set; }
        public long ISMMP_CreatedBy { get; set; }
        public long ISMMP_UpdatedBy { get; set; }
        public DateTime? ISMMP_CreatedDate { get; set; }
        public DateTime? ISMMP_UpdatedDate { get; set; }
        public DateTime? ISMMP_Date { get; set; }
        public long? ISMMP_AcademicYear { get; set; }
    }
}
