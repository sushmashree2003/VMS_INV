using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeMasterConcessionDTO
    {
        public long FMCC_Id { get; set; }
        public long MI_Id { get; set; }

        public string FMCC_ConcessionName { get; set; }
        public string FMCC_ConcessionFlag { get; set; }
        public string FMCC_ConcessionApplLimit { get; set; }
        public bool FMCC_ActiveFlag { get; set; }
         public Array savedata { get; set; }
        public Array editdata { get; set; }
        public Array feeconcess { get; set; }
        public string returnduplicatestatus { get; set; }
        public Array ClaSSCategoryArray { get; set; }
        public bool retrunval { get; set; }

        public string message { get; set; }

    }
}
