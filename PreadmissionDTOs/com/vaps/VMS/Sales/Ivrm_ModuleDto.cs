using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.Sales
{
    public class Ivrm_ModuleDto
    {
        public Array ivrmData { get; set; }
        public Array ism_SalesData { get; set; }

        public Array MasterData { get; set; }
        public long MI_Id { get; set; }
        public long User_Id { get; set; }
        public long IVRMM_Id { get; set; }
        public string IVRMM_ModuleName { get; set; }
        public string IVRMM_ModuleDesc { get; set; }
        public int Module_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long userid { get; set; }
        public long IVRMM_CreatedBy { get; set; }
        public long IVRMM_UpdatedBy { get; set; }

        public long IMMB_IVRMMID { get; set; }

        //ISM_Master_Module_BOM
        public long IMMB_ID { get; set; }
        public long IMMB_IVRMMIDS { get; set; }
        public long IMMB_BOMID { get; set; }
        public string IMMB_InsType { get; set; }
        public bool IMMB_ActiveFlag { get; set; }

        //array
        public ivrmIds[] IVRMMIDS { get; set; }

       // public Array ivrmM_Id { get; set; }

        
        public string returndata { get; set; }
        public int? IMMB_IVRMMID_Order { get; set; }

        public moduleorder[] ModuleDetails { get; set; }

        public class ivrmIds
        {
            //public long ModuleID { get; set; }
            public long ivrmM_Id { get; set; }
        }
        public class moduleorder
        {
            public long? IVRMM_Id { get; set; }
            public long? IMMB_ID { get; set; }
            public long? IMMB_IVRMMID { get; set; }
            public long? IMMB_BOMID { get; set; }
            public int? IMMB_IVRMMID_Order { get; set; }
        }
    }
}
