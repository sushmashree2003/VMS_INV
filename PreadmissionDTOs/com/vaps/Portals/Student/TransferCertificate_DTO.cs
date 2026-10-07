using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.Student
{
   public class TransferCertificate_DTO:CommonParamDTO
    {

        public long ASCA_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASCA_CertificateType { get; set; }
        public string ASCA_Reason { get; set; }
        public DateTime ASCA_ApplyDate { get; set; }
        public string ASCA_Status { get; set; }
        public bool ASCA_ActiveFlg { get; set; }


        public long ASCAP_Id { get; set; }      
        public long IVRMALU_Id { get; set; }
        public string ASCAP_Status { get; set; }
        public string ASCAP_ApproveReason { get; set; }
        public DateTime ASCAP_ApproveDate { get; set; }
        public bool ASCAP_ActiveFlg { get; set; }


        public long ASMCL_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public long ASMS_Id { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string AMST_emailId { get; set; }
        public long AMST_MobileNo { get; set; }


        public Array studentdetails { get; set; }
        public Array aply_aprvlist { get; set; }
        public string AMST_AdmNo { get; set; }
        public string ASMAY_Year { get; set; }
        public string flag { get; set; }
        public long UserId { get; set; }
        public long IVRMRT_Id { get; set; }
        public string flag_Type { get; set; }

        public bool returnval { get; set; }
        public string roletype { get; set; }
        public long HRME_Id { get; set; }
        public Array studlist { get; set; }
        public Array applylist { get; set; }
        public bool duplicate { get; set; }



    }
}
