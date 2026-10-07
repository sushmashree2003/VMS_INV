using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.TADA
{
    public class TADA_ReportsDelegetes
    {
        CommonDelegate<TADA_ApplyReportsDTO, TADA_ApplyReportsDTO> COMMI = new CommonDelegate<TADA_ApplyReportsDTO, TADA_ApplyReportsDTO>();

        public TADA_ApplyReportsDTO getalldata(TADA_ApplyReportsDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADA_ReportsFacade/getalldata/");
        }
        //Deptselectiondetails
        public TADA_ApplyReportsDTO Deptselectiondetails(TADA_ApplyReportsDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADA_ReportsFacade/Deptselectiondetails/");
        }
        //Desgselectiondetails
        public TADA_ApplyReportsDTO Desgselectiondetails(TADA_ApplyReportsDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADA_ReportsFacade/Desgselectiondetails/");
        }
        //
        public TADA_ApplyReportsDTO GetReport(TADA_ApplyReportsDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADA_ReportsFacade/GetReport/");
        }
        //
        public TADA_ApplyReportsDTO Viewdetails(TADA_ApplyReportsDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADA_ReportsFacade/Viewdetails/");
        }
    }
}
