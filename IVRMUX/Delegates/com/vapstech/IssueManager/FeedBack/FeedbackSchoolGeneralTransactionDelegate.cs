using CommonLibrary;
using PreadmissionDTOs.FeedBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace IVRMUX.Delegates.FeedBack
{
    public class FeedbackSchoolGeneralTransactionDelegate
    {
        CommonDelegate<FeedbackSchoolGeneralTransactionDTO, FeedbackSchoolGeneralTransactionDTO> _com = new CommonDelegate<FeedbackSchoolGeneralTransactionDTO, FeedbackSchoolGeneralTransactionDTO>();

        public FeedbackSchoolGeneralTransactionDTO getdetails(FeedbackSchoolGeneralTransactionDTO data)
        {
            return _com.POSTIssueManager(data, "FeedbackSchoolGeneralTransactionFacade/getdetails");
        }
        public FeedbackSchoolGeneralTransactionDTO getfeedback(FeedbackSchoolGeneralTransactionDTO data)
        {
            return _com.POSTIssueManager(data, "FeedbackSchoolGeneralTransactionFacade/getfeedback");
        }
        
        public FeedbackSchoolGeneralTransactionDTO savedata(FeedbackSchoolGeneralTransactionDTO data)
        {
            return _com.POSTIssueManager(data, "FeedbackSchoolGeneralTransactionFacade/savedata");
        }

        public FeedbackSchoolGeneralTransactionDTO getstudentstaffdetails(FeedbackSchoolGeneralTransactionDTO data)
        {
            return _com.POSTIssueManager(data, "FeedbackSchoolGeneralTransactionFacade/getstudentstaffdetails");
        }
        public FeedbackSchoolGeneralTransactionDTO getstaffname(FeedbackSchoolGeneralTransactionDTO data)
        {
            return _com.POSTIssueManager(data, "FeedbackSchoolGeneralTransactionFacade/getstaffname");
        }
        public FeedbackSchoolGeneralTransactionDTO getfeedbacksubject(FeedbackSchoolGeneralTransactionDTO data)
        {
            return _com.POSTIssueManager(data, "FeedbackSchoolGeneralTransactionFacade/getfeedbacksubject");
        }
        
        public FeedbackSchoolGeneralTransactionDTO studentstaffsavedata(FeedbackSchoolGeneralTransactionDTO data)
        {
            return _com.POSTIssueManager(data, "FeedbackSchoolGeneralTransactionFacade/studentstaffsavedata");
        }


        //Feedback_Staff_Transaction
        public FeedbackSchoolGeneralTransactionDTO getFeedback_Staff_Transactiondetails(FeedbackSchoolGeneralTransactionDTO data)
        {
            return _com.POSTIssueManager(data, "FeedbackSchoolGeneralTransactionFacade/getFeedback_Staff_Transactiondetails");
        }
        public FeedbackSchoolGeneralTransactionDTO saveFeedback_Staff_Transactiondetails(FeedbackSchoolGeneralTransactionDTO data)
        {
            return _com.POSTIssueManager(data, "FeedbackSchoolGeneralTransactionFacade/saveFeedback_Staff_Transactiondetails");
        }

        public FeedbackSchoolGeneralTransactionDTO certificateFeedback_Staff_Transactiondetails(FeedbackSchoolGeneralTransactionDTO data)
        {
            return _com.POSTIssueManager(data, "FeedbackSchoolGeneralTransactionFacade/certificateFeedback_Staff_Transactiondetails");
        }

        public FeedbackSchoolGeneralTransactionDTO getfeedback1(FeedbackSchoolGeneralTransactionDTO data)
        {
            return _com.POSTIssueManager(data, "FeedbackSchoolGeneralTransactionFacade/getfeedback1");
        }
        public FeedbackSchoolGeneralTransactionDTO savecanteen(FeedbackSchoolGeneralTransactionDTO data)
        {
            return _com.POSTIssueManager(data, "FeedbackSchoolGeneralTransactionFacade/savecanteen");
        }
     
    }
}
