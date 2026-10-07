using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISM_PDpnt_SDpnt_MappingDTO
    {
        public long ISMPDSDMAP_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long ISMPDSDMAP_SMI_Id { get; set; }
        public long ISMPDSDMAP_SHRMD_Id { get; set; }
        public bool ISMPDSDMAP_ActiveFlag { get; set; }
        public long ISMPDSDMAP_CreatedBy { get; set; }
        public long ISMPDSDMAP_UpdatedBy { get; set; }
        public DateTime? ISMPDSDMAP_CreatedDate { get; set; }
        public DateTime? ISMPDSDMAP_UpdatedDate { get; set; }
        public long User_Id { get; set; }
        public string Pmi_name { get; set; }
        public string Pdepartmentname { get; set; }
        public string Smi_name { get; set; }
        public string Sdepartmentname { get; set; }
        public string msg { get; set; }

        public Array departmentlist { get; set; }
        public Array institutionlist { get; set; }
        public Array gridviewdetails { get; set; }
        public Array editlist { get; set; }
    }
}