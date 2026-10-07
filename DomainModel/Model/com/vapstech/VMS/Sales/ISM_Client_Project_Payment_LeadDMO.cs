using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Client_Project_Payment_Lead")]
    public class ISM_Client_Project_Payment_LeadDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMCLTPRPL_Id { get; set; }
        public long ISMSLE_Id { get; set; }
        public long ISMCLTPRP_Year { get; set; }
        public string ISMCLTPRP_InstallmentName { get; set; }
        public DateTime? ISMCLTPRP_PaymentDate { get; set; }
        public decimal? ISMCLTPRP_InstallmentAmt { get; set; }
        public bool ISMCLTPRP_ActiveFlag { get; set; }
        public long ISMCLTPRP_CreatedBy { get; set; }
        public long ISMCLTPRP_UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long FTI_Id { get; set; }
        public string ISMCLTPRP_PaymentStatus { get; set; }
        public decimal? ISMCLTPRP_BalanceAmt { get; set; }
        public decimal? ISMCLTPRP_ExcessAmt { get; set; }
        public long FMI_Id { get; set; }
        public decimal ISMCLTPRPL_Per { get; set; }
        public long? ISMSLET_Id { get; set; }

       

    }
}
