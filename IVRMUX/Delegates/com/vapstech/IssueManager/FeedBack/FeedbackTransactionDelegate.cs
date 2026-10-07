using CommonLibrary;
using PreadmissionDTOs.FeedBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.FeedBack
{
    public class FeedbackTransactionDelegate
    {
        CommonDelegate<FeedbackTransactionDTO, FeedbackTransactionDTO> _com = new CommonDelegate<FeedbackTransactionDTO, FeedbackTransactionDTO>();

        public FeedbackTransactionDTO getdetails(FeedbackTransactionDTO data)
        {
            return _com.POSTIssueManager(data, "FeedbackTransactionFacade/getdetails");
        }
        public FeedbackTransactionDTO getfeedback(FeedbackTransactionDTO data)
        {
            return _com.POSTIssueManager(data, "FeedbackTransactionFacade/getfeedback");
        }
        
        public FeedbackTransactionDTO savedata(FeedbackTransactionDTO data)
        {
            return _com.POSTIssueManager(data, "FeedbackTransactionFacade/savedata");
        }

        public FeedbackTransactionDTO getstudentstaffdetails(FeedbackTransactionDTO data)
        {
            return _com.POSTIssueManager(data, "FeedbackTransactionFacade/getstudentstaffdetails");
        }
        public FeedbackTransactionDTO getstaffname(FeedbackTransactionDTO data)
        {
            return _com.POSTIssueManager(data, "FeedbackTransactionFacade/getstaffname");
        }
        public FeedbackTransactionDTO getfeedbacksubject(FeedbackTransactionDTO data)
        {
            return _com.POSTIssueManager(data, "FeedbackTransactionFacade/getfeedbacksubject");
        }
        public FeedbackTransactionDTO studentstaffsavedata(FeedbackTransactionDTO data)
        {
            return _com.POSTIssueManager(data, "FeedbackTransactionFacade/studentstaffsavedata");
        }
        
    }
}
