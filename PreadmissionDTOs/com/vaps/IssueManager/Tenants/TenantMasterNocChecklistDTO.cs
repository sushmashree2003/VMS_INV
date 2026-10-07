using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.Tenants
{
    public class TenantMasterNocChecklistDTO
    {
        public long MI_Id { get; set; }
        public long TENANTNOCCL_Id { get; set; }
        public long TENANTAGR_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRMAM_Id { get; set; }
        public Boolean TENANTNOCCL_WorkingFlg { get; set; }
        public Boolean TENANTNOCCL_ActiveFlag { get; set; }
        public DateTime TENANTNOCCL_CheckedDate { get; set; }
        public string TENANTNOCCL_Remarks { get; set; }
        public decimal TENANTNOCCL_DeductingAmount { get; set; }
        public DateTime TENANTNOCCL_CreatedDate { get; set; }
        public DateTime TENANTNOCCL_UpdatedDate { get; set; }
        public DateTime TENANTNOCCL_CreatedBy { get; set; }
        public DateTime TENANTNOCCL_UpdatedBy { get; set; }
        public string TENANTNOCCL_FileName { get; set; }
        public string TENANTNOCCL_FilePath { get; set; }
        public Array get_tenant { get; set; }
        public Array get_amenities { get; set; }
        public Array get_amenity { get; set; }
        public string returnduplicatestatus { get; set; }
        public string HRMAM_AmenitiesName { get; set; }
        public long User_Id { get; set; }

        public Array staffmappinglist { get; set; }
        public string name { get; set; }
        public string area { get; set; }
        public string remarks { get; set; }
        public DateTime date { get; set; }
        public decimal damount { get; set; }
        public bool returnval { get; set; }
        public Array editarrey { get; set; }

    }
}
