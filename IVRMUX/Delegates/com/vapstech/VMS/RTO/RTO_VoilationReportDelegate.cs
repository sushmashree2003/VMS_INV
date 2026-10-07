using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.RTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.RTO
{
    public class RTO_VoilationReportDelegate

    {
        CommonDelegate<RTO_VoilationReportDTO, RTO_VoilationReportDTO> comm = new CommonDelegate<RTO_VoilationReportDTO, RTO_VoilationReportDTO>();

        public RTO_VoilationReportDTO loaddata1(RTO_VoilationReportDTO data)
        {
            return comm.POSTIssueManager(data, "RTO_VoilationReportFacade/loaddata");
        }
        public RTO_VoilationReportDTO savedata(RTO_VoilationReportDTO data)
        {
            return comm.POSTIssueManager(data, "RTO_VoilationReportFacade/savedata");
        }
        //onchange
        public RTO_VoilationReportDTO onchange(RTO_VoilationReportDTO data)
        {
            return comm.POSTIssueManager(data, "RTO_VoilationReportFacade/onchange");
        }
        //savedataSource
        public RTO_VoilationReportDTO savedataSource(RTO_VoilationReportDTO data)
        {
            return comm.POSTIssueManager(data, "RTO_VoilationReportFacade/savedataSource");
        }
        //clientDecative
        public RTO_VoilationReportDTO clientDecative(RTO_VoilationReportDTO data)
        {
            return comm.POSTIssueManager(data, "RTO_VoilationReportFacade/clientDecative");
        }

        //
        public RTO_VoilationReportDTO RecieptReport1(RTO_VoilationReportDTO data)
        {
            return comm.POSTIssueManager(data, "RTO_VoilationReportFacade/RecieptReport1");
        }
    }
}