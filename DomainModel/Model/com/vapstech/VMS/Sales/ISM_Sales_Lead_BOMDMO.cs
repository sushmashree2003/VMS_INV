using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{

    [Table("Ism_Sales_Product_Services_Items_final")]
    public class ISM_Sales_Lead_BOMDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ISPSIF_Id { get; set; }
        public long ISMSLE_Id { get; set; }
        public long ISMSBOM_Id { get; set; }
        public long ISMSMPR_Id { get; set; }
        public long ISMSPS_ID { get; set; }
        public string ISPSIF_ItemsName { get; set; }
        public decimal? ISPSIF_Quantity { get; set; }
        public decimal? ISPSIF_Cost { get; set; }
        public decimal? Negotiation_Amount { get; set; }
        public bool ISPSIF_ActiveFlag { get; set; }
        public bool? ISMS_NegotiationFlag { get; set; }
        public bool? ISMS_MOMFlag { get; set; }
        public bool? ISMS_NegoMDApprovedFlag { get; set; }
        public long ISPSIF_CreatedBy { get; set; }
        public long ISPSIF_UpdatedBy { get; set; }
        public DateTime? ISPSIF_CreatedDate { get; set; }
        public DateTime? ISPSIF_UpdatedDate { get; set; }
        public string Negotiation_Client { get; set; }
        public string ISPSIF__Remarks { get; set; }
        
    }
    
}
