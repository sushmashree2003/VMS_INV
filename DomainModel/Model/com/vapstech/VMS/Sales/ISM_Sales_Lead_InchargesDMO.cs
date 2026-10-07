using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sales_Lead_Incharges")]
    public class ISM_Sales_Lead_InchargesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMSLEPRI_Id { get; set; }
        public long ISMSLE_Id { get; set; }
        public long? HRME_Id { get; set; }
        public bool ISMSLEPRI_ActiveFlag { get; set; }
        public DateTime? ISMSLEPRI_CreatedDate { get; set; }
        public DateTime? ISMSLEPRI_UpdatedDate { get; set; }
        public long ISMSLEPRI_CreatedBy { get; set; }
        public long ISMSLEPRI_UpdatedBy { get; set; }
    }
}
