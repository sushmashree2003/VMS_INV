using CommonLibrary;
using System;
using PreadmissionDTOs.com.vaps.VMS.TeleCaller;

namespace IVRMUX.Delegates.com.vapstech.VMS
{
    public class TeleCaller_DemoLlistDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<TeleCallerDTO, TeleCallerDTO> COMMM1 = new CommonDelegate<TeleCallerDTO, TeleCallerDTO>();
        public TeleCallerDTO TeleCaller_List(TeleCallerDTO tel)
        {
            return COMMM1.POSTIssueManager(tel, "Tele_Caller_DemoFacade/TeleCaller_List/");
        }
        public TeleCallerDTO UpdateTeleCaller_List(TeleCallerDTO tel)
        {
            return COMMM1.POSTIssueManager(tel, "Tele_Caller_DemoFacade/UpdateTeleCaller_List/");
        }
    }
}
