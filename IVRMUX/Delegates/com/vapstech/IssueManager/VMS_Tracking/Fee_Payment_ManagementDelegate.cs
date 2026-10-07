using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.VMS_Tracking
{
    public class Fee_Payment_ManagementDelegate
    {
        CommonDelegate<Fee_Payment_ManagementDTO, Fee_Payment_ManagementDTO> COMM = new CommonDelegate<Fee_Payment_ManagementDTO, Fee_Payment_ManagementDTO>();

        public Fee_Payment_ManagementDTO getloaddata (Fee_Payment_ManagementDTO dto)
        {
            return COMM.POSTIssueManager(dto, "Fee_Payment_ManagementFacade/getloaddata");
        }
        public Fee_Payment_ManagementDTO getreport(Fee_Payment_ManagementDTO dto)
        {
            return COMM.POSTIssueManager(dto, "Fee_Payment_ManagementFacade/getreport");
        }

    }
}
