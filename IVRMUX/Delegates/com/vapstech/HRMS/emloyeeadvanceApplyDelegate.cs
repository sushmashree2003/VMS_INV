using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;


namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class emloyeeadvanceApplyDelegate
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Emp_SalaryAdvanceDTO, HR_Emp_SalaryAdvanceDTO> COMMM = new CommonDelegate<HR_Emp_SalaryAdvanceDTO, HR_Emp_SalaryAdvanceDTO>();

        public HR_Emp_SalaryAdvanceDTO onloadgetdetails(HR_Emp_SalaryAdvanceDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "employeeadvanceapplyFacade/onloadgetdetails");
        }

        public HR_Emp_SalaryAdvanceDTO savedetails(HR_Emp_SalaryAdvanceDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "employeeadvanceapplyFacade/");
        }
        public HR_Emp_SalaryAdvanceDTO Edit(HR_Emp_SalaryAdvanceDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "employeeadvanceapplyFacade/Edit/");
        }
        public HR_Emp_SalaryAdvanceDTO deleterec(HR_Emp_SalaryAdvanceDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "employeeadvanceapplyFacade/deactivateRecordById/");
        }
        public HR_Emp_SalaryAdvanceDTO getDetailsByEmployee(HR_Emp_SalaryAdvanceDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "employeeadvanceapplyFacade/getDetailsByEmployee/");
        }
    }
}