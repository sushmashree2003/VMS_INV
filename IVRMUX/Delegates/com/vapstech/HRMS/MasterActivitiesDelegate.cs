using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
using PreadmissionDTOs.com.vaps.VMS.Training;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class MasterActivitiesDelegate
    {
        CommonDelegate<MasterActivitiesDTO, MasterActivitiesDTO> _com = new CommonDelegate<MasterActivitiesDTO, MasterActivitiesDTO>();

        public MasterActivitiesDTO onloaddata(MasterActivitiesDTO data)
        {
            return _com.POSTDataHRMS(data, "MasterActivitiesFacade/onloaddata");
        }
        public MasterActivitiesDTO saverecord(MasterActivitiesDTO data)
        {
            return _com.POSTDataHRMS(data, "MasterActivitiesFacade/saverecord");
        }
        public MasterActivitiesDTO deactiveY(MasterActivitiesDTO data)
        {
            return _com.POSTDataHRMS(data, "MasterActivitiesFacade/deactiveY");
        }
    }
}


