using CommonLibrary;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VisitorsManagement
{
    public class ApprovalInOutStatusDelegate
    {

        CommonDelegate<ApprovalInOutStatus_DTO, ApprovalInOutStatus_DTO> COMVISITOR = new CommonDelegate<ApprovalInOutStatus_DTO, ApprovalInOutStatus_DTO>();
        public ApprovalInOutStatus_DTO getDetails(ApprovalInOutStatus_DTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "ApprovalInOutStatusFacade/getDetails/");
        }

        public ApprovalInOutStatus_DTO EditDetails(ApprovalInOutStatus_DTO id)
        {
            return COMVISITOR.POSTDataVisitors(id, "ApprovalInOutStatusFacade/EditDetails/");
        }
        public ApprovalInOutStatus_DTO saveData(ApprovalInOutStatus_DTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "ApprovalInOutStatusFacade/saveData/");
        }
        public ApprovalInOutStatus_DTO change_radiobutton(ApprovalInOutStatus_DTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "ApprovalInOutStatusFacade/change_radiobutton/");
        }

    }
}
