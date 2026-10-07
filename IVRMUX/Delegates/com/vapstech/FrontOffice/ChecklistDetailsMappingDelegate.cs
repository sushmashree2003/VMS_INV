using CommonLibrary;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.FrontOffice
{
    public class ChecklistDetailsMappingDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_Checklist_DetailsDTO, ISM_Checklist_DetailsDTO> COMFRNT = new CommonDelegate<ISM_Checklist_DetailsDTO, ISM_Checklist_DetailsDTO>();
        //public ISM_Checklist_DetailsDTO getalldetails(int id)
        //{
        //    return COMFRNT.GetDataByIdFROFF(id, "ChecklistDetailsMappingFacade/getalldetails/");
        //}
        public ISM_Checklist_DetailsDTO getalldetails(ISM_Checklist_DetailsDTO report)
        {
            return COMFRNT.POSTDataHolidayReport(report, "ChecklistDetailsMappingFacade/getalldetails/");
        }
        public ISM_Checklist_DetailsDTO editRecord(ISM_Checklist_DetailsDTO report)
        {
            return COMFRNT.POSTDataHolidayReport(report, "ChecklistDetailsMappingFacade/editRecord/");
        }
        public ISM_Checklist_DetailsDTO DeleteYss(ISM_Checklist_DetailsDTO report)
        {
            return COMFRNT.POSTDataHolidayReport(report, "ChecklistDetailsMappingFacade/DeleteYss/");
        }
        public ISM_Checklist_DetailsDTO savedata(ISM_Checklist_DetailsDTO report)
        {
            return COMFRNT.POSTDataHolidayReport(report, "ChecklistDetailsMappingFacade/savedata/");
        }

        public ISM_Checklist_DetailsDTO approvalloaddata(ISM_Checklist_DetailsDTO report)
        {
            return COMFRNT.POSTDataHolidayReport(report, "ChecklistDetailsMappingFacade/approvalloaddata/");
        }

        public ISM_Checklist_DetailsDTO findchecklist(ISM_Checklist_DetailsDTO report)
        {
            return COMFRNT.POSTDataHolidayReport(report, "ChecklistDetailsMappingFacade/findchecklist/");
        }

        public ISM_Checklist_DetailsDTO saveapproval(ISM_Checklist_DetailsDTO report)
        {
            return COMFRNT.POSTDataHolidayReport(report, "ChecklistDetailsMappingFacade/saveapproval/");
        }

        public ISM_Checklist_DetailsDTO checklistreport(ISM_Checklist_DetailsDTO report)
        {
            return COMFRNT.POSTDataHolidayReport(report, "ChecklistDetailsMappingFacade/checklistreport/");
        }
    }
}
