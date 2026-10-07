using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_R_GRNDelegate
    {
        CommonDelegate<INV_T_GRNDTO, INV_T_GRNDTO> COMINV = new CommonDelegate<INV_T_GRNDTO, INV_T_GRNDTO>();
        public INV_T_GRNDTO getloaddata(INV_T_GRNDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_R_GRNFacade/getloaddata/");
        }
        public INV_T_GRNDTO onreport(INV_T_GRNDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_R_GRNFacade/onreport/");
        }

        
    }
}
