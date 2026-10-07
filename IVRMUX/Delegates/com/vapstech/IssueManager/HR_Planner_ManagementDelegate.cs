using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class HR_Planner_ManagementDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Planner_ManagementDTO, HR_Planner_ManagementDTO> COMMM = new CommonDelegate<HR_Planner_ManagementDTO, HR_Planner_ManagementDTO>();

        public HR_Planner_ManagementDTO getdetails(HR_Planner_ManagementDTO data)
        {
            return COMMM.POSTIssueManager(data, "HR_Planner_ManagementFacade/getdetails/");
        }
          public HR_Planner_ManagementDTO getdetailsreminder(HR_Planner_ManagementDTO data)
        {
            return COMMM.POSTIssueManager(data, "HR_Planner_ManagementFacade/getdetailsreminder/");
        }

        public HR_Planner_ManagementDTO savedata(HR_Planner_ManagementDTO data)
        {
            return COMMM.POSTIssueManager(data, "HR_Planner_ManagementFacade/savePlanner");
        }

        public HR_Planner_ManagementDTO edit(HR_Planner_ManagementDTO data)
        {
            return COMMM.POSTIssueManager(data, "HR_Planner_ManagementFacade/edit");
        }
        public HR_Planner_ManagementDTO activedeactive(HR_Planner_ManagementDTO data)
        {
            return COMMM.POSTIssueManager(data, "HR_Planner_ManagementFacade/activedeactive");
        }
        public HR_Planner_ManagementDTO DeleteYY(HR_Planner_ManagementDTO data)
        {
            return COMMM.POSTIssueManager(data, "HR_Planner_ManagementFacade/DeleteYY");
        }
        public HR_Planner_ManagementDTO viewuploadflies(HR_Planner_ManagementDTO data)
        {
            return COMMM.POSTIssueManager(data, "HR_Planner_ManagementFacade/viewuploadflies");
        }
        public HR_Planner_ManagementDTO getempmailid(HR_Planner_ManagementDTO data)
        {
            return COMMM.POSTIssueManager(data, "HR_Planner_ManagementFacade/getempmailid");
        }
        public HR_Planner_ManagementDTO deleteuploadfile(HR_Planner_ManagementDTO data)
        {
            return COMMM.POSTIssueManager(data, "HR_Planner_ManagementFacade/deleteuploadfile");
        }
        public HR_Planner_ManagementDTO clientDetails(HR_Planner_ManagementDTO data)
        {
            return COMMM.POSTIssueManager(data, "HR_Planner_ManagementFacade/clientDetails");
        }
    }
}
