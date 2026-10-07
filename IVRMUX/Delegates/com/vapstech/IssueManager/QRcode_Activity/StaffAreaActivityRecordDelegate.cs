using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.QRcode_Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.QRcode_Activity
{
    public class StaffAreaActivityRecordDelegate
    {
        CommonDelegate<StaffAreaActivityRecordDTO, StaffAreaActivityRecordDTO> COMINV = new CommonDelegate<StaffAreaActivityRecordDTO, StaffAreaActivityRecordDTO>();

        public StaffAreaActivityRecordDTO getloaddata(StaffAreaActivityRecordDTO data)
        {
            return COMINV.POSTIssueManager(data, "StaffAreaActivityRecordFacade/getloaddata/");
        }


        public StaffAreaActivityRecordDTO savedetails(StaffAreaActivityRecordDTO data)
        {
            return COMINV.POSTIssueManager(data, "StaffAreaActivityRecordFacade/savedetails/");
        }

        public StaffAreaActivityRecordDTO Activitysavedetails(StaffAreaActivityRecordDTO data)
        {
            return COMINV.POSTIssueManager(data, "StaffAreaActivityRecordFacade/Activitysavedetails/");
        }

    }
}
