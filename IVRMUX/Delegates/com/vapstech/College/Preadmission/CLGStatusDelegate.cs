using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Admission;
using PreadmissionDTOs.com.vaps.College.Preadmission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Preadmission
{
    public class CLGStatusDelegate 
    {
        CommonDelegate<CollegePreadmissionstudnetDto, CollegePreadmissionstudnetDto> common = new CommonDelegate<CollegePreadmissionstudnetDto, CollegePreadmissionstudnetDto>();
      
        public CollegePreadmissionstudnetDto Getdetails(CollegePreadmissionstudnetDto obj)
        {
            return common.CollegePOSTData(obj, "CLGStatusFacade/Getdetails/");
        }
        public CollegePreadmissionstudnetDto getCourse(CollegePreadmissionstudnetDto data)
        {
            return common.CollegePOSTData(data, "CLGStatusFacade/getCourse/");
        }
        public CollegePreadmissionstudnetDto getBranch(CollegePreadmissionstudnetDto dt)
        {
            return common.CollegePOSTData(dt, "CLGStatusFacade/getBranch/");
        }
        public CollegePreadmissionstudnetDto SearchData(CollegePreadmissionstudnetDto dt)
        {
            return common.CollegePOSTData(dt, "CLGStatusFacade/SearchData/");
        }

    }
}
