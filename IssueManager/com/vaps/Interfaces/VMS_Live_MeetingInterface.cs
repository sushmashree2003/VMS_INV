using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface VMS_Live_MeetingInterface
    {
        VMS_Live_MeetingDTO Getdetails(VMS_Live_MeetingDTO dto);
        VMS_Live_MeetingDTO savemeeting(VMS_Live_MeetingDTO dto);
        VMS_Live_MeetingDTO getempdetails(VMS_Live_MeetingDTO dto);
        VMS_Live_MeetingDTO onstartmeeting(VMS_Live_MeetingDTO dto);
        VMS_Live_MeetingDTO endmainmeeting(VMS_Live_MeetingDTO dto);
        VMS_Live_MeetingDTO getempdetails_join(VMS_Live_MeetingDTO dto);
        VMS_Live_MeetingDTO onstartmeeting_join(VMS_Live_MeetingDTO dto);
        VMS_Live_MeetingDTO endmainmeeting_join(VMS_Live_MeetingDTO dto);
        VMS_Live_MeetingDTO GetReport(VMS_Live_MeetingDTO dto);
    }
}
