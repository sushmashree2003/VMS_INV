using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.SpaceBooking;
using SpaceBooking.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpaceBooking.Controllers
{
    [Route("api/[controller]")]
    public class Booking_DetailsFacade : Controller
    {
        public Booking_DetailsInterface _Interface;
        public Booking_DetailsFacade(Booking_DetailsInterface parameter)
        {
            _Interface = parameter;
        }
        [Route("getdetails")]
        public Booking_DetailsDTO getdetails([FromBody] Booking_DetailsDTO data)
        {
            return _Interface.getdetails(data);
        }
        [Route("booking_report")]
        public Task<Booking_DetailsDTO> booking_report([FromBody]Booking_DetailsDTO data)
        {
            return _Interface.booking_report(data);
        }
    }
}
