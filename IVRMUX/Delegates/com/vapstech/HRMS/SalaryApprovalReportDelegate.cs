using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class SalaryApprovalReportDelegate
    {
        CommonDelegate<HR_Employee_SalaryDTO, HR_Employee_SalaryDTO> COMFRNT = new CommonDelegate<HR_Employee_SalaryDTO, HR_Employee_SalaryDTO>();
        public HR_Employee_SalaryDTO getalldetails(HR_Employee_SalaryDTO data)
        {
            return COMFRNT.POSTDataHRMS(data, "CumulativeSalaryReportFacade/onloadgetdetails/");
        }

        public HR_Employee_SalaryDTO report(HR_Employee_SalaryDTO data)
        {
            return COMFRNT.POSTDataHRMS(data, "CumulativeSalaryReportFacade/report/");
        }

    }
}
