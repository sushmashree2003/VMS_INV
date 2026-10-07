using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Interfaces
{
  public  interface MasterHolidayInterface
    {
        MasterHolidayDTO getdata(int id);
        MasterHolidayDTO getdetails(int id);
        MasterHolidayDTO delete_data(int id);
        MasterHolidayDTO save_details(MasterHolidayDTO id);
        Task<MasterHolidayDTO> Change(MasterHolidayDTO id);

        //******************Advance********************///
        MasterHolidayDTO advloaddata(MasterHolidayDTO obj);
        MasterHolidayDTO saveadvmasterHolidaydata(MasterHolidayDTO obj);
        MasterHolidayDTO advdelete(int id);
        MasterHolidayDTO editadvmasterHoliday(MasterHolidayDTO obj);
        MasterHolidayDTO savedataWeekoff(MasterHolidayDTO obj);
    }
    
}
