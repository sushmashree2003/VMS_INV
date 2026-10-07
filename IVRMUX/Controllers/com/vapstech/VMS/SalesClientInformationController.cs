using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.Sales;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VMS
{
    [Route("api/[controller]")]
    public class SalesClientInformationController : Controller
    {
        SalesClientInformationDelegate sales = new SalesClientInformationDelegate();

        [Route("OnLoadData/{id:int}")]
        public SalesClientInformationDto OnLoadData(int id)
        {
            SalesClientInformationDto dto = new SalesClientInformationDto();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return sales.OnLoadData(dto);
        }

       
    }
}
