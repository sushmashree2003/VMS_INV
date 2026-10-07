using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class VMS_SalaryCheck_ApprovalDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Employee_Salary_PaymentDetailsDTO, HR_Employee_Salary_PaymentDetailsDTO> COMMM = new CommonDelegate<HR_Employee_Salary_PaymentDetailsDTO, HR_Employee_Salary_PaymentDetailsDTO>();

        public HR_Employee_Salary_PaymentDetailsDTO onloadgetdetails(HR_Employee_Salary_PaymentDetailsDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "VMS_SalaryCheck_ApprovalFacade/onloadgetdetails");
        }

        //getEmployeedetailsBySelection
        public HR_Employee_Salary_PaymentDetailsDTO getEmployeedetailsBySelection(HR_Employee_Salary_PaymentDetailsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "VMS_SalaryCheck_ApprovalFacade/getEmployeedetailsBySelection/");
        }
        
        public HR_Employee_Salary_PaymentDetailsDTO saveapproval(HR_Employee_Salary_PaymentDetailsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "VMS_SalaryCheck_ApprovalFacade/saveapproval/");
        }
    }
}
