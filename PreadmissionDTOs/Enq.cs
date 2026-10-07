using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class Enq : CommonParamDTO
    {
        public long PASE_Id { get; set; }


        public long MI_Id { get; set; }

        public long ASMAY_Id { get; set; }

        public long AMC_Id { get; set; }

        public long ASMCL_Id { get; set; }
        public string PASE_FirstName { get; set; }
        public string PASE_MiddleName { get; set; }
        public string PASE_LastName { get; set; }
        public DateTime PASE_Date { get; set; }
        public string PASE_EnquiryNo { get; set; }
        public string PASE_Address1 { get; set; }
        public string PASE_Address2 { get; set; }
        public string PASE_Address3 { get; set; }
        public string PASE_City { get; set; }
        public string PASE_State { get; set; }
        public long IVRMMC_Id { get; set; }
        public long PASE_Pincode { get; set; }
        public long PASE_MobileNo { get; set; }
        public string PASE_emailid { get; set; }
        public int PASE_ActiveFlag { get; set; }
        public long PASE_Phone { get; set; }

        public long Id { get; set; }

        public string PASE_EnquiryDetails { get; set; }

        public bool returnval { get; set; }
        public Array enqdata { get; set; }
        public Array enquiryList { get; set; }
        public Array stateDrpDown { get; set; }
        public Array cityDrpDown { get; set; }

        public Array enquirylist { get; set; }
        public SortingPagingInfoDTO trustPagination { get; set; }
        public string searchType { get; set; }
        public string searchString { get; set; }
        public int count { get; set; }
        public string UserName { get; set; }
        public string ASMCL_ClassName { get; set; }


        //---------------------------------//

        public int IVRMMS_Id { get; set; }

        public string IVRMMS_Name { get; set; }

        public string IVRMMC_CountryName { get; set; }
        public long IVRMMCT_Id { get; set; }
        public string IVRMMCT_Name { get; set; }


        public Array countryDrpDown { get; set; }
        // public Array countryDrpDown { get; set; }
        public Array courseDrpDown { get; set; }
        //   public Array stateDrpDown { get; set; }
        // public Array cityDrpDown { get; set; }

        //sripad added
        public Array yearDrpDwn { get; set; }
        public Array classDrpDwn { get; set; }
        public Array categoryDrpDwn { get; set; }

        public Array organisationname { get; set; }
        public string GeneratedNumber { get; set; }
        public string returnMsg { get; set; }

        //added on 11 jan

        public Array configurationsettings { get; set; }
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }


        public string returnduplicatestatus { get; set; }
    }

    public class Enquirydrpdown
    {
        public int amcl_id { get; set; }
        public string amco_name { get; set; }
        public int amy_id { get; set; }
        public string amy_name { get; set; }
        public int amsta_id { get; set; }
        public string amsta_name { get; set; }
        public int amcon_id { get; set; }
        public string amcon_name { get; set; }
    }


    class MonthDayYearDateConverter : IsoDateTimeConverter
    {
        public MonthDayYearDateConverter()
        {
            DateTimeFormat = "MM/dd/yyyy";
        }
    }
}
