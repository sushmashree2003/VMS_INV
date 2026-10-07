using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.VMS_Tracking.Interface
{
    public interface Fee_Payment_ManagementInterface
    {
        Fee_Payment_ManagementDTO getloaddata(Fee_Payment_ManagementDTO dto);
        Fee_Payment_ManagementDTO getreport(Fee_Payment_ManagementDTO dto);
    }
}
