using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("Ism_Sales_Product_Services_Items")]
    public class Ism_Sales_Product_Services_ItemsDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long ISMSPSI_ID { get; set; }
        public long MI_ID { get; set; }
        public long ISMSPS_ID { get; set; }
        public string ISMSPSI_ItemsName { get; set; }
        public string ISMSPSI_ItemsDescription { get; set; }
        public string ISMSPSI_ItemFlag { get; set; }
        public decimal ISMSPSI_Quantity { get; set; }
        public decimal ISMSPSI_Cost { get; set; }
        public decimal ISMSPSI_AMC_Cost { get; set; }
        public bool ISMSPSI_AMC_Flag { get; set; }
        public bool ISMSPSI_ActiveFlag { get; set; }
        public long ISMSPSI_CreatedBy { get; set; }
        public long ISMSPSI_UpdatedBy { get; set; }
        public DateTime? ISMSPSI_CreatedDate { get; set; }
        public DateTime? ISMSPSI_UpdatedDate { get; set; }
    }
}
