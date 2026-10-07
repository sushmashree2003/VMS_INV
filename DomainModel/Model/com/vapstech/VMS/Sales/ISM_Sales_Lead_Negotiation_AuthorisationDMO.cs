using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sales_Lead_Negotiation_Authorisation")]
    public class ISM_Sales_Lead_Negotiation_AuthorisationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ISMSALENA_Id { get; set; }
        public long ISMSLE_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public long ISMSALENA_Sanctioned { get; set; }
        public decimal? ISMSALENA_SanctionedAmount { get; set; }
        public string ISMSALENA_Remarks { get; set; }
        public string  ISMSALENA_StatusFlg { get; set; }
        public DateTime? ISMSALENA_Date { get; set; }
        public bool ISMSALENA_ActiveFlg { get; set; }
        public long  ISMSALENA_CreatedBy { get; set; }
        public long ISMSALENA_UpdatedBy { get; set; }
        public DateTime? ISMSALENA_CreatedDate { get; set; }
        public DateTime? ISMSALENA_UpdatedDate { get; set; }
        public long ISMSLET_Id { get; set; }
    } 
}
