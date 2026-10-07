using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Sales
{
    public class ISM_Master_SciencePark_VendorCostDto
    {
        public long IMSV_ID { get; set; }

        public string IMSV_PackageName { get; set; }
        public decimal IMSV_Quantity { get; set; }
        public decimal IMSV_Price { get; set; }
        public decimal IMSV_Percentage { get; set; }
        public decimal IMSV_TotalCost { get; set; }
        public decimal IMSV_TotalCostPerQty { get; set; }
        public long INVMS_ID { get; set; }
        public string IMSV_PackageFlag { get; set; }
        public DateTime? IMSV_CreatedDate { get; set; }
        public DateTime? IMSV_UpdatedDate { get; set; }
        public bool IMSV_ActiveFlag { get; set; }
        public long? IMSV_CreatedBy { get; set; }
        public long? IMSV_UpdatedBy { get; set; }
        public Array OnLoadVendorCostList { get; set; }
        public Array OnLoadSupplierList { get; set; }

        public long MI_ID { get; set; }
        public long User_Id { get; set; }
        public string returndata { get; set; }

        public VendorCost[] VendorCostSave { get; set; }
        public class VendorCost {
            public string IMSV_PackageName { get; set; }
            public decimal IMSV_Quantity { get; set; }
            public decimal IMSV_Price { get; set; }
            public decimal IMSV_Percentage { get; set; }
            public decimal IMSV_TotalCost { get; set; }
            public decimal IMSV_TotalCostPerQty { get; set; }
            public string IMSV_PackageFlag { get; set; }
        }

    }
    
}
