using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs
{
    public class IVRMandVMSInterlinkingDashboardDTO
    {
        public long MI_Id { get; set; }
        public long roleId { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRMEMNO_MobileNo { get; set; }
        public string HRMEM_EmailId { get; set; }
        public string IEName { get; set; }
        public Array clintlist { get; set; }
        public Array clientIEDetails { get; set; }
        public Array clientpaymentdetails { get; set; }
    }
}
