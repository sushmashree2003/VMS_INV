using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.FeedBack
{
   public  class EmployeeHeadFeedBackDTO
    {

        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long HRME_Id { get; set; }
        public long IVRMRT_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRGFE_Id { get; set; }
        public bool roleflag { get; set; }
        public bool returnval { get; set; }
        public Array DepartmentList { get; set; }
        public Array Designation { get; set; }
        public Array feedbackgrid { get; set; }
        public Array get_userEmplist { get; set; }
        public string empname { get; set; }
        public string empdepartment { get; set; }
        public string Role_flag { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string userEmpName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRGFE_FeedBack { get; set; }
        public string returnduplicatestatus { get; set; }
        public DateTime HRGFE_FeedbackDate { get; set; }
        public totalgridd[] totalgrid { get; set; }
        
    }
    public class totalgridd
    {
        public long HRME_Id { get; set; }
        public string HRGFE_FeedBack { get; set; }
        
    }
}
