using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Sports
{
    public class MasterSportsCCNameDTO:CommonParamDTO
    {
        public long SPCCMSCC_Id { get; set; }
        public long SPCCMSCCG_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCMSCC_SportsCCName { get; set; }
        public string SPCCMSCC_SportsCCDesc { get; set; }
        public string SPCCMSCC_SGFlag { get; set; }
        public int SPCCMSCC_NoOfMembers { get; set; }
        public string SPCCMSCC_RecHighLowFlag { get; set; }
        public string SPCCMSCC_RecInfo { get; set; }
        public bool SPCCMSCC_ActiveFlag { get; set; }
        public int count { get; set; }
        public string returnVal { get; set; }
        public Array sportsCCNameList { get; set; }
        public Array editDetails { get; set; }
        public Array gropList { get; set; }
        public string groupName { get; set; }
        public bool retval { get; set; }

    }
}
