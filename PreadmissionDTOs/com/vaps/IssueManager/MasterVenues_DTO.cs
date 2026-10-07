using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
   public class MasterVenues_DTO:CommonParamDTO
    {
        public long ISMMVNE_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public string ISMMVNE_VenueName { get; set; }
        public string ISMMVNE_VenueAddress { get; set; }
        public bool ISMMVNE_ActiveFlag { get; set; }
        public long? ISMMVNE_CreatedBy { get; set; }
        public long? ISMMVNE_UpdatedBy { get; set; }


        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array alldata { get; set; }

    }
}
