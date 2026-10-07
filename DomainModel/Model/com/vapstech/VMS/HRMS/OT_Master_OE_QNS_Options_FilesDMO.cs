using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("OT_Master_OE_QNS_Options_Files")]
    public class OT_Master_OE_QNS_Options_FilesDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
 
        public long OTMOEQOAF_Id { get; set; }
        public long OTMOEQOA_Id { get; set; }
        public string OTMOEQOAF_FileName { get; set; }
        public string OTMOEQOAF_FilePath { get; set; }
        public bool OTMOEQOAF_ActiveFlag { get; set; }
    }
}