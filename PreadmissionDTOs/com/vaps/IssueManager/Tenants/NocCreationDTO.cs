using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.Tenants
{
    public class NocCreationDTO
    {


        public long TENANTNOC_Id { get; set; }
        public long MI_Id { get; set; }
        public long TENANTAGR_Id { get; set; }
        public long HRMF_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRMR_Id { get; set; }
        public long User_Id { get; set; }
        public DateTime TENANTNOC_NOCDate { get; set; }
        public string TENANTNOC_Remarks { get; set; }
        public bool TENANTNOC_ActiveFlag { get; set; }
        public DateTime TENANTNOC_CreatedDate { get; set; }
        public DateTime TENANTNOC_UpdatedDate { get; set; }
        public long TENANTNOC_CreatedBy { get; set; }
        public long TENANTNOC_UpdatedBy { get; set; }
        public Array tenantlist { get; set; }
        public Array tname { get; set; }
        public Array get_building { get; set; }
        public Array get_Area { get; set; }
        public Array get_floor { get; set; }
        public Array get_getAreaName { get; set; }
        public Array get_report { get; set; }
        public Array editarrey { get; set; }
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }

        public Array staffmappinglist { get; set; }
        public Array staffmappinglistTemp { get; set; }
        public string name { get; set; }
        public string area { get; set; }
        public string remarks { get; set; }
        public DateTime date { get; set; }
        public TempDTOO[] ActivitynameArray { get; set; }
        public long HRMB_Id { get; set; }
        public class TempDTOO
        { 
            public long TENANTAGR_Id { get; set; }
        }
    }
}
