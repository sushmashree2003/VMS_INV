using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{

   public class ISM_RoleDeptMappingDTO
    {
        public long ISMRTDEM_Id { get; set; }
        public long MI_Id { get; set; }
        public long IVRMRT_Id { get; set; }
        public long HRMD_Id { get; set;}
        public bool ISMRTDEM_ActiveFlag { get; set;}
        public long ISMRTDEM_CreatedBy { get; set;}
        public long ISMRTDEM_UpdatedBy { get; set;}
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UserId { get; set; }
        public Array role_list { get; set; }
        public Array get_deptlist { get; set; }
        public Array getdept { get; set; }
        public Array department_list { get; set; }
        public string IVRMRT_Role { get; set;}
        public string HRMD_DepartmentName { get; set;}
        public string Rolename { get; set; }
        public ISM_RoleDeptMappingDTO[] departmentids { get; set; }
        public bool duplicate { get; set; }
        public string msg { get; set; }
        public bool returnval { get; set; }


    }
}
