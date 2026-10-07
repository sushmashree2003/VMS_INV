using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISM_BlockEmployeeDTO
    {
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }   
        public long ASMAY_Id { get; set; }   
        public bool returnval { get; set; }
        public string message { get; set; }
        public long userid { get; set; }
    
        public long ISMBE_Id { get; set; }
        
        public long HRME_Id { get; set; }
        public string empname { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public DateTime? ISMBE_BlockDate { get; set; }
        public DateTime? ISEBE_UnblockDate { get; set; }
        public string ISMBE_Reason { get; set; }
        public bool ISMBE_BlockFlg { get; set; }
        public bool ISMBE_ActiveFlg { get; set; }
        public long ISMBE_CreatedBy { get; set; }
        public long ISMBE_UpdatedBy { get; set; }
        public string HRME_AppDownloadedDeviceId { get; set; }
        public string sound { get; set; }

        public Array get_Emplist { get; set; }       
        public Array get_blockedEmplist { get; set; }       
        public Array get_blockedEmpDeatils { get; set; }       
        public Array deviceArray { get; set; }       
        public temp_blockEmployee[] temp_blockEmployee { get; set; }


        //********************** Block Report
        public string TypeFlg { get; set; }
        public string SelectionFlag { get; set; }
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
        public Array get_empblockReport { get; set; }
        public temp_EmployeeDTO[] temp_Employee { get; set; }
        public tempDepartmentDto[] dept { get; set; }
    }
    public class tempDepartmentDto
    {
        public long HRMDC_ID { get; set; }
    }
    public class temp_blockEmployee
    {
        public long ISMBE_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }    
    }
    public class temp_EmployeeDTO
    {     
        public long HRMD_Id { get; set; }  
        public long HRME_Id { get; set; }
    }
}
