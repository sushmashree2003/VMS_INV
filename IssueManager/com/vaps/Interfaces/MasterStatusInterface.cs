using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface MasterStatusInterface
    {
        MasterStatus_DTO getdetails(MasterStatus_DTO dTO);
        MasterStatus_DTO saverecord(MasterStatus_DTO data);
        MasterStatus_DTO deactiveY(MasterStatus_DTO data);
        MasterStatus_DTO save_ack(MasterStatus_DTO data);

    }
}
