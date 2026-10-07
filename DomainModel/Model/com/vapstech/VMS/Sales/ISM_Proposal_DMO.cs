using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_PostSales_Signup")]
    public class ISM_Proposal_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ISMPSSUP_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime? ISMPSSUP_Date { get; set; }
        public string ISMPSSUP_TemplateName { get; set; }
        public string ISMPSSUP_Stauts { get; set; }
        public bool ISMPSSUP_ActiveFlag { get; set; }
        public long ISMPSSUP_CreatedBy { get; set; }
        public DateTime? ISMPSSUP_CreatedDate { get; set; }
        public long ISMPSSUP_UpdatedBy { get; set; }
        public DateTime? ISMPSSUP_UpdatedDate { get; set; }
        public long ISMCLTPRMP_AcademicYear { get; set; }
        public string ISMPSSUP_EndUSer { get; set; }
        public long ISMPSSUP_TemplateID { get; set; }
        public List<ISM_ProposalDMO_Files> ISM_Proposal_FilesDMO { get; set; }




    }
}
