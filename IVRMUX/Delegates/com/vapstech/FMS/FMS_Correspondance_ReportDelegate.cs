using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.FMS;

namespace IVRMUX.Delegates.com.vapstech.FMS
{
    public class FMS_Correspondance_ReportDelegate
    {
       CommonDelegate<FMS_Correspondance_ReportDTO, FMS_Correspondance_ReportDTO> _comm = new CommonDelegate<FMS_Correspondance_ReportDTO, FMS_Correspondance_ReportDTO>();

        public FMS_Correspondance_ReportDTO onload (FMS_Correspondance_ReportDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Correspondance_ReportFacade/onload");
        }
        public FMS_Correspondance_ReportDTO onchnageradiobtnsfile(FMS_Correspondance_ReportDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Correspondance_ReportFacade/onchnageradiobtnsfile");
        }
        public FMS_Correspondance_ReportDTO getprojectist(FMS_Correspondance_ReportDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Correspondance_ReportFacade/getprojectist");
        }
        public FMS_Correspondance_ReportDTO getreport(FMS_Correspondance_ReportDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Correspondance_ReportFacade/getreport");
        }
        public FMS_Correspondance_ReportDTO SearchReport(FMS_Correspondance_ReportDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Correspondance_ReportFacade/SearchReport");
        }
        public FMS_Correspondance_ReportDTO viewreferencedetais(FMS_Correspondance_ReportDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Correspondance_ReportFacade/viewreferencedetais");
        }
        public FMS_Correspondance_ReportDTO CountReport(FMS_Correspondance_ReportDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Correspondance_ReportFacade/CountReport");
        }
        public FMS_Correspondance_ReportDTO ViewRefernceNoDetails(FMS_Correspondance_ReportDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Correspondance_ReportFacade/ViewRefernceNoDetails");
        }

        // File Cabinet Report
        public FMS_Correspondance_ReportDTO OnloadFileCabinetReport(FMS_Correspondance_ReportDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Correspondance_ReportFacade/OnloadFileCabinetReport");
        }
        public FMS_Correspondance_ReportDTO FileCabinetReport(FMS_Correspondance_ReportDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Correspondance_ReportFacade/FileCabinetReport");
        }
    }

}
