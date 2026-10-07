using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("ISM_Checklist_Approval")]
    public class ISM_Checklist_ApprovalDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMCLA_Id { get; set; }
        public long ISMCLD_Id { get; set; }
        public DateTime? ISMCLT_Date { get; set; }
        public long ISMCLT_UserId { get; set; }
        public string ISMCLT_Remarks { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        public long ISMCLT_CreatedBy { get; set; }
        public long ISMCLT_UpdatedBy { get; set; }
        public decimal? ISMCLD_MorningQty { get; set; }
        public decimal? ISMCLD_EveningQty { get; set; }
        public string ISMCLT_FilePath { get; set; }
    }
}     
     
















       
