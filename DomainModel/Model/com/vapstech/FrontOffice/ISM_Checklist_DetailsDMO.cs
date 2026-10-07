using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("ISM_Checklist_Details")]
    public class ISM_Checklist_DetailsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMCLD_Id { get; set; }
        public long ISMCLT_Id { get; set; }
        public string ISMCL_Name { get; set; }
        public long ISMCL_UserId { get; set; }
        public bool ISMCL_ActiveFlag { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        public long HRMF_Id { get; set; }
        public long ISMCLD_Qty { get; set; }
    }
}     
     
















       
