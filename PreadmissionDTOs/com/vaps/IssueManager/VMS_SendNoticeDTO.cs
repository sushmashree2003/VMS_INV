using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class VMS_SendNoticeDTO
    {
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public long UserId { get; set; }
        public long VNB_ID { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string VNB_Title { get; set; }
        public string VNB_Description { get; set; }
        public string VNB_Type { get; set; }
        public bool VNB_ActiveFlag { get; set; }
        public string VNB_FileName { get; set; }
        public string VNB_FilePath { get; set; }
        public DateTime? VNB_CreatedDate { get; set; }
        public DateTime? VNB_UpdatedDate { get; set; }
        public long VNB_CreatedBy { get; set; }
        public long VNB_UpdatedBy { get; set; }

        public string message { get; set; }
        public Array employeelist { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmailId { get; set; }
        public Array vmsnoticelist { get; set; }
        public studentarrays[] studentarray { get; set; }

        public FilePath_Arrays[] FilePath_Array { get; set; }

        public class studentarrays
        {
            public long HRME_Id { get; set; }
            public long MI_Id { get; set; }
            public string HRME_EmailId { get; set; }
            public string HRME_EmployeeFirstName { get; set; }
        }
        public class FilePath_Arrays
        {
            public string VNB_FilePath { get; set; }
            public string VNB_FileName { get; set; }
        }
    }
}