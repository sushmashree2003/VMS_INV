using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates
{
    public class VMS_Live_MeetingDelegates
    {
        CommonDelegate<VMS_Live_MeetingDTO, VMS_Live_MeetingDTO> COMM = new CommonDelegate<VMS_Live_MeetingDTO, VMS_Live_MeetingDTO>();
        public VMS_Live_MeetingDTO Getdetails (VMS_Live_MeetingDTO dto)
        {
            return COMM.POSTIssueManager(dto, "VMS_Live_MeetingFacade/Getdetails/");
        }

        public VMS_Live_MeetingDTO savemeeting(VMS_Live_MeetingDTO dto)
        {
            return COMM.POSTIssueManager(dto, "VMS_Live_MeetingFacade/savemeeting/");
        }
        public VMS_Live_MeetingDTO getempdetails(VMS_Live_MeetingDTO dto)
        {
            return COMM.POSTIssueManager(dto, "VMS_Live_MeetingFacade/getempdetails/");
        }
        public VMS_Live_MeetingDTO onstartmeeting(VMS_Live_MeetingDTO dto)
        {
            return COMM.POSTIssueManager(dto, "VMS_Live_MeetingFacade/onstartmeeting/");
        }
        public VMS_Live_MeetingDTO endmainmeeting(VMS_Live_MeetingDTO dto)
        {
            return COMM.POSTIssueManager(dto, "VMS_Live_MeetingFacade/endmainmeeting/");
        }
        public VMS_Live_MeetingDTO getempdetails_join(VMS_Live_MeetingDTO dto)
        {
            return COMM.POSTIssueManager(dto, "VMS_Live_MeetingFacade/getempdetails_join/");
        }
        public VMS_Live_MeetingDTO onstartmeeting_join(VMS_Live_MeetingDTO dto)
        {
            return COMM.POSTIssueManager(dto, "VMS_Live_MeetingFacade/onstartmeeting_join/");
        }
         public VMS_Live_MeetingDTO endmainmeeting_join(VMS_Live_MeetingDTO dto)
        {
            return COMM.POSTIssueManager(dto, "VMS_Live_MeetingFacade/endmainmeeting_join/");
        }
         public VMS_Live_MeetingDTO GetReport(VMS_Live_MeetingDTO dto)
        {
            return COMM.POSTIssueManager(dto, "VMS_Live_MeetingFacade/GetReport/");
        }

    }
}
