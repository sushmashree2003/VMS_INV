using IssueManager.com.DriversChart.Interface;
using IssueManager.com.QRcode_Activity.Interfaces;
using IssueManager.com.TeleCaller.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.TeleCaller;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.TeleCaller.Facade
{
    [Route("api/[controller]")]
    public class Tele_Caller_DemoFacade : Controller
    {
        public Tele_caller_demoInterface inter;
        public Tele_Caller_DemoFacade(Tele_caller_demoInterface s)
        {
            inter = s;
        }

        [Route("TeleCaller_List")]
        public TeleCallerDTO TeleCaller_List([FromBody] TeleCallerDTO data)
        {
            return inter.TeleCaller_List(data);
        }
        [Route("UpdateTeleCaller_List")]
        public TeleCallerDTO UpdateTeleCaller_List([FromBody] TeleCallerDTO data)
        {
            return inter.UpdateTeleCaller_List(data);
        }


        
    }
}
