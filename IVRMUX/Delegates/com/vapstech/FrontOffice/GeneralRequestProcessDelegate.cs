using CommonLibrary;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.FrontOffice
{
    public class GeneralRequestProcessDelegate
    {
        CommonDelegate<FO_Emp_PunchDTO, FO_Emp_PunchDTO> COMFRNT = new CommonDelegate<FO_Emp_PunchDTO, FO_Emp_PunchDTO>();
        /////punch request ////
        public FO_Emp_PunchDTO getpunch(FO_Emp_PunchDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "GeneralRequestProcessFacade/getpunch/");
        }
        public FO_Emp_PunchDTO emprequest(FO_Emp_PunchDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "GeneralRequestProcessFacade/emprequest/");
        }
        public FO_Emp_PunchDTO grsavedetail(FO_Emp_PunchDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "GeneralRequestProcessFacade/grsavedetail/");
        }
        //approval

        public FO_Emp_PunchDTO getdetails(FO_Emp_PunchDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "GeneralRequestProcessFacade/getalldetails/");
        }
        public FO_Emp_PunchDTO getstatus(FO_Emp_PunchDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "GeneralRequestProcessFacade/getstatus/");
        }
        public FO_Emp_PunchDTO approvaldata(FO_Emp_PunchDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "GeneralRequestProcessFacade/approvaldata/");
        }
    }
}
