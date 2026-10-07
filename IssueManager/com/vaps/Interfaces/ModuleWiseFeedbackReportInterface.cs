using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ModuleWiseFeedbackReportInterface
    {
        ModuleWiseFeedbackReport_DTO OnLoadClient(ModuleWiseFeedbackReport_DTO data);
        ModuleWiseFeedbackReport_DTO getfeedbackreport(ModuleWiseFeedbackReport_DTO data);
        ModuleWiseFeedbackReport_DTO saveModuleWisefeedback(ModuleWiseFeedbackReport_DTO data);
        ModuleWiseFeedbackReport_DTO getvmsmodulefeedbackreport(ModuleWiseFeedbackReport_DTO data);
        ModuleWiseFeedbackReport_DTO getcanteenandhousekeepingfeedback(ModuleWiseFeedbackReport_DTO data);

    }
}
