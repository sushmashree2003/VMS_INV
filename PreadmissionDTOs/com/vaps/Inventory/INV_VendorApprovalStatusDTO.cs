using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INV_VendorApprovalStatusDTO
    {
        public long VAS_Id { get; set; }

        public long MI_Id { get; set; }

        public string ApprovalStatus { get; set; }

        public DateTime? ActionDate { get; set; }

        public long MVE_Id { get; set; }

        public long CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public long? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
