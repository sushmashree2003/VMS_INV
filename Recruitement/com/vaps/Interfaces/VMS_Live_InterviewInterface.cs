using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface VMS_Live_InterviewInterface
    {
        VMS_Live_InterviewDTO Getdetails(VMS_Live_InterviewDTO dto);
        VMS_Live_InterviewDTO GetOnloaddata(VMS_Live_InterviewDTO dto);
        VMS_Live_InterviewDTO savemeeting(VMS_Live_InterviewDTO dto);
        VMS_Live_InterviewDTO getempdetails(VMS_Live_InterviewDTO dto);
        VMS_Live_InterviewDTO onstartmeeting(VMS_Live_InterviewDTO dto);
        VMS_Live_InterviewDTO endmainmeeting(VMS_Live_InterviewDTO dto);
        VMS_Live_InterviewDTO getempdetails_join(VMS_Live_InterviewDTO dto);
        VMS_Live_InterviewDTO onstartmeeting_join(VMS_Live_InterviewDTO dto);
        VMS_Live_InterviewDTO endmainmeeting_join(VMS_Live_InterviewDTO dto);
        VMS_Live_InterviewDTO GetReport(VMS_Live_InterviewDTO dto);
    }
}
