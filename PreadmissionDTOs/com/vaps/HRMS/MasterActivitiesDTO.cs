using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.HRMS
{
   public class MasterActivitiesDTO
    {
        public int HR_PR_ID { get; set; }

        public string HR_PR_NAME { get; set; }

        public string HR_PR_DESC { get; set; }
        public DateTime HR_CreatedDate { get; set; }
        public DateTime HR_UpdatedDate{ get; set; }
        public long HR_CreatedBy{ get; set; }
        public long HR_UpdatedBy{ get; set; }
        public long MI_Id { get; set; }
        public long Userid { get; set; }
        public long roleid { get; set; }
        public string retrunMsg { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }

        public Array getloaddetails { get; set; }
        public long LogInUserId { get; set; }
    }
}
