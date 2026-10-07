using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace corewebapi18072016.Delegates.com.vapstech.FrontOffice
{
    public class DriverOTDelegate
    {

        CommonDelegate<DriverOTDTO, DriverOTDTO> COMFRNT = new CommonDelegate<DriverOTDTO, DriverOTDTO>();

        public DriverOTDTO getdata(DriverOTDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "DriverOTFacade/getalldetails/");
        }
        public DriverOTDTO employeereport(DriverOTDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "DriverOTFacade/employeereport/");
        }
        public DriverOTDTO savedata(DriverOTDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "DriverOTFacade/savedata/");
        }
        public DriverOTDTO getdetails(int id)
        {
            return COMFRNT.DeleteDataByIdFROFF(id, "DriverOTFacade/getdetails/");
        }
        public DriverOTDTO deletedata(int id)
        {
            return COMFRNT.DeleteDataByIdFROFF(id, "DriverOTFacade/deletedata/");
        }

        public DriverOTDTO getApprovalStatus(DriverOTDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "DriverOTFacade/getApprovalStatus/");
        }

        public DriverOTDTO get_status(DriverOTDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "DriverOTFacade/get_status/");
        }

        public DriverOTDTO reject_status(DriverOTDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "DriverOTFacade/reject_status/");
        }

        public DriverOTDTO finalOTreport(DriverOTDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "DriverOTFacade/OTFinalReportAsync/");
        }        
    }
}
