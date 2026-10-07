using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_PO_ToSupplierDelegate
    {
        CommonDelegate<INV_PO_ToSupplierDTO, INV_PO_ToSupplierDTO> COMINV = new CommonDelegate<INV_PO_ToSupplierDTO, INV_PO_ToSupplierDTO>();
        public INV_PO_ToSupplierDTO getloaddata(INV_PO_ToSupplierDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PO_ToSupplierFacade/getloaddata/");
        }
        public INV_PO_ToSupplierDTO getpiDetail(INV_PO_ToSupplierDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PO_ToSupplierFacade/getpiDetail/");
        }
        public INV_PO_ToSupplierDTO savedetails(INV_PO_ToSupplierDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PO_ToSupplierFacade/savedetails/");
        }
   
        public INV_PO_ToSupplierDTO deactive(INV_PO_ToSupplierDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PO_ToSupplierFacade/deactive/");
        }
        public INV_PO_ToSupplierDTO viewtemplatedetails(INV_PO_ToSupplierDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PO_ToSupplierFacade/viewtemplatedetails/");
        }
      public INV_PO_ToSupplierDTO sendmail(INV_PO_ToSupplierDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PO_ToSupplierFacade/sendmail/");
        }
     

        

    }
}
