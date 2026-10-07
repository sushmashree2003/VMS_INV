using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Postsales_enhancement_Elearning")]
    public class ISM_Postsales_enhancement_Elearning_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMPEE_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long ISMPEE_Enhancement { get; set; }
        public bool ISMPEE_ActiveFlag { get; set; }
        public long ISMPEE_CreatedBy { get; set; }
        public long ISMPEE_UpdatedBy { get; set; }
        public DateTime? ISMPEE_CreatedDate { get; set; }
        public DateTime? ISMPEE_UpdatedDate { get; set; }
        public long ISMPEE_Studentstrength { get; set; }


        public long? ISMPEE_AcademicYear { get; set; }
    }
}
