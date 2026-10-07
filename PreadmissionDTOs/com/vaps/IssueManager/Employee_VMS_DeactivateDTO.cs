using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class Employee_VMS_DeactivateDTO
    {
        public long HRME_Id { get; set; }
        public long UserId { get; set; }
        public long MI_Id { get; set; }
        public string HRMEM_EmailId { get; set; }
        public string DeptHeadmail { get; set; }
        public string returnmsg { get; set; }
        public Array get_employeelist { get; set; }
    
        public employeearray[] employeedetails { get; set; }
    }
    public class employeearray
    {
        public long HRME_Id { get; set; }
        public long Activeflag { get; set; }
    }
}
