using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.HRMS
{
    public class COETriggerDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<COETriggerDTO, COETriggerDTO> COMMM = new CommonDelegate<COETriggerDTO, COETriggerDTO>();

        public COETriggerDTO onloadgetdetails(COETriggerDTO dto)
        {
            return COMMM.POSTVMS(dto, "COETriggerFacade/onloadgetdetails");
        }

        public COETriggerDTO sendsmsemail(COETriggerDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "COETriggerFacade/sendsmsemail/");
        }
        public COETriggerDTO getcoetemplate(COETriggerDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "COETriggerFacade/getcoetemplate/");
        }
        public COETriggerDTO getRecorddetailsById(int id)
        {
            return COMMM.GetVMS(id, "COETriggerFacade/getRecordById/");
        }
        
        public COETriggerDTO getrpt(COETriggerDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "COETriggerFacade/getrpt/");
        }
        public COETriggerDTO getrpt_lead(COETriggerDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "COETriggerFacade/getrpt_lead/");
        }
        public COETriggerDTO loadtemplate(COETriggerDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "COETriggerFacade/loadtemplate/");
        }
        public COETriggerDTO viewtemplatedetails(COETriggerDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "COETriggerFacade/viewtemplatedetails/");
        }

    }
}
