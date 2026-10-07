using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface COETriggerInterface
    {
        COETriggerDTO getBasicData(COETriggerDTO dto);
        COETriggerDTO sendsmsemail(COETriggerDTO dto);
        COETriggerDTO editData(int id);

        COETriggerDTO getcoetemplate(COETriggerDTO dto);
        COETriggerDTO getrpt(COETriggerDTO dto);
        COETriggerDTO getrpt_lead(COETriggerDTO dto);
        COETriggerDTO loadtemplate(COETriggerDTO dto);
        COETriggerDTO viewtemplatedetails(COETriggerDTO dto);
    }
}
