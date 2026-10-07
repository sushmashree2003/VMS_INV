using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class RemaindersDetailsDTO
    {
        public long IVRM_MI_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long ISES_Id { get; set; }
        public long IVRM_ClientCode { get; set; }
        public Array GetInstitutionDetails { get; set; }
        public Array GetMasterTemplateDetails { get; set; }
        public Array getTemplateDetails { get; set; }
        public Array getUserDetails { get; set; }
        public Array getMappedUserDetails { get; set; }
        public Temp_UserMapping_Add[] Temp_UserMapping_Add { get; set; }
        public Temp_UserMapping_Remove[] Temp_UserMapping_Remove { get; set; }
        public string ISES_Template_Name { get; set; }
        public string ISES_TemplateContent { get; set; }
        public string returnvalue { get; set; }
        public string ISMMCLT_ClientName { get; set; }
        public string ISMMCLT_EmailId { get; set; }
        public string ISMMCLT_ContractExpDate { get; set; }
    }

    public class Temp_UserMapping_Add
    {
        public long UserId { get; set; }
        public long ISESUSR_Id { get; set; }
    }

    public class Temp_UserMapping_Remove
    {
        public long UserId { get; set; }
        public long ISESUSR_Id { get; set; }
    }
}
