using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
   public interface EmailDeviationInterface
    {
        EmailDeviationDTO sendeach(EmailDeviationDTO data);
        EmailDeviationDTO updatepriority(EmailDeviationDTO data);
        EmailDeviationDTO blockdailyreport(EmailDeviationDTO data);
        EmailDeviationDTO makercheckerverification(EmailDeviationDTO data);       
        EmailDeviationDTO Sendnotassignedtask(EmailDeviationDTO data);       
        EmailDeviationDTO Sendnotassignedtask_Critical(EmailDeviationDTO data);       
        EmailDeviationDTO senddepartmentapproval(EmailDeviationDTO data);       
    }
}
