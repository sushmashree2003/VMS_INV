using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class EmployeeProfileReportDTO : Master_Employee_QulaificationDTO
    {
        // public long MI_Id { get; set; }
        // public string retrunMsg { get; set; }
        public long roleId { get; set; }
        // public long HRME_Id { get; set; }

        //institution
        public InstitutionDTO institutionDetails { get; set; }
        //designation name
        public string DesignationName { get; set; }
        //list for employee
        public MasterEmployeeDTO currentemployeeDetails { get; set; }
        //employee name dropdown
        public Array employeedropdown { get; set; }
        //employee Qualification
        public Array employequalification { get; set; }
        public Array employeedocument { get; set; }

        public string HRMC_QulaificationName { get; set; }


        public long?[] hrmgT_IdList { get; set; }
        public long?[] employeeTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }

        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }

        public Array employeeTypedropdown { get; set; }

        public Array groupTypedropdown { get; set; }


        public long LogInUserId { get; set; }
        public EmployeeProfileReportDTO[] ArrayempsList { get; set; }
        public Array institutionDetails_Array { get; set; }
        public List<EmployeeProfileReportDTO> AllInOne { get; set; }

        public string HRMEDS_DocumentName { get; set; }
        public string HRMEDS_DocumentImageName { get; set; }
        public string HRMEDS_DucumentDescription { get; set; }
    }
}
