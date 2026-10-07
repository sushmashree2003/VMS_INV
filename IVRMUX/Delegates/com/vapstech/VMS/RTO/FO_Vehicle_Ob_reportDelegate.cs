using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.RTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.RTO
{
    public class FO_Vehicle_Ob_reportDelegate
    {
        CommonDelegate<FO_Vehicle_Ob_reportDTO, FO_Vehicle_Ob_reportDTO> comm = new CommonDelegate<FO_Vehicle_Ob_reportDTO, FO_Vehicle_Ob_reportDTO>();
      
        public FO_Vehicle_Ob_reportDTO loaddata1(FO_Vehicle_Ob_reportDTO data)
        {
            return comm.POSTIssueManager(data, "FO_Vehicle_Ob_reportFacade/loaddata");
        }
        public FO_Vehicle_Ob_reportDTO savedata(FO_Vehicle_Ob_reportDTO data)
        {
            return comm.POSTIssueManager(data, "FO_Vehicle_Ob_reportFacade/savedata");
        }
        //onchange
        public FO_Vehicle_Ob_reportDTO onchange(FO_Vehicle_Ob_reportDTO data)
        {
            return comm.POSTIssueManager(data, "FO_Vehicle_Ob_reportFacade/onchange");
        }
        //savedataSource
        public FO_Vehicle_Ob_reportDTO savedataSource(FO_Vehicle_Ob_reportDTO data)
        {
            return comm.POSTIssueManager(data, "FO_Vehicle_Ob_reportFacade/savedataSource");
        }
        //clientDecative
        public FO_Vehicle_Ob_reportDTO clientDecative(FO_Vehicle_Ob_reportDTO data)
        {
            return comm.POSTIssueManager(data, "FO_Vehicle_Ob_reportFacade/clientDecative");
        }

        //
        public FO_Vehicle_Ob_reportDTO RecieptReport1(FO_Vehicle_Ob_reportDTO data)
        {
            return comm.POSTIssueManager(data, "FO_Vehicle_Ob_reportFacade/RecieptReport1");
        }
    }
}
