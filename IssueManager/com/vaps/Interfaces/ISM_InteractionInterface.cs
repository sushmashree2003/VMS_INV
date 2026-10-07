using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_InteractionInterface
    {
        Task<ISM_InteractionDTO> getdetails(ISM_InteractionDTO data);           
        ISM_InteractionDTO savedata(ISM_InteractionDTO data); 
        Task<ISM_InteractionDTO> get_replydetails(ISM_InteractionDTO data);
        ISM_InteractionDTO savereply(ISM_InteractionDTO data);

        //====================feedback================
        ISM_InteractionDTO getfeedbac(ISM_InteractionDTO data);
        ISM_InteractionDTO feedback_msg_details(ISM_InteractionDTO data);
        Task<ISM_InteractionDTO> desktopnotification(ISM_InteractionDTO data);
        ISM_InteractionDTO Sendfeedback_tfr(ISM_InteractionDTO data);
        ISM_InteractionDTO getViewfeedback(ISM_InteractionDTO data);

        //====================notification================
        ISM_InteractionDTO getnotification(ISM_InteractionDTO data);
        ISM_InteractionDTO clicknotification(ISM_InteractionDTO data);
    }
}
