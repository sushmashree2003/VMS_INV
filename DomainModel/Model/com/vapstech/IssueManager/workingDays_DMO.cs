using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Master_WorkingDays")]
    public class workingDays_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMMWD_Id { get; set; }
        public long MI_Id { get; set; }
        public long FOMD_Id { get; set; }
        public decimal? ISMMWD_Hours{ get; set; }
        public bool ISMPRTY_ActiveFlag{ get; set; }
        public long ISMMWD_CreatedBy{ get; set; }
        public long ISMMWD_UpdatedBy{ get; set; }


    }
}
