using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Preadmission
{
    public class CollegeStudentGuardianDTO : CommonParamDTO
    {
        public long PACSTG_Id { get; set; }
        public long PACA_Id { get; set; }
        public string PACSTG_GuardianName { get; set; }
        public string PACSTG_GuardianAddress { get; set; }
        public string PACSTG_GuardianPhoneNo { get; set; }
        public string PACSTG_emailid { get; set; }
        public string PACSTG_GuardianPhoto { get; set; }
        public string PACSTG_GuardianSign { get; set; }
        public string PACSTG_Fingerprint { get; set; }
        public string PACSTG_GuardianLoginFlag { get; set; }
    }
}
