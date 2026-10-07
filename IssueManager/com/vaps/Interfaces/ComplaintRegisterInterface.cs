using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace IssueManager.com.vaps.Interfaces
{
    public interface ComplaintRegisterInterface
    {
        ComplaintRegister_DTO save(ComplaintRegister_DTO data);
    }
}
