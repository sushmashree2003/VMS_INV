using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("VMS_TADA_Master_City")]
    public class VMSTADAMasterCityDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VTADAMC_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public string VTADAMC_FromState { get; set; }
        public string VTADAMC_ToState { get; set; }
        public long VTADAMC_CreatedBy { get; set; }
        public long VTADAMC_UpdatedBy { get; set; }
        public DateTime VTADAMC_CreatedDate { get; set; }
        public DateTime VTADAMC_UpdatedDate { get; set; }
        public decimal VTADAMC_Amount { get; set; }
        public long MI_Id { get; set; }
        public long VTADAMT_Id { get; set; }
        public long VTADAMD_Id { get; set; }
        public long VRMMDT_Id { get; set; }
       



    }
}
