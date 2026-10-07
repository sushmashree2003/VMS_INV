using FMS.Com.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FMS;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMS.Com.Facade
{
    [Route("api/[controller]")]
    public class Employee_Wise_VMS_FeedBackFacade : Controller
    {
        public Employee_Wise_VMS_FeedBackInterface _interface;
        public Employee_Wise_VMS_FeedBackFacade(Employee_Wise_VMS_FeedBackInterface _inter)
        {
            _interface = _inter;
        }
        [Route("getfeedback")]
        public Employee_Wise_VMS_FeedBackDTO getfeedback([FromBody] Employee_Wise_VMS_FeedBackDTO data)
        {
            return _interface.getfeedback(data);
        }
        [Route("Savedata")]
        public Employee_Wise_VMS_FeedBackDTO Savedata([FromBody] Employee_Wise_VMS_FeedBackDTO data)
        {
            return _interface.Savedata(data);
        }

        //======================================================
        [Route("GetMonthYear")]
        public HW_HeathCheckUpDTO GetMonthYear([FromBody]HW_HeathCheckUpDTO data)
        {
            return _interface.GetMonthYear(data);
        }


        [Route("employeefeedback")]
        public HW_HeathCheckUpDTO getfeedback([FromBody] HW_HeathCheckUpDTO data)
        {
            return _interface.employeefeedback(data);
        }

        [Route("savecallcenter")]
        public HW_HeathCheckUpDTO savecallcenter([FromBody] HW_HeathCheckUpDTO data)
        {
            return _interface.savecallcenter(data);
        }

        [Route("ViewHealthCheckUpFiles")]
        public HW_HeathCheckUpDTO ViewHealthCheckUpFiles([FromBody] HW_HeathCheckUpDTO data)
        {
            return _interface.ViewHealthCheckUpFiles(data);
        }
    }
}
