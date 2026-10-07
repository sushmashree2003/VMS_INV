using PreadmissionDTOs.com.vaps.FMS;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMS.Com.Interface
{
    public  interface Employee_Wise_VMS_FeedBackInterface
    {
        Employee_Wise_VMS_FeedBackDTO getfeedback(Employee_Wise_VMS_FeedBackDTO data);
        Employee_Wise_VMS_FeedBackDTO Savedata(Employee_Wise_VMS_FeedBackDTO data);

        HW_HeathCheckUpDTO GetMonthYear(HW_HeathCheckUpDTO data);
        HW_HeathCheckUpDTO employeefeedback(HW_HeathCheckUpDTO data);
        HW_HeathCheckUpDTO savecallcenter(HW_HeathCheckUpDTO data);
        HW_HeathCheckUpDTO ViewHealthCheckUpFiles(HW_HeathCheckUpDTO data);
    }
}
