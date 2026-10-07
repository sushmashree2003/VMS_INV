using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("OT_Master_OE_Questions_Files")]
    public class OT_Master_OE_Questions_FilesDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
 
        public long OTMOEQF_Id { get; set; }
        public long OTMOEQ_Id { get; set; }
        public string OTMOEQF_FileName { get; set; }
        public string OTMOEQF_FilePath { get; set; }
        public bool OTMOEQF_ActiveFlag { get; set; }
     
    }
}