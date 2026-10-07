using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class FF_SettlementAdminReportDelegate
    {

        
            private const String JsonContentType = "application/json; charset=utf-8";
            
            CommonDelegate<AdvanceReportDTO, AdvanceReportDTO> COMMM1 = new CommonDelegate<AdvanceReportDTO, AdvanceReportDTO>();

            public AdvanceReportDTO onloadgetdetails(AdvanceReportDTO dto)
            {
                return COMMM1.POSTDataaHRMS(dto, "FF_SettlementAdminReportFacade/onloadgetdetails");
            }

    

            public AdvanceReportDTO getEmployeedetailsBySelection(AdvanceReportDTO maspage)
            {
                return COMMM1.POSTDataHRMS(maspage, "FF_SettlementAdminReportFacade/getEmployeedetailsBySelection/");
            }

            public AdvanceReportDTO get_depts(AdvanceReportDTO maspage)
            {
                return COMMM1.POSTDataHRMS(maspage, "FF_SettlementAdminReportFacade/get_depts/");
            }
            public AdvanceReportDTO get_desig(AdvanceReportDTO maspage)
            {
                return COMMM1.POSTDataHRMS(maspage, "FF_SettlementAdminReportFacade/get_desig/");
            }
    }
}


