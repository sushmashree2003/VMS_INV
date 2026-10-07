using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class LocationFeeGroupMappingDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<TR_Location_FeeGroup_MappingDTO, TR_Location_FeeGroup_MappingDTO> _areazone = new CommonDelegate<TR_Location_FeeGroup_MappingDTO, TR_Location_FeeGroup_MappingDTO>();

        public TR_Location_FeeGroup_MappingDTO getdata(int id)
        {
            return _areazone.GetDataByIdTransport(id, "LocationFeeGroupMappingFacade/getdata/");
        }
        public TR_Location_FeeGroup_MappingDTO savedata(TR_Location_FeeGroup_MappingDTO data)
        {
            return _areazone.POSTDataTransport(data, "LocationFeeGroupMappingFacade/savedata/");
        }
        public TR_Location_FeeGroup_MappingDTO geteditdata(TR_Location_FeeGroup_MappingDTO data)
        {
            return _areazone.POSTDataTransport(data, "LocationFeeGroupMappingFacade/geteditdata/");
        }
        public TR_Location_FeeGroup_MappingDTO activedeactive(TR_Location_FeeGroup_MappingDTO data)
        {
            return _areazone.POSTDataTransport(data, "LocationFeeGroupMappingFacade/activedeactive/");
        }
    }

}
