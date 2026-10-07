using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Legal
{
    public class Legal_Notice_Request_DocumentDTO
    {
        public long LNRD_Id { get; set; }
        public long LNR_Id { get; set; }
        public string LNRD_Document { get; set; }
        public DateTime? LNRD_CreatedDate { get; set; }
        public DateTime? LNRD_UpdatedDate { get; set; }
        public long LNRD_CreatedBy { get; set; }
        public long LNRD_UpdatedBy { get; set; }
    }
}
