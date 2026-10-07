using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates
{
    public class VMS_Live_InterviewDelegates
    {
        CommonDelegate<VMS_Live_InterviewDTO, VMS_Live_InterviewDTO> COMM = new CommonDelegate<VMS_Live_InterviewDTO, VMS_Live_InterviewDTO>();
        public VMS_Live_InterviewDTO Getdetails (VMS_Live_InterviewDTO dto)
        {
            return COMM.POSTVMS(dto, "VMS_Live_InterviewFacade/Getdetails/");
        }
        public VMS_Live_InterviewDTO GetOnloaddata(VMS_Live_InterviewDTO dto)
        {
            return COMM.POSTVMS(dto, "VMS_Live_InterviewFacade/GetOnloaddata/");
        }

        public VMS_Live_InterviewDTO savemeeting(VMS_Live_InterviewDTO dto)
        {
            return COMM.POSTVMS(dto, "VMS_Live_InterviewFacade/savemeeting/");
        }
        public VMS_Live_InterviewDTO getempdetails(VMS_Live_InterviewDTO dto)
        {
            return COMM.POSTVMS(dto, "VMS_Live_InterviewFacade/getempdetails/");
        }
        public VMS_Live_InterviewDTO onstartmeeting(VMS_Live_InterviewDTO dto)
        {
            return COMM.POSTVMS(dto, "VMS_Live_InterviewFacade/onstartmeeting/");
        }
        public VMS_Live_InterviewDTO endmainmeeting(VMS_Live_InterviewDTO dto)
        {
            return COMM.POSTVMS(dto, "VMS_Live_InterviewFacade/endmainmeeting/");
        }
        public VMS_Live_InterviewDTO getempdetails_join(VMS_Live_InterviewDTO dto)
        {
            return COMM.POSTVMS(dto, "VMS_Live_InterviewFacade/getempdetails_join/");
        }
        public VMS_Live_InterviewDTO onstartmeeting_join(VMS_Live_InterviewDTO dto)
        {
            return COMM.POSTVMS(dto, "VMS_Live_InterviewFacade/onstartmeeting_join/");
        }
         public VMS_Live_InterviewDTO endmainmeeting_join(VMS_Live_InterviewDTO dto)
        {
            return COMM.POSTVMS(dto, "VMS_Live_InterviewFacade/endmainmeeting_join/");
        }
         public VMS_Live_InterviewDTO GetReport(VMS_Live_InterviewDTO dto)
        {
            return COMM.POSTVMS(dto, "VMS_Live_InterviewFacade/GetReport/");
        }

    }
}
