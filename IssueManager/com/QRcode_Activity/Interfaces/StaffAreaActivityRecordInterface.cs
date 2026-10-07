using PreadmissionDTOs.com.vaps.IssueManager.QRcode_Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.QRcode_Activity.Interfaces
{
  public  interface StaffAreaActivityRecordInterface
    {
        StaffAreaActivityRecordDTO getloaddata(StaffAreaActivityRecordDTO data);

        StaffAreaActivityRecordDTO savedetails(StaffAreaActivityRecordDTO data);
        StaffAreaActivityRecordDTO Activitysavedetails(StaffAreaActivityRecordDTO data);

    }
}
