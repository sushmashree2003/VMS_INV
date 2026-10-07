using PreadmissionDTOs.FeedBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.Feedback.Interface
    {
        public interface EmployeeHeadFeedBackInterface
        {
          
            Task<EmployeeHeadFeedBackDTO> getdetails(EmployeeHeadFeedBackDTO data);
        EmployeeHeadFeedBackDTO savedetails(EmployeeHeadFeedBackDTO data);
        EmployeeHeadFeedBackDTO delete_data(int id);

    }
    }
