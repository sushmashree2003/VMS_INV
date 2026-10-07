using CommonLibrary;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.LeaveManagement
{
    public class Dept_HeadWiseLeaveReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Dept_HeadWiseLeaveReportDTO, Dept_HeadWiseLeaveReportDTO> COMMM = new CommonDelegate<Dept_HeadWiseLeaveReportDTO, Dept_HeadWiseLeaveReportDTO>();
        
        public Dept_HeadWiseLeaveReportDTO getdatedetails(Dept_HeadWiseLeaveReportDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "Dept_HeadWiseLeaveReportFacade/getdatedetails/");
        }
    }
}
