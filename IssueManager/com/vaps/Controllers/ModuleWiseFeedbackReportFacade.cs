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
    public class ModuleWiseFeedbackReportFacade : Controller
    {
        public ModuleWiseFeedbackReportInterface _objinter;

        public ModuleWiseFeedbackReportFacade(ModuleWiseFeedbackReportInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("OnLoadClient")]
        public ModuleWiseFeedbackReport_DTO OnLoadClient([FromBody]ModuleWiseFeedbackReport_DTO data)
        {
            return _objinter.OnLoadClient(data);
        }

        [Route("getfeedbackreport")]
        public ModuleWiseFeedbackReport_DTO getfeedbackreport([FromBody]ModuleWiseFeedbackReport_DTO data)
        {
            return _objinter.getfeedbackreport(data);
        }

        [Route("saveModuleWisefeedback")]
        public ModuleWiseFeedbackReport_DTO saveModuleWisefeedback([FromBody]ModuleWiseFeedbackReport_DTO data)
        {
            return _objinter.saveModuleWisefeedback(data);
        }
        [Route("getvmsmodulefeedbackreport")]
        public ModuleWiseFeedbackReport_DTO getvmsmodulefeedbackreport([FromBody]ModuleWiseFeedbackReport_DTO data)
        {
            return _objinter.getvmsmodulefeedbackreport(data);
        }
        [Route("getcanteenandhousekeepingfeedback")]
        public ModuleWiseFeedbackReport_DTO getcanteenandhousekeepingfeedback([FromBody]ModuleWiseFeedbackReport_DTO data)
        {
            return _objinter.getcanteenandhousekeepingfeedback(data);
        }
    }
}
