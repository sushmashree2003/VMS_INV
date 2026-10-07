using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class FF_SettlemetAdminApplyDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<FF_Settlement_Process_DTO, FF_Settlement_Process_DTO> COMMM = new CommonDelegate<FF_Settlement_Process_DTO, FF_Settlement_Process_DTO>();

        public FF_Settlement_Process_DTO onloadgetdetails(FF_Settlement_Process_DTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "FF_SettlemetAdminApplyFacade/onloadgetdetails");
        }

        public FF_Settlement_Process_DTO savedetails(FF_Settlement_Process_DTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "FF_SettlemetAdminApplyFacade/");
        }
        public FF_Settlement_Process_DTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "FF_SettlemetAdminApplyFacade/getRecordById/");
        }
        public FF_Settlement_Process_DTO deleterec(FF_Settlement_Process_DTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "FF_SettlemetAdminApplyFacade/deactivateRecordById/");
        }

        public FF_Settlement_Process_DTO getDetailsByEmployee(FF_Settlement_Process_DTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "FF_SettlemetAdminApplyFacade/getDetailsByEmployee/");
        }
        public FF_Settlement_Process_DTO empdetails(FF_Settlement_Process_DTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "FF_SettlemetAdminApplyFacade/empdetails/");
        }


        public FF_Settlement_Process_DTO searchfilter(FF_Settlement_Process_DTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "FF_SettlemetAdminApplyFacade/searchfilter/");
        }
    }
}
