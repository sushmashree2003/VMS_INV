using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface HR_Planner_ManagementInterface
    {
        HR_Planner_ManagementDTO getdetails(HR_Planner_ManagementDTO data);
        HR_Planner_ManagementDTO getdetailsreminder(HR_Planner_ManagementDTO data);
        HR_Planner_ManagementDTO savePlanner(HR_Planner_ManagementDTO data);  
        HR_Planner_ManagementDTO edit(HR_Planner_ManagementDTO data);  
        HR_Planner_ManagementDTO activedeactive(HR_Planner_ManagementDTO data);  
        HR_Planner_ManagementDTO viewuploadflies(HR_Planner_ManagementDTO data);  
        HR_Planner_ManagementDTO getempmailid(HR_Planner_ManagementDTO data);  
        HR_Planner_ManagementDTO deleteuploadfile(HR_Planner_ManagementDTO data);  
        HR_Planner_ManagementDTO generalreminderemailtemp(HR_Planner_ManagementDTO data);  
        HR_Planner_ManagementDTO generalreminderemail(HR_Planner_ManagementDTO data);  
        HR_Planner_ManagementDTO generalreminderemail_temp(HR_Planner_ManagementDTO data);  
        HR_Planner_ManagementDTO generalremindersms(HR_Planner_ManagementDTO data);  
        HR_Planner_ManagementDTO generalreminderrepeatEmail(HR_Planner_ManagementDTO data);  
        HR_Planner_ManagementDTO rptmail(HR_Planner_ManagementDTO data);  
        HR_Planner_ManagementDTO generalreminderrepeatsms(HR_Planner_ManagementDTO data);

        HR_Planner_ManagementDTO DeleteYY(HR_Planner_ManagementDTO data);
        HR_Planner_ManagementDTO clientDetails(HR_Planner_ManagementDTO data);
    }
}
