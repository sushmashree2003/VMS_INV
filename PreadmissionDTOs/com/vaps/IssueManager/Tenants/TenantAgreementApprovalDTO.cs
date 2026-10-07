using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.Tenants
{
    public class TenantAgreementApprovalDTO
    {
        public long TENANTAGRAPR_Id { get; set; }
        public long TENANTAGR_Id { get; set; }
        public bool TENANTAGRAPR_ApprovedFlg { get; set; }
        public long TENANTAGRAPR_FApprovedBy { get; set; }
        public string TENANTAGRAPR_Remarks { get; set; }
        public bool TENANTAGRAPR_ActiveFlg { get; set; }
        public DateTime TENANTAGRAPR_CreatedDate { get; set; }
        public DateTime TENANTAGRAPR_UpdatedDate { get; set; }
        public long TENANTAGRAPR_CreatedBy { get; set; }
        public long TENANTAGRAPR_UpdatedBy { get; set; }
        public long MI_Id { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public Array viewlist { get; set; }

        public long UserId { get; set; }

        public Array getinstitution { get; set; }
        public grid_array[] grid_arrays { get; set; }

        public string returnval { get; set; }
    }

    public class grid_array
    {
        public long? TENANTAGRAPR_FApprovedBy { get; set; }
        public long? TENANTAGR_Id { get; set; }
        public string TENANTAGRAPR_Remarks { get; set; }
        public int TENANTAGRAPR_ApprovedFlg { get; set; }
        public long? UserId { get; set; }

    }

}
