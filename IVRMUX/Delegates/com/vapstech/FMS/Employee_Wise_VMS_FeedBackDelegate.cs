using CommonLibrary;
using PreadmissionDTOs.com.vaps.FMS;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.FMS
{
    public class Employee_Wise_VMS_FeedBackDelegate
    {
        CommonDelegate<Employee_Wise_VMS_FeedBackDTO, Employee_Wise_VMS_FeedBackDTO> _comm = new CommonDelegate<Employee_Wise_VMS_FeedBackDTO, Employee_Wise_VMS_FeedBackDTO>();
        CommonDelegate<HW_HeathCheckUpDTO, HW_HeathCheckUpDTO> _commm = new CommonDelegate<HW_HeathCheckUpDTO, HW_HeathCheckUpDTO>();

        public Employee_Wise_VMS_FeedBackDTO getfeedback(Employee_Wise_VMS_FeedBackDTO data)
        {
            return _comm.FMSGetPostData(data, "Employee_Wise_VMS_FeedBackFacade/getfeedback");
        }
        public Employee_Wise_VMS_FeedBackDTO Savedata(Employee_Wise_VMS_FeedBackDTO data)
        {
            return _comm.FMSGetPostData(data, "Employee_Wise_VMS_FeedBackFacade/Savedata");
        }

        //===================================================
        
        public HW_HeathCheckUpDTO GetMonthYear(HW_HeathCheckUpDTO data)
        {
            return _commm.FMSGetPostData(data, "Employee_Wise_VMS_FeedBackFacade/GetMonthYear");
        }
        public HW_HeathCheckUpDTO employeefeedback(HW_HeathCheckUpDTO data)
        {
            return _commm.FMSGetPostData(data, "Employee_Wise_VMS_FeedBackFacade/employeefeedback");
        }

        public HW_HeathCheckUpDTO savecallcenter(HW_HeathCheckUpDTO data)
        {
            return _commm.FMSGetPostData(data, "Employee_Wise_VMS_FeedBackFacade/savecallcenter");
        }
        public HW_HeathCheckUpDTO ViewHealthCheckUpFiles(HW_HeathCheckUpDTO data)
        {
            return _commm.FMSGetPostData(data, "Employee_Wise_VMS_FeedBackFacade/ViewHealthCheckUpFiles");
        }


    }
}
