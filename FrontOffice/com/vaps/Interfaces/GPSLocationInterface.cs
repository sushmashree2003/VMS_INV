using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Interfaces
{
    public  interface GPSLocationInterface
    {
        VMS_Attendance_GPSLocationDTO getdata(VMS_Attendance_GPSLocationDTO data);
        VMS_Attendance_GPSLocationDTO savedata(VMS_Attendance_GPSLocationDTO data);
    }
}
