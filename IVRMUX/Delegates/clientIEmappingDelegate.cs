using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
using PreadmissionDTOs.com.vaps.VMS.Training;

namespace IVRMUX.Delegates.com.vapstech.VMS.Training
{
    public class clientIEmappingDelegate
    {
        CommonDelegate<clientIEmappingDTO, clientIEmappingDTO> _com = new CommonDelegate<clientIEmappingDTO, clientIEmappingDTO>();

        public clientIEmappingDTO onloaddata(clientIEmappingDTO data)
        {
            return _com.POSTDataHolidayReport(data, "clientIEmappingFacade/onloaddata");
        }
        public clientIEmappingDTO saverecord(clientIEmappingDTO data)
        {
            return _com.POSTDataHolidayReport(data, "clientIEmappingFacade/saverecord");
        }
        public clientIEmappingDTO deactive(clientIEmappingDTO data)
        {
            return _com.POSTDataHolidayReport(data, "clientIEmappingFacade/deactive");
        }

        public clientIEmappingDTO validatedata(clientIEmappingDTO data)
        {
            return _com.POSTDataHolidayReport(data, "clientIEmappingFacade/validatedata/");
        }
    }
}

