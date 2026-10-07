using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class clientIEmappingDTO
    {
        public long FOBVIEM_Id { get; set; }
        public long MI_Id { get; set; }
        public string FOBVIEM_BiometricId { get; set; }
        public long FOBVIEM_HRMEId { get; set; }
        public long FOBVIEM_InsertURL { get; set; }
        public bool FOBVIEM_ActiveFlg { get; set; }
        public long FOBVIEM_CreatedBy { get; set; }
        public long FOBVIEM_UpdatedBy { get; set; }
        public long FOBVIEM_Insert_MI_Id { get; set; }
        public DateTime? FOBVIEM_CreatedDate { get; set; }
        public DateTime? FOBVIEM_UpdatedDate { get; set; }
        public string EmplYoeeName { get; set; }
        public long Userid { get; set; }
        public long roleid { get; set; }
        public string retrunMsg { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public Array employee_list { get; set; }
        public Array client_list { get; set; }
        public Array getloaddetails { get; set; }
        public string BioMetricCode { get;set; }
        public string MI_Name { get; set; }

        public string Type { get; set; }
        public long HRME_Id { get; set; }
        public string HRME_BiometricCode { get; set; }
    }
}
