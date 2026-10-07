using CommonLibrary;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.FrontOffice
{
    public class AttendanceApprovalReportDelegate
    {
        CommonDelegate<AttendanceApprovalReportDTO, AttendanceApprovalReportDTO> COMFRNT = new CommonDelegate<AttendanceApprovalReportDTO, AttendanceApprovalReportDTO>();

        public AttendanceApprovalReportDTO getdata(AttendanceApprovalReportDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "AttendanceApprovalReportFacade/getalldetails/");
        }

     public AttendanceApprovalReportDTO report(AttendanceApprovalReportDTO data)
         {
            return COMFRNT.POSTDataHolidayReport(data, "AttendanceApprovalReportFacade/report/");
        }
    }
}
