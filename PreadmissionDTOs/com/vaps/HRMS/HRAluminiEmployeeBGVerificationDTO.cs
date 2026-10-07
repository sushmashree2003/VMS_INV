using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.HRMS
{
   public class HRAluminiEmployeeBGVerificationDTO
    {

        public long HRMEVER_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMRESG_Id { get; set; }
        public DateTime? HRMEVER_GeneratedDate { get; set; }       
        public string HRMEVER_VerificationCode { get; set; }
        public DateTime? HRMEVER_ExpiryDate { get; set; }     
        public string HRMEMNO_RequestedBy { get; set; }
        public string returnval { get; set; }
        public string ManagerMailid { get; set; }
        public string Template { get; set; }
        public string TeamLeadMailid { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRMEVER_RequestedCompanyName { get; set; }
        public string HRMEVER_EmailId { get; set; }
        //public string Editdata { get; set; }
        public long HRMEVER_ContactNo { get; set; }
        public bool HRMEVER_ActiveFlg { get; set; }
        public DateTime HRMEVER_CreatedDate { get; set; }
        public DateTime HRMEVER_UpdatedDate { get; set; }
        public long HRMEVER_CreatedBy { get; set; }
        public long HRMEVER_UpdatedBy { get; set; }

        public string retrunMsg { get; set; }
        public long MI_Id { get; set; }
        public long roleId { get; set; }       
        public Array employees { get; set; }
        public Array griddata { get; set; }
        public long userId { get; set; }
        public string retrun { get; set; }                  
        public string returnvales { get; set; }    
        public Array getdetails { get; set; }
        public dynamic[] checklist { get; set; }



    }
}
