using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_NotificationInterface
    {
        Task<ISM_NotificationDTO> getdetails(ISM_NotificationDTO data);
        Task<ISM_NotificationDTO> getnotificationdetails(ISM_NotificationDTO data);
        Task<ISM_NotificationDTO> pushnotificationdetails(ISM_NotificationDTO data);
        Task<ISM_NotificationDTO> getplannernotgenerated(ISM_NotificationDTO data);
        Task<ISM_NotificationDTO> emialnotificationfordailyreportblock (ISM_NotificationDTO data);
        Task<ISM_NotificationDTO> getplannernotapproved(ISM_NotificationDTO data);
        Task<ISM_NotificationDTO> getplannernotapprovedmd(ISM_NotificationDTO data);
        Task<ISM_NotificationDTO> getdailyreportnotapproved(ISM_NotificationDTO data);
        Task<ISM_NotificationDTO> unauthorizednotice(ISM_NotificationDTO data);
        Task<ISM_NotificationDTO> lateinearlyoutnotice(ISM_NotificationDTO data);
        Task<ISM_NotificationDTO> noticeProbition(ISM_NotificationDTO data);
    }
}
