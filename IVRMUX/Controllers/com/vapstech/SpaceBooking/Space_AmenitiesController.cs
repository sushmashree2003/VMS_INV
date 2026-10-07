using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.SpaceBooking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.SpaceBooking;

namespace IVRMUX.Controllers.com.vapstech.SpaceBooking
{
    [Produces("application/json")]
    [Route("api/Space_Amenities")]
    public class Space_AmenitiesController : Controller
    {
        Space_AmenitiesDelegate sad = new Space_AmenitiesDelegate();

        [Route("get_master_amenities/{id:int}")]
        public SpaceBookingDTO get_master_amenities (int id)
        {
            SpaceBookingDTO dto = new SpaceBookingDTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return sad.get_master_amenities(dto);
        }
        [Route("SaveEdit_MasterAmenities")]
        public SpaceBookingDTO SaveEdit_MasterAmenities([FromBody] SpaceBookingDTO dto)
        {
             dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return sad.SaveEdit_MasterAmenities(dto);
        }
        [Route("Edit_MasterAmenities/{id:int}")]
        public SpaceBookingDTO Edit_MasterAmenities(int id)
        {
            SpaceBookingDTO dto = new SpaceBookingDTO();
             dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRMAM_Id = id;
            return sad.Edit_MasterAmenities(dto);
        }
        [Route("deactivate_MasterAmenities")]
        public SpaceBookingDTO deactivate_MasterAmenities([FromBody]SpaceBookingDTO dto)
        {
            
             dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return sad.deactivate_MasterAmenities(dto);
        }
    }
}