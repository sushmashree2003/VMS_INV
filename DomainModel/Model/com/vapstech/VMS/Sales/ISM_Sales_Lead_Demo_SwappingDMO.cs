using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sales_Lead_Demo_Swapping")]
    public class ISM_Sales_Lead_Demo_SwappingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMSLEDMS_Id { get; set; }
        public long? ISMSLEDM_Id { get; set; }
        public long? MI_Id { get; set; }
        public long? ISMSLE_Id { get; set; }
        public long? HRME_BeforeDemoScheduledTo { get; set; }
        public long? HRME_AfterDemoScheduledTo { get; set; }
        public bool ISMSLEDMS_Activeflag { get; set; }
        public string ISMSLEDMS_DemoType { get; set; }
        public long? ISMSLEDMS_DemoScheduledBy { get; set; }
        public DateTime? ISMSLEDMS_CreatedDate { get; set; }
        public DateTime? ISMSLEDMS_UpdatedDate { get; set; }
        public long? ISMSLEDMS_CreatedBy { get; set; }
        public long? ISMSLEDMS_UpdatedBy { get; set; }

    }
}
