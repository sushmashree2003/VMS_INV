using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INV_VendorCategoryMasterDTO
    {
        public long VCM_Id { get; set; }
        public long MI_Id { get; set; }
        public string VCM_Vendorname { get; set; }
        public bool VCM_Activeflag { get; set; }
        public long VCM_Createdby { get; set; }
        public long VCM_Updatedby { get; set; }
        public DateTime VCM_Createddate { get; set; }
        public DateTime VCM_Updateddate { get; set; }


        public bool returnval { get; set; }
        public long UserId { get; set; }
        public string saveorupdate { get; set; }

        public Array VendorCategoryMasterArray { get; set; }
    }
}
