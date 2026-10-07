using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("ISM_Checklist_Type")]
    public class ISM_Checklist_TypeDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMCLT_Id { get; set; }
        public string ISMCLT_Name { get; set; }
        public long ISMCLT_UserId { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        public bool ISM_ActiveFlag { get; set; }
        public long ISMCLT_CreatedBy { get; set; }
        public long ISMCLT_UpdatedBy { get; set; }
    }
}     
     
















       
