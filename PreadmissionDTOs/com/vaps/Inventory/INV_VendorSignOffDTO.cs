using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INV_VendorSignOffDTO
    {
        public long INVMSO_Id { get; set; }
        public long MI_Id { get; set; }
        public long MVE_Id { get; set; }
        public DateTime? INVMSO_SignOffDate { get; set; }
        public string INVMSO_MOU_FilePath { get; set; }
        public string INVMSO_WorkOrder_FilePath { get; set; }
        public bool INVMSO_ActiveFlg { get; set; }
        public long INVMSO_CreatedBy { get; set; }
        public long INVMSO_UpdatedBy { get; set; }

        public long UserId { get; set; }
        public bool returnval { get; set; }
        public string saveorupdate { get; set; }

        public string MVE_VendorName { get; set; }
        public string MVE_CompanyName { get; set; }
        public string MVE_Email_Address { get; set; }
        public string htmldata { get; set; }
        public string msg { get; set; }

        public Array vendorList { get; set; }
        public Array signOffList { get; set; }
        public INV_VendorSignOffDTO[] selected_vendor_signoff_array { get; set; }
        public Array VendroNameDropdownArray { get; set; }
        public Array VendrosignoffGridArray { get; set; }
    }
}
