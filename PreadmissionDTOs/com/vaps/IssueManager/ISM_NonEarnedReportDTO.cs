using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISM_NonEarnedReportDTO 
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public Array GetReport { get; set; }
        public long IVRMMS_Id { get; set; }       
        public long IVRMRT_Id { get; set; }
        public Array DepartmentList { get; set; }
        public Array Designation { get; set; }
        public Array get_userEmplist { get; set; }
        public Array Veiwdetails { get; set; }
        public Array Veiwdetailstt { get; set; }
        public bool roleflag { get; set; }
        public string Role_flag { get; set; }
        public string empdepartment { get; set; }
        public string empname { get; set; }
        public long ASMAY_Id { get; set; }
        public Array City_Type { get; set; }
        //public Array Get_MasterReport { get; set; }
        public long HRMDES_Id { get; set; }
        //public string RdoPunch { get; set; }
        public DepartmentDTO[] departmentlist { get; set; }
        public DesignationDTO[] designationlist { get; set; }
        public arrayuserEmployeeDTO[] arrayuserEmp { get; set; }
        public DateTime? From_Date { get; set; }
        public DateTime? To_Date { get; set; }
        public long VTADAAA_Id { get; set; }

        //********************** Block Report
        public string TypeFlg { get; set; }
        public string SelectionFlag { get; set; }
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
        public Array get_empblockReport { get; set; }
        public temp_EmployeeDTO[] temp_Employee { get; set; }
        public tempDepartmentDto[] dept { get; set; }
        public long HRMD_Id { get; set; }
        public long userid { get; set; }
        public Array getempblock { get; set; }
        public Array getemployeeReport { get; set; }
       
    }    
}
