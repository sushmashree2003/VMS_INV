using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.MOM
{
    public class MOM_TransactionDelegate
    {
        CommonDelegate<MOM_TransactionDTO, MOM_TransactionDTO> COMMM = new CommonDelegate<MOM_TransactionDTO, MOM_TransactionDTO>();


        public MOM_TransactionDTO getDetails(MOM_TransactionDTO data)
        {
            return COMMM.POSTIssueManager(data, "MOM_TransactionFacade/getDetails/");
        }

        public MOM_TransactionDTO savedata(MOM_TransactionDTO data)
        {
            return COMMM.POSTIssueManager(data, "MOM_TransactionFacade/savedata/");
        }
    }
}
