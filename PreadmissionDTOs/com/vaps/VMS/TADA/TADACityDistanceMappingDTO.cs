using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.TADA
{
   public class TADACityDistanceMappingDTO
    {
        public long VTADAMC_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public bool Flag { get; set; }  
        public long HRME_Id { get; set; }
        public string Categoryname { get; set; }
        public Array StateList { get; set; }
        public Array CityList { get; set; }
        public string VTADAMC_FromState { get; set; }
        public string VTADAMC_ToState { get; set; }
        public Array Getloaddata { get; set; }
        public long IVRMMS_Id { get; set; }
        public bool returnval { get; set; }
        public long Userid { get; set; }
        public string Message { get; set; }
        public long VRMMDT_Id { get; set; }
        public Array editdesign { get; set; }
        public Array Statedatalist { get; set; }
        public Array citynames { get; set; }
        public Array TADA_Traveltier { get; set; }
        public string IVRMMCT_Name { get; set; }
        
        public Array Travel_Tier { get; set; }
        public Array Design_List { get; set; }
        public Array EditDetails { get; set; }
        public Array companylist { get; set; }
        public long VTADAMT_Id { get; set; }
        public Array Travel_data { get; set; }
        public Array Travel_Details { get; set; }
        public Array Travelstatedetails { get; set; }
        public Array Designation { get; set; }
        public long HRMDES_Id { get; set; }
        public long VTADAMD_Id { get; set; }
        public long VTADAMC_CreatedBy { get; set; }
        public long VTADAMC_UpdatedBy { get; set; }
        public DateTime VTADAMC_CreatedDate { get; set; }
        public DateTime VTADAMC_UpdatedDate { get; set; }
        public bool duplicate { get; set; }
        public Array Tadaeditdetails { get; set; }
        public long IVRMMCT_Id { get; set; }
        public string Cityname  { get; set; }
        public decimal amount { get; set; }
        public TADACityDistanceMappingDTO[] Designationlist { get; set; }
        public TADACityDistanceMappingDTO[] Traveldatalist { get; set; }
        public TADACityDistanceMappingDTO[] TravelCity { get; set; }
        public TADACityDisDTO[] TADACityDistanceDTO { get; set; }
        public TADATravelAmountDTO[] TADATravelAmount { get; set; }
        
        public class TADACityDisDTO
        {
            public long VTADAMC_Id { get; set; }
            public string VTADAMC_City1 { get; set; }
            public string VTADAMC_City2 { get; set; }
            public string VTADAMC_State1 { get; set; }
            public string VTADAMC_State2 { get; set; }
            public long VTADAMC_DistanceKM { get; set; }
            public long VTADAMC_CreatedBy { get; set; }
            public long VTADAMC_UpdatedBy { get; set; }
            public DateTime VTADAMC_CreatedDate { get; set; }
            public DateTime VTADAMC_UpdatedDate { get; set; }
        }

        public class TADATravelAmountDTO
        {
            public decimal Amount { get; set; }
            public long VTADAMT_Id { get; set; }
            public long VTADAMD_Id { get; set; }    
            
        }

    }
}
