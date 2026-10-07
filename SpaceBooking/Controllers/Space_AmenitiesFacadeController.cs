using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.SpaceBooking;
using SpaceBooking.Interfaces;

namespace SpaceBooking.Controllers
{
    [Produces("application/json")]
    [Route("api/Space_AmenitiesFacade")]
    public class Space_AmenitiesFacadeController : Controller
    {
        public Space_Amenities_Interface _smIntr;
        public Space_AmenitiesFacadeController(Space_Amenities_Interface am)
        {
            _smIntr = am;
        }

        [Route("get_master_amenities")]
        public SpaceBookingDTO get_master_amenities([FromBody] SpaceBookingDTO dto)
        {
            return _smIntr.get_master_amenities(dto);
        }
        [Route("SaveEdit_MasterAmenities")]
        public SpaceBookingDTO SaveEdit_MasterAmenities([FromBody] SpaceBookingDTO dto)
        {
            return _smIntr.SaveEdit_MasterAmenities(dto);
        }
        [Route("Edit_MasterAmenities")]
        public SpaceBookingDTO Edit_MasterAmenities([FromBody] SpaceBookingDTO dto)
        {
            return _smIntr.Edit_MasterAmenities(dto);
        }
        [Route("deactivate_MasterAmenities")]
        public SpaceBookingDTO deactivate_MasterAmenities([FromBody] SpaceBookingDTO dto)
        {
            return _smIntr.deactivate_MasterAmenities(dto);
        }
    }
}