using IVRMUX.Delegates.com.vapstech.VMS.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.VMS.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class InternalJobOpeningPostController : Controller
    {
        InternalJobOpeningPostDelegate del = new InternalJobOpeningPostDelegate();

        [Route("getalldetails/{id:int}")]

        public InternalJobOpeningPostDTO getalldetails(int id)
        {
            InternalJobOpeningPostDTO data = new InternalJobOpeningPostDTO();
            {
                int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                data.MI_Id = mid;
                return del.getalldetails(data);
            }

        }
    }
}
