using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOffice.com.vaps.Interfaces
{
   public interface GeneralRequestProcessInterface
    {
        /////punch request ////
        FO_Emp_PunchDTO getpunch(FO_Emp_PunchDTO data);
        FO_Emp_PunchDTO emprequest(FO_Emp_PunchDTO data);
        FO_Emp_PunchDTO grsavedetail(FO_Emp_PunchDTO data);
        //Approval
        FO_Emp_PunchDTO getdata(FO_Emp_PunchDTO data);
        FO_Emp_PunchDTO getstatus(FO_Emp_PunchDTO data);
        FO_Emp_PunchDTO approvaldata(FO_Emp_PunchDTO data);
    }
}
