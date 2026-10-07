 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.FeedBack;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.FeedBack
    {
        public class EmployeeHeadFeedBackDelegate
        {
            CommonDelegate<EmployeeHeadFeedBackDTO, EmployeeHeadFeedBackDTO> _comm = new CommonDelegate<EmployeeHeadFeedBackDTO, EmployeeHeadFeedBackDTO>();

        

            public EmployeeHeadFeedBackDTO getdetails(EmployeeHeadFeedBackDTO data)
            {
                return _comm.POSTIssueManager(data, "EmployeeHeadFeedBackFacade/getdetails/");
            }




        public EmployeeHeadFeedBackDTO savedetails(EmployeeHeadFeedBackDTO data)
        {
            return _comm.POSTIssueManager(data, "EmployeeHeadFeedBackFacade/savedetails/");
        }
        public EmployeeHeadFeedBackDTO delete_data(int id)
        {

            return _comm.GetIssueManager(id, "EmployeeHeadFeedBackFacade/delete_data/");
        }
    }
    }
