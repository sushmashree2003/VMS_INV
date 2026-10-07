using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class DepartmentWiseSalaryReportDTO
    {
        public long MI_Id { get; set; }
        public string MI_Name { get; set; }
        public long HRME_Id { get; set; }
        public long UserId { get; set; }
        public long roleId { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public string IVRM_Month_Name { get; set; }
        public Array get_mi_list1 { get; set; }
        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }
        //dropdown Array list
        public Array employeedetailList { get; set; }
        public Array employeeTypedropdownlist { get; set; }
        public Array groupTypedropdownlist { get; set; }
        public Array departmentdropdownlist { get; set; }
        public Array designationdropdownlist { get; set; }
        public Array gradedropdownlist { get; set; }
        public Array genderdropdownlist { get; set; }
        public Array yearlist { get; set; }
        public Array monthlist { get; set; }
        public Array GetReportvalue { get; set; }
        public long?[] MI_Idlist { get; set; }
        public long?[] employeeTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }
        public long?[] groupTypeIdList { get; set; }
        public long LogInUserId { get; set; }
    }
}
