using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
   public class ComplaintRegister_DTO
    {
        public long VMSCR_Id { get; set; }
        public long MI_Id { get; set; }
        public string VMSCR_Name { get; set; }
        public string VMSCR_EmailId { get; set; }
        public string VMSCR_TaskTitle { get; set; }
        public string VMSCR_Desc { get; set; }
        public double VMSCR_MobileNo { get; set; }
        public DateTime? VMSCR_CreatedDate { get; set; }
        public DateTime? VMSCR_UpdatedDate { get; set; }
        public string message { get; set; }
    }
}
