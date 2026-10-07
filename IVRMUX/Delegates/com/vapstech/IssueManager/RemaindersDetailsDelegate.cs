using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class RemaindersDetailsDelegate
    {
        CommonDelegate<RemaindersDetailsDTO, RemaindersDetailsDTO> _comm = new CommonDelegate<RemaindersDetailsDTO, RemaindersDetailsDTO>();

        public RemaindersDetailsDTO GetDetails(RemaindersDetailsDTO data)
        {
            return _comm.POSTIssueManager(data, "RemaindersDetailsFacade/GetDetails");
        }
        public RemaindersDetailsDTO OnChangeClient(RemaindersDetailsDTO data)
        {
            return _comm.POSTIssueManager(data, "RemaindersDetailsFacade/OnChangeClient");
        }
        public RemaindersDetailsDTO SaveData(RemaindersDetailsDTO data)
        {
            return _comm.POSTIssueManager(data, "RemaindersDetailsFacade/SaveData");
        }
    }
}
