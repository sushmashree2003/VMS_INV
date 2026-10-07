using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class IndentApprovalDelegate
    {
        CommonDelegate<IndentApprovalDTO, IndentApprovalDTO> COMINV = new CommonDelegate<IndentApprovalDTO, IndentApprovalDTO>();
        public IndentApprovalDTO getloaddata(IndentApprovalDTO data)
        {
            return COMINV.POSTDataInventory(data, "IndentApprovalFacade/getloaddata/");
        }

        
        public IndentApprovalDTO getpidetails(IndentApprovalDTO data)
        {
            return COMINV.POSTDataInventory(data, "IndentApprovalFacade/getpidetails/");
        }
        public IndentApprovalDTO savedetails(IndentApprovalDTO data)
        {
            return COMINV.POSTDataInventory(data, "IndentApprovalFacade/savedetails/");
        }
        public IndentApprovalDTO getprDetail(IndentApprovalDTO data)
        {
            return COMINV.POSTDataInventory(data, "IndentApprovalFacade/getprDetail/");
        }
        public IndentApprovalDTO deactiveM(IndentApprovalDTO data)
        {
            return COMINV.POSTDataInventory(data, "IndentApprovalFacade/deactiveM/");
        }
        public IndentApprovalDTO deactive(IndentApprovalDTO data)
        {
            return COMINV.POSTDataInventory(data, "IndentApprovalFacade/deactive/");
        }
        public IndentApprovalDTO get_details(IndentApprovalDTO data)
        {
            return COMINV.POSTDataInventory(data, "IndentApprovalFacade/get_details/");
        }
        public IndentApprovalDTO edit(IndentApprovalDTO data)
        {
            return COMINV.POSTDataInventory(data, "IndentApprovalFacade/edit/");
        }
        public IndentApprovalDTO rejectallreq(IndentApprovalDTO data)
        {
            return COMINV.POSTDataInventory(data, "IndentApprovalFacade/rejectallreq/");
        }
        public IndentApprovalDTO OnClickIndividualViewComments(IndentApprovalDTO data)
        {
            return COMINV.POSTDataInventory(data, "IndentApprovalFacade/OnClickIndividualViewComments/");
        }
    }
}