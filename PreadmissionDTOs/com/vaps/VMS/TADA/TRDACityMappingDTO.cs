using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.TADA
{
   public  class TRDACityMappingDTO
    {
        public long UserId { get; set; }
        public long MI_Id { get; set; }
        public long VTADACM_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public decimal VTADACM_FoodAmt { get; set; }
        public decimal VTADACM_AccommodationAmt { get; set; }
        public decimal VTADACM_TransportAmt { get; set; }
        public string returnval { get; set; }
        public bool returnvalue { get; set; }
        public Array Designation_types { get; set; }
         public Array City_Type { get; set; }
        public Array Get_CityMapping { get; set; }
        public Array state { get; set; }
        public Array Get_MasterReport { get; set; }
             public long IVRMMS_Id { get; set; }
        public Array editfiles { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string IVRMMCT_Name { get; set; }
        public long IVRMMC_Id { get; set; }
        public bool VTADACM_ActiveFlag { get; set; }
        public selectedcitylist[] city_array { get; set; }
        public string message { get; set; }
        public Array Master_Instiute { get; set; }
        public string company_name { get; set; }
        public DesighnList[] List { get; set; }
        public string MI_Logo { get; set; }
        public decimal? VTADACM_TravelHour { get; set; }


    }
    //public selecteddetails[] AllowenceArray { get; set; }
    public class DesighnList
    {
        public long HRMDES_IdS { get; set; }
    }
    public class selectedcitylist
    {
        public long IVRMMCT_Id { get; set; }
        public string IVRMMCT_Name { get; set; }

    }
}
