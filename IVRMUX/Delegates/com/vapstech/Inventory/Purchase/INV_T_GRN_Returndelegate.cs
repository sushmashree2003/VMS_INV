using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory.Purchase
{
    public class INV_T_GRN_Returndelegate
    {

        CommonDelegate<GRN_ReturnDTO, GRN_ReturnDTO> COMINV = new CommonDelegate<GRN_ReturnDTO, GRN_ReturnDTO>();
        public GRN_ReturnDTO getloaddata(GRN_ReturnDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_T_GRN_ReturnFacade/getloaddata/");
        }
        public GRN_ReturnDTO getitem(GRN_ReturnDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_T_GRN_ReturnFacade/getitem/");
        }
        public GRN_ReturnDTO getitemDetail(GRN_ReturnDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_T_GRN_ReturnFacade/getitemDetail/");
        }
        public GRN_ReturnDTO savedetails(GRN_ReturnDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_T_GRN_ReturnFacade/savedetails/");
        }
        public GRN_ReturnDTO get_GRNitemDetails(GRN_ReturnDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_T_GRN_ReturnFacade/get_GRNitemDetails/");
        }
         public GRN_ReturnDTO deactive(GRN_ReturnDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_T_GRN_ReturnFacade/deactive/");
        }
        
       
       
    }
}
