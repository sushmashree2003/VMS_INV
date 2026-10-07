using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.Tenants
{
    public class RoomBankDeatilsDelegate
    {
        CommonDelegate<RoomBankDeatilsDTO, RoomBankDeatilsDTO> _comm = new CommonDelegate<RoomBankDeatilsDTO, RoomBankDeatilsDTO>();




        public RoomBankDeatilsDTO getloaddata(RoomBankDeatilsDTO data)
        {
            return _comm.POSTIssueManager(data, "RoomBankDeatilsFacade/getloaddata");
        }

        public RoomBankDeatilsDTO savedetails(RoomBankDeatilsDTO data)
        {
            return _comm.POSTIssueManager(data, "RoomBankDeatilsFacade/savedata/");
        }
        public RoomBankDeatilsDTO deactive(RoomBankDeatilsDTO data)
        {
            return _comm.POSTIssueManager(data, "RoomBankDeatilsFacade/deactive/");
        }
    }
}


