using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{

    [Table("ISM_Master_TaskCategory_Employee")]
  public  class ISM_Master_TaskCategory_Employee_DMO
    {

[Key]
[DatabaseGenerated(DatabaseGeneratedOption.Identity)]


      public long ISMMTCATE_Id { get; set; }
        public long ISMMTCAT_Id { get; set; }
        public long HRME_Id { get; set; }
        public Decimal ISMMTCATE_TaskPercentage { get; set; }
        public bool ISMMTCATE_ActiveFlag { get; set; }
        public long ISMMTCATE_CreatedBy { get; set; }
        public long ISMMTCATE_UpdatedBy { get; set; }
        public bool? ISMMTCATE_CompulsoryFlg { get; set; }
        public DateTime ISMMTCATE_CreatedDate { get; set; }
        public DateTime ISMMTCATE_UpdatedDate { get; set; }        

    }
}
