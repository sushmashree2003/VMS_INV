using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Client_Project_Payment")]
    public class ISM_Master_ClientProject_PaymentDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long ISMCLTPRP_Id { get; set; }
        public long ISMMCLTPR_Id { get; set; }
        public long? FTI_Id { get; set; }
        public long ISMCLTPRP_Year { get; set; }
        public string ISMCLTPRP_InstallmentName { get; set; }
        public decimal? ISMCLTPRP_InstallmentAmt { get; set; }
        public decimal? ISMCLTPRP_BalanceAmt { get; set; }
        public decimal? ISMCLTPRP_ExcessAmt { get; set; }
        public DateTime? ISMCLTPRP_PaymentDate { get; set; }
        public bool ISMCLTPRP_ActiveFlag { get; set; }
        public long ISMCLTPRP_CreatedBy { get; set; }
        public long? ISMCLTPRP_UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string ISMCLTPRP_PaymentStatus { get; set; }
    }
}
