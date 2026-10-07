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
    public class GPSLocationDelegate
    {
        CommonDelegate<VMS_Attendance_GPSLocationDTO, VMS_Attendance_GPSLocationDTO> COMFRNT = new CommonDelegate<VMS_Attendance_GPSLocationDTO, VMS_Attendance_GPSLocationDTO>();

        public VMS_Attendance_GPSLocationDTO getdata(VMS_Attendance_GPSLocationDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "GPSLocationFacade/getalldetails/");
        }

        public VMS_Attendance_GPSLocationDTO savedata(VMS_Attendance_GPSLocationDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "GPSLocationFacade/savedata/");
        }      
    }
}
