using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Master_SciencePark_VendorCost")]
    public class ISM_Master_SciencePark_VendorCostDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IMSV_ID { get; set; }
       
        public string IMSV_PackageName { get; set; }
        public decimal? IMSV_Quantity { get; set; }
        public decimal? IMSV_Price { get; set; }
        public decimal? IMSV_Percentage { get; set; }
        public decimal? IMSV_TotalCost { get; set; }
        public decimal? IMSV_TotalCostPerQty { get; set; }
        public long? INVMS_ID { get; set; }
        public string IMSV_PackageFlag { get; set; }
        public DateTime? IMSV_CreatedDate { get; set; }
        public DateTime? IMSV_UpdatedDate { get; set; }
        public bool? IMSV_ActiveFlag { get; set; }
        public long? IMSV_CreatedBy { get; set; }
        public long? IMSV_UpdatedBy { get; set; }


    }
}
