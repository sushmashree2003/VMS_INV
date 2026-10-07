using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.IVRS
{
   public class IVRSOBD
    {
        public long  MI_ID { get; set; }
        public long  ASMAY_ID { get; set; }
        public long  ASMCL_ID { get; set; }
        public long  ASMS_ID { get; set; }
        public Array maindata { get; set; }
        public string studentName { get; set; }
        public string AMST_AdmNo { get; set; }
        public string AMST_emailId { get; set; }
        public long AMST_MobileNo { get; set; }
        public long AMST_Id { get; set; }
        public Array acs_lst { get; set; }
        public Array clas_list { get; set; }
        public Array sect_list { get; set; }
        public string schoolname { get; set; }       
    }
}
