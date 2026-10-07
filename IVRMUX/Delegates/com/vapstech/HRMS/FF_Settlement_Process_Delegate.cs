using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class FF_Settlement_Process_Delegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<FF_Settlement_Process_DTO, FF_Settlement_Process_DTO> COMMM = new CommonDelegate<FF_Settlement_Process_DTO, FF_Settlement_Process_DTO>();

        public FF_Settlement_Process_DTO getalldetails(FF_Settlement_Process_DTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "FF_Settlement_Process_Facade/getalldetails");
        }
        public FF_Settlement_Process_DTO SaveUpdate(FF_Settlement_Process_DTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "FF_Settlement_Process_Facade/SaveUpdate");
        }
        public FF_Settlement_Process_DTO Edit(FF_Settlement_Process_DTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "FF_Settlement_Process_Facade/Edit");
        }
        public FF_Settlement_Process_DTO deactivate(FF_Settlement_Process_DTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "FF_Settlement_Process_Facade/deactivate");
        }
     
       ///////////////// //Approval//////////////////
        public FF_Settlement_Process_DTO getData(FF_Settlement_Process_DTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "FF_Settlement_Process_Facade/getData");
        }
       
        public FF_Settlement_Process_DTO empdetail(FF_Settlement_Process_DTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "FF_Settlement_Process_Facade/empdetail");
        }
        public FF_Settlement_Process_DTO approvedrecord(FF_Settlement_Process_DTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "FF_Settlement_Process_Facade/approvedrecord");
        }
        
        /////////////Report////////////////////////
        public FF_Settlement_Process_DTO Onload(FF_Settlement_Process_DTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "FF_Settlement_Process_Facade/Onload");
        }
        public FF_Settlement_Process_DTO getapprovalreport(FF_Settlement_Process_DTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "FF_Settlement_Process_Facade/getapprovalreport");
        }
    }
}
