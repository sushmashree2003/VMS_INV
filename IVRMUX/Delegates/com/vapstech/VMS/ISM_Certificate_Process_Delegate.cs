using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS;
using PreadmissionDTOs.com.vaps.VMS.Exit;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS
{
    public class ISM_Certificate_Process_Delegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonVMSDelegate<ISM_Resignation_Master_Reasons_DTO, ISM_Resignation_Master_Reasons_DTO> COMMI = new CommonVMSDelegate<ISM_Resignation_Master_Reasons_DTO, ISM_Resignation_Master_Reasons_DTO>();
        CommonVMSDelegate<ISM_Resignation_Master_CheckLists_DTO, ISM_Resignation_Master_CheckLists_DTO> COMMRC = new CommonVMSDelegate<ISM_Resignation_Master_CheckLists_DTO, ISM_Resignation_Master_CheckLists_DTO>();
        CommonVMSDelegate<ISM_Resignation_DTO, ISM_Resignation_DTO> COMMR = new CommonVMSDelegate<ISM_Resignation_DTO, ISM_Resignation_DTO>();
        CommonVMSDelegate<ISM_Certificate_RequestDTO, ISM_Certificate_RequestDTO> COMM = new CommonVMSDelegate<ISM_Certificate_RequestDTO, ISM_Certificate_RequestDTO>();
        CommonVMSDelegate<ISM_Resignation_ChecKLists_DTO, ISM_Resignation_ChecKLists_DTO> COMRC = new CommonVMSDelegate<ISM_Resignation_ChecKLists_DTO, ISM_Resignation_ChecKLists_DTO>();


     

        //---------------------------ISM_Certificate_Request---------------------
        public ISM_Certificate_RequestDTO getloaddata(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "ISM_Certificate_Process_Facade/getloaddata/");
        }

        public ISM_Certificate_RequestDTO savedetails(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "ISM_Certificate_Process_Facade/savedata/");
        }
        public ISM_Certificate_RequestDTO deactive(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "ISM_Certificate_Process_Facade/deactive/");
        }
        public ISM_Certificate_RequestDTO Edit(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "ISM_Certificate_Process_Facade/Edit/");
        }
        public ISM_Certificate_RequestDTO Viewdata(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "ISM_Certificate_Process_Facade/Viewdata");
        }

        //--------- ISM ADMIN Certificate Apply-------------------
        public ISM_Certificate_RequestDTO getadminloaddata(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "ISM_Certificate_Process_Facade/getadminloaddata");
        }
        public ISM_Certificate_RequestDTO getcertificate(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "ISM_Certificate_Process_Facade/getcertificate");
        }

        public ISM_Certificate_RequestDTO saveadmindetails(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "ISM_Certificate_Process_Facade/saveadmindetails/");
        }


        //--------certificate approval----------------------


        public ISM_Certificate_RequestDTO loaddata(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "ISM_Certificate_Process_Facade/loaddata");
        }

        public ISM_Certificate_RequestDTO viewuploadflies(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "ISM_Certificate_Process_Facade/viewuploadflies");
        }

        public ISM_Certificate_RequestDTO approvedrecord(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "ISM_Certificate_Process_Facade/approvedrecord");
        }

        //------------------ ISM Certificate Approval Report------------------------------
        public ISM_Certificate_RequestDTO Onload(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "ISM_Certificate_Process_Facade/Onload");
        }

        public ISM_Certificate_RequestDTO getapprovalreport(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "ISM_Certificate_Process_Facade/getapprovalreport");
        }

    }
}
