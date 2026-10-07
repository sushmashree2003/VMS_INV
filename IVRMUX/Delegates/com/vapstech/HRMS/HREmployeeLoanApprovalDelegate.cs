using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class HREmployeeLoanApprovalDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HREmployeeLoanApprovalDTO, HREmployeeLoanApprovalDTO> COMMM = new CommonDelegate<HREmployeeLoanApprovalDTO, HREmployeeLoanApprovalDTO>();

        public HREmployeeLoanApprovalDTO onloadgetdetails(HREmployeeLoanApprovalDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "HREmployeeLoanApprovalFacade/onloadgetdetails");
        }


        //getEmployeedetailsBySelection  

        public HREmployeeLoanApprovalDTO getEmployeedetailsBySelection(HREmployeeLoanApprovalDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HREmployeeLoanApprovalFacade/getEmployeedetailsBySelection/");
        }

        public HREmployeeLoanApprovalDTO get_depts(HREmployeeLoanApprovalDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HREmployeeLoanApprovalFacade/get_depts/");
        }
        public HREmployeeLoanApprovalDTO get_desig(HREmployeeLoanApprovalDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HREmployeeLoanApprovalFacade/get_desig/");
        }
        public HREmployeeLoanApprovalDTO getapprvaldata(HREmployeeLoanApprovalDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HREmployeeLoanApprovalFacade/getapprvaldata/");
        }
        public HREmployeeLoanApprovalDTO SalaryApproval(HREmployeeLoanApprovalDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HREmployeeLoanApprovalFacade/SalaryApproval/");
        }
        public HREmployeeLoanApprovalDTO SalaryApprovalgriddata(HREmployeeLoanApprovalDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HREmployeeLoanApprovalFacade/SalaryApprovalgriddata/");
        }

        //F&F Report
        public HREmployeeLoanApprovalDTO getOnloaddata(HREmployeeLoanApprovalDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "HREmployeeLoanApprovalFacade/getOnloaddata");
        }
        public HREmployeeLoanApprovalDTO getemployeedetails(HREmployeeLoanApprovalDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HREmployeeLoanApprovalFacade/getemployeedetails/");
        }
        public HREmployeeLoanApprovalDTO ViewFiles(HREmployeeLoanApprovalDTO data)
        {
            return COMMM.POSTDataHRMS(data, "HREmployeeLoanApprovalFacade/ViewFiles");
        }

    }
}
