using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
   public class VenueMapping_DTO:CommonParamDTO
    {
        public long ISMMMER_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMMVNE_Id { get; set; }
        public string ISMMMER_MeetingRoomName { get; set; }
        public string ISMMMER_FloorName { get; set; }
        public string ISMMMER_Facilities { get; set; }
        public bool ISMMMER_ActiveFlag { get; set; }
        public long ISMMMER_CreatedBy { get; set; }
        public long ISMMMER_UpdatedBy { get; set; }
        public long UserId { get; set; }

        public string ISMMVNE_VenueName { get; set; }
        public string ISMMVNE_VenueAddress { get; set; }

        public bool returnval { get; set; }
        public bool duplicate { get; set; }
        public Array venuelist { get; set; }
        public Array alldata { get; set; }



    }
}
