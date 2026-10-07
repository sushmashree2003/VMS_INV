using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS
{
    public class GeneralSendEmailDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<SalesSMSEMAILDTO, SalesSMSEMAILDTO> COMMM = new CommonDelegate<SalesSMSEMAILDTO, SalesSMSEMAILDTO>();
        public SalesSMSEMAILDTO sendsmsemail(SalesSMSEMAILDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "GeneralSendEmailFacade/sendsmsemail/");
        }
        //sendsmsemailsales
        public SalesSMSEMAILDTO sendsmsemailsales(SalesSMSEMAILDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "GeneralSendEmailFacade/sendsmsemailsales/");
        }
        //onload
        public SalesSMSEMAILDTO onload(SalesSMSEMAILDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "GeneralSendEmailFacade/onload/");
        }
    }
}
