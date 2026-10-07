using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
   public class EmailSms_HistoryDTO
    {
        public long UserId { get; set; }
        public long MI_Id { get; set; }
        public Array Institution { get; set; }
        public Array Module { get; set; }
        public string IVRMM_ModuleName { get; set; }
        public long IVRMM_Id { get; set; }      
        public DateTime ? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public Array getReport { get; set; }
        public uploadarray[] uploadarray { get; set; }
        public string optionflag { get; set; }
    }
    public class uploadarray
    {
        public string IVRMMM_ModuleName { get; set; }
        public long IVRMM_Id { get; set; }
    }
}
