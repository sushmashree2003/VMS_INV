using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.Tenants
{
    public class RoomWiseAmenitiesMappingDTO
    {
       
     
        public long MI_Id { get; set; }
        public long HRMB_Id { get; set; }
        public long HRMR_Id { get; set; }
      
        public long UserId { get; set; }
        
        public Array rooms { get; set; }
        public Array building { get; set; }
        public Array get_org { get; set; }
        public long[] HRMRAM_Array { get; set; }
        public Array get_levl { get; set; }
        public string HRMR_RoomName { get; set; }
        
        public long HRMAM_Id { get; set; }

        public string HRMAM_AmenitiesName { get; set; }
        public long HRMRAM_Id { get; set; }
        public decimal HRMRAM_RentPerDay { get; set; }
        public long HRMRAM_NoOfHrs { get; set; }
        public decimal HRMRAM_RentPerHour { get; set; }
        public long User_Id { get; set; }
        public bool HRMAM_ActiveFlag { get; set; }
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        public string HRMRAM_filepath { get; set; }

        public Array getloaddetails { get; set; }
        // public HRMRAM_Array[] HRMRAM_Array { get; set; }
    }

 

    //public class HRMRAM_Array
    //{
    //    public long HRMAM_Id { get; set; }
       
    //}

}
