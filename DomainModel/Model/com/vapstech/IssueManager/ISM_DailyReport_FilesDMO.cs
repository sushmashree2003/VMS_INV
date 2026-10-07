using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_DailyReport_Files")]
    public class ISM_DailyReport_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMDRPTFL_Id { get; set; }
        public long ISMMTCATCL_Id { get; set; }
        public long ISMDRPT_Id { get; set; }
        public string ISMDRPTFL_FileName { get; set; }
        public string ISMDRPTFL_RefNo { get; set; }
        public string ISMDRPTFL_Remarks { get; set; }
        public string ISMDRPTFL_FilePath { get; set; }
        public bool ISMDRPT_ActiveFlg { get; set; }
        public DateTime ISMDRPT_CreatedDate { get; set; }
        public DateTime ISMDRPT_UpdatedDate { get; set; }
        public long ISMDRPT_CreatedBy { get; set; }
        public long ISMDRPT_UpdatedBy { get; set; }
    }
}
