using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ModuleWiseFeedbackReportDelegate
    {
        CommonDelegate<ModuleWiseFeedbackReport_DTO, ModuleWiseFeedbackReport_DTO> _comm = new CommonDelegate<ModuleWiseFeedbackReport_DTO, ModuleWiseFeedbackReport_DTO>();

        public ModuleWiseFeedbackReport_DTO OnLoadClient(ModuleWiseFeedbackReport_DTO data)
        {
            return _comm.POSTIssueManager(data, "ModuleWiseFeedbackReportFacade/OnLoadClient");
        }
        public ModuleWiseFeedbackReport_DTO getfeedbackreport(ModuleWiseFeedbackReport_DTO data)
        {
            return _comm.POSTIssueManager(data, "ModuleWiseFeedbackReportFacade/getfeedbackreport");
        }
        public ModuleWiseFeedbackReport_DTO getvmsmodulefeedbackreport(ModuleWiseFeedbackReport_DTO data)
        {
            return _comm.POSTIssueManager(data, "ModuleWiseFeedbackReportFacade/getvmsmodulefeedbackreport");
        }
        public ModuleWiseFeedbackReport_DTO getcanteenandhousekeepingfeedback(ModuleWiseFeedbackReport_DTO data)
        {
            return _comm.POSTIssueManager(data, "ModuleWiseFeedbackReportFacade/getcanteenandhousekeepingfeedback");
        }
    }
}
