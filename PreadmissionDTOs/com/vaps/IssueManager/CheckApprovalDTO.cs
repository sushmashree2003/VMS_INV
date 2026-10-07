using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class CheckApprovalDTO
    {
        public long UserId { get; set; }
        public long MI_Id { get; set; }
        public CheckBookEntryApprovalDTO[] Apprval { get; set; }
        public long VPAYVOUAUT_OTP { get; set; }
        public  bool returnvalue { get; set; }

       
    }
}
