using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;


namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class SalaryAdvanceApprovalDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Emp_SalaryAdvanceDTO, HR_Emp_SalaryAdvanceDTO> COMMM = new CommonDelegate<HR_Emp_SalaryAdvanceDTO, HR_Emp_SalaryAdvanceDTO>();

        public HR_Emp_SalaryAdvanceDTO onloadgetdetails(HR_Emp_SalaryAdvanceDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "SalaryAdvanceApprovalFacade/onloadgetdetails");
        }
        public HR_Emp_SalaryAdvanceDTO empdetails(HR_Emp_SalaryAdvanceDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "SalaryAdvanceApprovalFacade/empdetails");
        }

        public HR_Emp_SalaryAdvanceDTO approvedrecord(HR_Emp_SalaryAdvanceDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "SalaryAdvanceApprovalFacade/approvedrecord");
        }

        //------------------ Advance Approval Report------------------------------
        public HR_Emp_SalaryAdvanceDTO Onload(HR_Emp_SalaryAdvanceDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "SalaryAdvanceApprovalFacade/Onload");
        }
        public HR_Emp_SalaryAdvanceDTO getapprovalreport(HR_Emp_SalaryAdvanceDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "SalaryAdvanceApprovalFacade/getapprovalreport");
        }
    }
}
