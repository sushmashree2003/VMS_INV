using AssetTrackingServiceHub.com.vaps.AssetTag.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Facade
{
    [Route("api/[controller]")]
    public class AssetTagSMS_NotificationFacadeController : Controller
    {
        public AssetTagSMS_NotificationInterface _inter;

        public AssetTagSMS_NotificationFacadeController(AssetTagSMS_NotificationInterface maspag)
        {
            _inter = maspag;
        }
        [Route("getdata/{id:int}")]
        public AssetTagDTO getdata(int id)
        {
            AssetTagDTO dto = new AssetTagDTO();
            dto.t_id = id;
            return _inter.getdata(dto);
        }


        [Route("mailDemo")]
        public AssetTagDTO mailDemo([FromBody] AssetTagDTO dto)
        {
            
            return _inter.mailDemo(dto);
        }




    }
}
