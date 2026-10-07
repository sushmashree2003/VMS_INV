using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
        public class ActiveDeactiveEmployeeReportDelgate
        {
            CommonDelegate<ActiveDeactiveEmployeeReportDTO, ActiveDeactiveEmployeeReportDTO> COMFRNT = new CommonDelegate<ActiveDeactiveEmployeeReportDTO, ActiveDeactiveEmployeeReportDTO>();
            public ActiveDeactiveEmployeeReportDTO getalldetails(ActiveDeactiveEmployeeReportDTO data)
            {
                return COMFRNT.POSTDataHRMS(data, "ActiveDeactiveEmployeeReportFacade/getalldetails/");
            }
        
            public ActiveDeactiveEmployeeReportDTO report(ActiveDeactiveEmployeeReportDTO data)
            { 
            return COMFRNT.POSTDataHRMS(data, "ActiveDeactiveEmployeeReportFacade/report/");
            }

        }
    
}
