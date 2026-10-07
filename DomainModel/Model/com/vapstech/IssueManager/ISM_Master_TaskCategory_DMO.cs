using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Master_TaskCategory")]
   public class ISM_Master_TaskCategory_DMO
    { 
     [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long ISMMTCAT_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public string ISMMTCAT_TaskCategoryName { get; set; }
        public decimal ISMMTCAT_TaskPercentage { get; set; }
        public string ISMMTCAT_TCRemarks { get; set; }
        public bool ISMMTCAT_ActiveFlag { get; set; }
        public long ISMMTCAT_CreatedBy { get; set; }
        public long ISMMTCAT_UpdatedBy { get; set; }
        public DateTime ISMMTCAT_CreatedDate { get; set; }
        public DateTime ISMMTCAT_UpdatedDate { get; set; }
        public string ISMMTCAT_EachTaskMaxDuration { get; set; }
        public string ISMMTCAT_DurationFlg { get; set; }
        public bool ISMMTCAT_CompulsoryFlg { get; set; }
    }
}
