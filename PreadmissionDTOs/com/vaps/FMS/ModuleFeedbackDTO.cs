using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FMS
{
   public class ModuleFeedbackDTO
    {
        public long VMF_ID { get; set; }
        public long IVRMM_Id { get; set; }
        public string VMF_ModuleName { get; set; }
        public long HRME_Id { get; set; }
        public long VMF_Rating { get; set; }
        public string VMF_Remarks { get; set; }
        public bool VMF_ActiveFlag { get; set; }
        public DateTime VMF_CreatedDate { get; set; }
        public DateTime VMF_UpdatedDate { get; set; }
        public long VMF_CreatedBy { get; set; }
        public long VMF_UpdatedBy { get; set; }
        public string saved { get; set; }
        public string feedbacktype { get; set; }
        public long  MI_Id { set; get; }
        public long? userid { get; set; }
        public long IVRMRT_Id { get; set; }
        public string Role_flag { get; set; }
        public string existfeedback { set; get; }
        public string wrongfeedback { set; get; }
        public Array useronloadfeedback { set; get; }
    
        public string nofeedbackfound { get; set; }

        //---------------------------- Auto Task Creation
        public bool createtask { set; get; }
        public string BugOREnh { set; get; }
        public string feedbacktask { set; get; }
    }
}
