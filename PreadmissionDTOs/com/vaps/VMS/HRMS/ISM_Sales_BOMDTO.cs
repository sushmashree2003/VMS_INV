using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class ISM_Sales_BOMDTO
    {

        public long User_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMSPS_ID { get; set; }
        public long ISMSPSI_ID { get; set; }
        public long ISMSBOM_Id { get; set; }
        public long ISMSMPR_Id { get; set; }
        
        public string ISMSPS_Service_Name { get; set; }
        public string ISMSBOM_BOMName { get; set; }
        public string ISMSBOM_BOMDescription { get; set; }
        public string ISMSMPR_ProductName { get; set; }
        
        public string ISMSPS_Service_Description { get; set; }
        public bool ISMSPS_ActiveFlag { get; set; }
      
        public bool ISMSPSI_ActiveFlag { get; set; }
        public bool ISMSBOM_ActiveFlag { get; set; }
        public long AbvStrength { get; set; }
        

        public Array Productlist { get; set; }
        public Array ServiceList { get; set; }
        public Array Serviceitemlist { get; set; }
        public Array BOMlist { get; set; }
        public Array viewservicedetailslist { get; set; }
        public Iteamsavedfiless[] Iteamsavedfiles { get;set;}
        public serviceListtwos[] serviceListtwo { get; set; }

        public string retrunMsg { get; set; }
        public bool returnval { get; set; }


        public Array Sales_BOM_edits { get; set; }



    }
    public class Iteamsavedfiless
    {
        public string ISMSPSI_ItemsName { get; set; }
        public string ISMSPSI_ItemsDescription { get; set; }
        public decimal ISMSPSI_Quantity { get; set; }
        public decimal ISMSPSI_Cost { get; set; }
        public decimal ISMSPSI_AMC_Cost { get; set; }
        public bool ISMSPSI_AMC_Flag { get; set; }

        public string ISMSPSI_ItemFlag { get; set; }

    }

    public class serviceListtwos
    {
        public long ISMSPS_ID { get; set; }
        public long ISMSBOM_Id { get; set; }
        
        public string ISMSPS_Service_Name { get; set; }

    }
}
