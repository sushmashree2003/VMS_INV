using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.College.COE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.COE
{
    public class ClgCOEReportDelegate
    {
        CommonDelegate<ClgMasterCOEDTO, ClgMasterCOEDTO> COMMM = new CommonDelegate<ClgMasterCOEDTO, ClgMasterCOEDTO>();
     
        //public ClgMasterCOEDTO getdetails(ClgMasterCOEDTO categorypage)
        //{
        //    return COMMM.POSTClgCOE(categorypage, "ClgMasterCOEFacade/getdetails");
        //}

        public ClgMasterCOEDTO getdetails(int id)
        {
            return COMMM.GETClgCOE(id, "ClgCOEReportFacade/getdata/");
        }
        public ClgMasterCOEDTO getReport(ClgMasterCOEDTO dto)
        {
            return COMMM.POSTClgCOE(dto, "ClgCOEReportFacade/getReport");
        }

        public ClgMasterCOEDTO mothreport(ClgMasterCOEDTO dto)
        {
            return COMMM.POSTClgCOE(dto, "ClgCOEReportFacade/mothreport");
        }
    }
}
