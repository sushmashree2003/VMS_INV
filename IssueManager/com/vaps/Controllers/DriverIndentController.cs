using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class DriverIndentController : Controller
    {
        public DriverIndentInterface _objinter;

        public DriverIndentController(DriverIndentInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public  ISM_Driver_IndentDTO getdetails([FromBody] ISM_Driver_IndentDTO data)
        {
            return _objinter.getdetails(data);
        }

        [Route("loaddata")]
        public ISM_Driver_IndentDTO loaddata([FromBody] ISM_Driver_IndentDTO data)
        {
            return _objinter.loaddata(data);
        }

        [Route("save")]
        public ISM_Driver_IndentDTO save([FromBody] ISM_Driver_IndentDTO data)
        {
            return _objinter.save(data);
        }

        [Route("edittab1")]
        public ISM_Driver_IndentDTO edittab1([FromBody] ISM_Driver_IndentDTO data)
        {
            return _objinter.edittab1(data);
        }

        [Route("getApprovalStatus")]
        public Task<ISM_Driver_IndentDTO> getApprovalStatus([FromBody] ISM_Driver_IndentDTO data)
        {
            return _objinter.getApprovalStatusAsync(data);
        }

        [Route("get_status")]
        public ISM_Driver_IndentDTO get_status([FromBody] ISM_Driver_IndentDTO data)
        {
            return _objinter.get_status(data);
        }

        [Route("reject_status")]
        public ISM_Driver_IndentDTO reject_status([FromBody] ISM_Driver_IndentDTO data)
        {
            return _objinter.reject_status(data);
        }

        [Route("getrpt")]
        public ISM_Driver_IndentDTO getrpt([FromBody] ISM_Driver_IndentDTO data)
        {
            return _objinter.getrpt(data);
        }

        [Route("checkedVehicleno")]
        public ISM_Driver_IndentDTO checkedVehicleno([FromBody] ISM_Driver_IndentDTO data)
        {
            return _objinter.checkedVehicleno(data);
        }
    }
}
