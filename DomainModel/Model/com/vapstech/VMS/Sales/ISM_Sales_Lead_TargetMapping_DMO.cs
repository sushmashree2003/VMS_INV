using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sales_Lead_TargetMapping_New")]
    public class ISM_Sales_Lead_TargetMapping_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ISMSLETARM_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMMTCAT_Id { get; set; }
        public long ISMSLETARM_Numbers { get; set; }
        public string ISMSLETARM_Periodicity { get; set; }
        public long ISMSLETARM_ApprovedBy { get; set; }
        public bool ISMSLETARM_ActiveFlag { get; set; }
        public DateTime? ISMSLETARM_CreatedDate { get; set; }
        public DateTime? ISMSLETARM_UpdatedDate { get; set; }
        public long ISMSLETARM_CreatedBy { get; set; }
        public long ISMSLETARM_UpdatedBy { get; set; }


       
    }
}
