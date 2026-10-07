using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.SpaceBooking;

namespace corewebapi18072016.Delegates.com.vapstech.SpaceBooking
{
    public class MasterSpaceDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<MasterSpaceDTO, MasterSpaceDTO> _areazone = new CommonDelegate<MasterSpaceDTO, MasterSpaceDTO>();

        public MasterSpaceDTO getdata(int id)
        {
            return _areazone.GetDataByIdSpaceBooking(id, "MasterSpaceFacade/getdata/");
        }
        public MasterSpaceDTO savedata(MasterSpaceDTO data)
        {
            return _areazone.POSTDataSpaceBooking(data, "MasterSpaceFacade/savedata/");
        }
        public MasterSpaceDTO geteditdata(MasterSpaceDTO data)
        {
            return _areazone.POSTDataSpaceBooking(data, "MasterSpaceFacade/geteditdata/");
        }
        public MasterSpaceDTO activedeactive(MasterSpaceDTO data)
        {
            return _areazone.POSTDataSpaceBooking(data, "MasterSpaceFacade/activedeactive/");
        }
    }

}
