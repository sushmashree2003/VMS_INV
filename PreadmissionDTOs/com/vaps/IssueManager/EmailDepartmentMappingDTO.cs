using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class EmailDepartmentMappingDTO
    {
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long ISMDR_Id { get; set; }
        public long ISM_EId { get; set; }
        public Array getdepartment { get; set; }
        public bool returnval { get; set; }
        public Array geteditdata { get; set; }
        public Array getonchangedata { get; set; }
        public string message { get; set; }
        public long userid { get; set; }
        public Array getsavelist { get; set; }
        public string departmentname { get; set; }
        public string instituitonname { get; set; }
        public string remarks { get; set; }

        public string emailids { get; set; }
        public temp_emaildeptmapping[] multipleemailid { get; set; }
        public temp_deptremarks[] temp_deptremarks { get; set; }
        public Array getinstitution { get; set; }
        public Array getsavedlist { get; set; }
        public Array getinstdeptsavedlist { get; set; }
        public Array geteditremarksdata { get; set; }
        public bool ISMDR_ActiveFlag { get; set; }
        public bool ISMDR_ReasonFlag { get; set; }


    }
    public class temp_emaildeptmapping
    {
        public string emaild { get; set; }
    }
    public class temp_deptremarks
    {
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long ISMDR_Id { get; set; }
        public string ISMDR_Remarks { get; set; }
        public bool ISMDR_ReasonFlag { get; set; }
    }
}
