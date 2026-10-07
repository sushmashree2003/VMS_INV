using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.RTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.RTO
{
    public class VMS_Vehicle_ReportDelegate
    {
        CommonDelegate<VMS_Vehicle_ReportDTO, VMS_Vehicle_ReportDTO> comm = new CommonDelegate<VMS_Vehicle_ReportDTO, VMS_Vehicle_ReportDTO>();

        public VMS_Vehicle_ReportDTO loaddata(VMS_Vehicle_ReportDTO data)
        {
            return comm.POSTIssueManager(data, "VMS_Vehicle_ReportFacade/loaddata");
        }
        public VMS_Vehicle_ReportDTO savedata(VMS_Vehicle_ReportDTO data)
        {
            return comm.POSTIssueManager(data, "VMS_Vehicle_ReportFacade/savedata");
        }
        //onchange
        public VMS_Vehicle_ReportDTO onchange(VMS_Vehicle_ReportDTO data)
        {
            return comm.POSTIssueManager(data, "VMS_Vehicle_ReportFacade/onchange");
        }
        //savedataSource
        public VMS_Vehicle_ReportDTO savedataSource(VMS_Vehicle_ReportDTO data)
        {
            return comm.POSTIssueManager(data, "VMS_Vehicle_ReportFacade/savedataSource");
        }
        //clientDecative
        public VMS_Vehicle_ReportDTO clientDecative(VMS_Vehicle_ReportDTO data)
        {
            return comm.POSTIssueManager(data, "VMS_Vehicle_ReportFacade/clientDecative");
        }

        //
        public VMS_Vehicle_ReportDTO RecieptReport(VMS_Vehicle_ReportDTO data)
        {
            return comm.POSTIssueManager(data, "VMS_Vehicle_ReportFacade/RecieptReport");
        }

        //for refence of multibox
        public VMS_Vehicle_ReportDTO getdetails(VMS_Vehicle_ReportDTO data)
        {
            return comm.POSTIssueManager(data, "VMS_Vehicle_ReportFacade/RecieptReport");
        }
        public VMS_Vehicle_ReportDTO get_report(VMS_Vehicle_ReportDTO data)
        {
            return comm.POSTIssueManager(data, "VMS_Vehicle_ReportFacade/RecieptReport");
        }
        public VMS_Vehicle_ReportDTO buildingchange(VMS_Vehicle_ReportDTO data)
        {
            return comm.POSTIssueManager(data, "VMS_Vehicle_ReportFacade/RecieptReport");
        }
        public VMS_Vehicle_ReportDTO Floorchange(VMS_Vehicle_ReportDTO data)
        {
            return comm.POSTIssueManager(data, "VMS_Vehicle_ReportFacade/RecieptReport");
        }


    }
}
