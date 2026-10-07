using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class Master_DepartmentHead_MappingDTO
    {
        public Array allcompany { get; set; }
        public Array alldepartmentcode { get; set; }
        public Array allEmployeename { get; set; }
        public Array getdetails { get; set; }
        public Array displaydata { get; set; }
        public long UserId { get; set; }
        public long HRMDC_ID { get; set; }
        public string HRMDC_Name { get; set; }
        public int HRMDC_Order { get; set; }
        public string HRMDC_Code { get; set; }
        public long HRME_ID { get; set; }
        public long HRMDCH_ID { get; set; }
        public string returnduplicatestatus { get; set; }
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public long HRMDCH_CreatedBy { get; set; }
        public long HRMDCH_UpdatedBy { get; set; }
    }
}
