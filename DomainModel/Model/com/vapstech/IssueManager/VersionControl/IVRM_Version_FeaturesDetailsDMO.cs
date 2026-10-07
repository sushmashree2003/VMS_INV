using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.VersionControl
{
    [Table("IVRM_Version_FeaturesDetails")]
    public class IVRM_Version_FeaturesDetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMVFD_Id { get; set; }
        public long IVRMVT_Id { get; set; }
        public string IVRMVFD_Features { get; set; }
        public long ISMTCR_Id { get; set; }
        public string IVRMVFD_Status { get; set; }
        public string IVRMVFD_Remarks { get; set; }
        public bool IVRMVFD_ActiveFlag { get; set; }
        public string IVRMVFD_Test_Case { get; set; }
        public string IVRMVFD_Test_Case_File_Name { get; set; }
        public bool IVRMVFD_ApprovedFlag { get; set; }
        public DateTime Createddate { get; set; }
        public DateTime Updateddate { get; set; }
        public long Createdby { get; set; }
        public long Updatedby { get; set; }
    }
}
