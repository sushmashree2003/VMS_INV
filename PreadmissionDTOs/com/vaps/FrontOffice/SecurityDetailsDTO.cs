using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
   public class SecurityDetailsDTO
    {

        public long ISMSD_Id { get; set; }
        public string ISMSD_FirstName { get; set; }
        public string ISMSD_MiddleName { get; set; }
        public string ISMSD_LastName { get; set; }
        public long ISMSD_MobileNo { get; set; }
        public string ISMSD_EmailId { get; set; }
        public string ISMSD_PerStreet { get; set; }
        public string ISMSD_PerArea { get; set; }
        public string ISMSD_PerCity { get; set; }
        public long ISMSD_PerPincode { get; set; }
        public long IVRMMC_Id { get; set; }
        public long IVRMMS_Id { get; set; }
        public string ISMSD_Photo { get; set; }
        public long ISMSD_Aadhar { get; set; }
        public string ISMSD_Designation { get; set; }
        public string ISMSD_PANNo { get; set; }
        public bool ISMSD_LeftFlag { get; set; }
        public bool ISMSD_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMSD_CreatedBy { get; set; }
        public long ISMSD_UpdatedBy { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public string countryname { get; set; }
        public string statename { get; set; }
        public bool duplicate { get; set; }
        public string msg { get; set; }
        public bool ret { get; set; }
        public Array Editlist { get; set; }
        public Array countrylist { get; set; }
        public Array statelist { get; set; }
        public Array alldata { get; set; }
        public Array statelistdata { get; set; }

       
        public string IVRMMC_CountryName { get; set; }
        public string IVRMMC_CountryCode { get; set; }

        
        public string IVRMMS_Name { get; set; }
        public string IVRMMS_Code { get; set; }

    }
}
