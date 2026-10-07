using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FMS
{
    public class FMS_Dept_Letter_HeadMappingDTO
    {
        public long FMSDLH_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long? UserId { get; set; }
        public string FMSDLH_ReceivedBy { get; set; }
        public string FMSDLH_LetterheadsFromNo { get; set; }
        public string FMSDLH_LetterheadsToNo { get; set; }
        public DateTime? FMSDLH_ReceivedDate { get; set; }
        public DateTime? createddate { get; set; }
        public bool? FMSDFC_ActiveFlg { get; set; }
        public Array getinstitution { get; set; }
        public Array getdepartment { get; set; }
        public Array geteditdetails { get; set; }
        public Array getdetails { get; set; }
        public Array getinstitutiondetails { get; set; }
        public Array getusedletterheads { get; set; }
        public string MI_Name { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public long FMSDLHT_Id { get; set; }
        public string FMSDLHT_LetterheadNo { get; set; }
        public bool? FMSDLHT_CancelledFlag { get; set; }
        public string FMSDLHT_Reason { get; set; }
        public string UserName { get; set; }
        public DateTime? FMSDLHT_Date { get; set; }
        public Array getdeptletterheadlist { get; set; }
        public Array trackingeditdata { get; set; }
        public Array getreport { get; set; }
    }
}