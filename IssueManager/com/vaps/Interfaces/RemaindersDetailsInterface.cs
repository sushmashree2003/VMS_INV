using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface RemaindersDetailsInterface
    {
        RemaindersDetailsDTO GetDetails(RemaindersDetailsDTO data);
        RemaindersDetailsDTO OnChangeClient(RemaindersDetailsDTO data);
        RemaindersDetailsDTO SaveData(RemaindersDetailsDTO data);
        RemaindersDetailsDTO ClientSubscriptionMail(RemaindersDetailsDTO data);
    }
}
