using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class MOMUpdateDelegate
    {
        CommonDelegate<MOMUpdateDTO, MOMUpdateDTO> COMMNFUN = new CommonDelegate<MOMUpdateDTO, MOMUpdateDTO>();

        // ////////////////////////////////trans//////////////////////////////////////////////


        public MOMUpdateDTO getonloaddata(MOMUpdateDTO obj)
        {
            return COMMNFUN.POSTDataaHRMS(obj, "MOMUpdateFacade/getonloaddata/");
        }
        public MOMUpdateDTO savetransaction(MOMUpdateDTO obj)
        {
            return COMMNFUN.POSTDataaHRMS(obj, "MOMUpdateFacade/savetransaction/");
        }
        public MOMUpdateDTO viewflies(MOMUpdateDTO obj)
        {
            return COMMNFUN.POSTDataaHRMS(obj, "MOMUpdateFacade/viewflies/");
        }
        public MOMUpdateDTO deactivate(MOMUpdateDTO obj)
        {
            return COMMNFUN.POSTDataaHRMS(obj, "MOMUpdateFacade/deactivate/");
        }
        public MOMUpdateDTO Edittrans(MOMUpdateDTO obj)
        {
            return COMMNFUN.POSTDataaHRMS(obj, "MOMUpdateFacade/Edittrans/");
        }
        public MOMUpdateDTO oldmeetingdetail(MOMUpdateDTO obj)
        {
            return COMMNFUN.POSTDataaHRMS(obj, "MOMUpdateFacade/oldmeetingdetail/");
        }
        //======update=====/////////////////////////////////////////////
        public MOMUpdateDTO getDetails(MOMUpdateDTO obj)
        {
            return COMMNFUN.POSTDataaHRMS(obj, "MOMUpdateFacade/getDetails/");
        }
        public MOMUpdateDTO UpdateDetails(MOMUpdateDTO obj)
        {
            return COMMNFUN.POSTDataaHRMS(obj, "MOMUpdateFacade/UpdateDetails/");
        }
        public MOMUpdateDTO Edit(MOMUpdateDTO obj)
        {
            return COMMNFUN.POSTDataaHRMS(obj, "MOMUpdateFacade/Edit/");
        }
        public MOMUpdateDTO viewuploadflies(MOMUpdateDTO obj)
        {
            return COMMNFUN.POSTDataaHRMS(obj, "MOMUpdateFacade/viewuploadflies/");
        }
        public MOMUpdateDTO saveData(MOMUpdateDTO obj)
        {
            return COMMNFUN.POSTDataaHRMS(obj, "MOMUpdateFacade/saveData/");
        }
        public MOMUpdateDTO checkvalidation(MOMUpdateDTO obj)
        {
            return COMMNFUN.POSTDataaHRMS(obj, "MOMUpdateFacade/checkvalidation/");
        }
        //public MOMUpdateDTO save_excel_data(MOMUpdateDTO obj)
        //{
        //    return COMMNFUN.POSTDataaHRMS(obj, "MOMUpdateFacade/save_excel_data/");
        //}
        //================================================================//////////report//////////////////////

        public MOMUpdateDTO getreport(MOMUpdateDTO obj)
        {
            return COMMNFUN.POSTDataaHRMS(obj, "MOMUpdateFacade/getreport/");
        }
        public MOMUpdateDTO showreport(MOMUpdateDTO obj)
        {
            return COMMNFUN.POSTDataaHRMS(obj, "MOMUpdateFacade/showreport/");
        }
        public MOMUpdateDTO viewdetails(MOMUpdateDTO obj)
        {
            return COMMNFUN.POSTDataaHRMS(obj, "MOMUpdateFacade/viewdetails/");
        }



        ///==/////////////////////////////////Approval===////////////////////
        public MOMUpdateDTO getdata(MOMUpdateDTO obj)
        {
            return COMMNFUN.POSTDataaHRMS(obj, "MOMUpdateFacade/getdata/");
        }
        public MOMUpdateDTO approvaldata(MOMUpdateDTO obj)
        {
            return COMMNFUN.POSTDataaHRMS(obj, "MOMUpdateFacade/approvaldata/");
        }
        public MOMUpdateDTO getstatus(MOMUpdateDTO obj)
        {
            return COMMNFUN.POSTDataaHRMS(obj, "MOMUpdateFacade/getstatus/");
        }
        public MOMUpdateDTO oldmeeting(MOMUpdateDTO obj)
        {
            return COMMNFUN.POSTDataaHRMS(obj, "MOMUpdateFacade/oldmeeting/");
        }
        public MOMUpdateDTO sendapprovalMail(MOMUpdateDTO obj)
        {
            return COMMNFUN.POSTDataaHRMS(obj, "MOMUpdateFacade/sendapprovalMail/");
        }

        public MOMUpdateDTO SaveVendors(MOMUpdateDTO obj)
        {
            return COMMNFUN.POSTDataaHRMS(obj, "MOMUpdateFacade/SaveVendors/");
        }
        public MOMUpdateDTO onloadVendors(MOMUpdateDTO obj)
        {
            return COMMNFUN.POSTDataaHRMS(obj, "MOMUpdateFacade/onloadVendors/");
        }
        
    }
}
