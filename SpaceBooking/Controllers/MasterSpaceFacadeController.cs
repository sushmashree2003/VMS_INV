using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.SpaceBooking;
using SpaceBooking.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SpaceBooking.Controllers
{
    [Route("api/[controller]")]
    public class MasterSpaceFacadeController : Controller
    {

        public MasterSpaceInterface _areaint;

        public MasterSpaceFacadeController(MasterSpaceInterface adstu)
        {
            _areaint = adstu;
        }

  
        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [Route("getdata/{id:int}")]
        public OS_EnquiryDTO getdata(int id)
        {

            return _areaint.getdata(id);
        }

        [Route("savedata")]
        public OS_EnquiryDTO savedata([FromBody]OS_EnquiryDTO data)
        {
            return _areaint.savedata(data);
        }

        [Route("saveEnquiry")]
        public OS_EnquiryDTO saveEnquiry([FromBody]OS_EnquiryDTO data)
        {
            return _areaint.saveEnquiry(data);
        }
        [Route("geteditdata")]
        public OS_EnquiryDTO geteditdata([FromBody] OS_EnquiryDTO data)
        {
            return _areaint.geteditdata(data);
        }
        [Route("activedeactive")]
        public OS_EnquiryDTO activedeactive([FromBody] OS_EnquiryDTO data)
        {
            return _areaint.activedeactive(data);
        }
        [Route("loadcountry")]
        public OS_EnquiryDTO loadcountry([FromBody] OS_EnquiryDTO data)
        {
            return _areaint.loadcountry(data);
        }
        [Route("get_state")]
        public OS_EnquiryDTO get_state([FromBody] OS_EnquiryDTO data)
        {
            return _areaint.get_state(data);
        }

        // POST api/values
        //[HttpPost]
        [Route("SaveBooking")]
        public OS_Space_BookingDTO SaveBooking([FromBody] OS_Space_BookingDTO Spacebooking)
        {
            return _areaint.SaveBooking(Spacebooking);
        }
        [Route("Availability")]
        public OS_Space_BookingDTO Availability([FromBody] OS_Space_BookingDTO Spacebooking)
        {
            return _areaint.Availability(Spacebooking);
        }

        [Route("OnlinePaymentDetails")]
        public OS_Space_BookingDTO OnlinePaymentDetails([FromBody] OS_Space_BookingDTO Spacebooking)
        {
            return _areaint.OnlinePaymentDetails(Spacebooking);
        }

        [Route("GenerateOrderId")]
        public OS_Space_BookingDTO GenerateOrderId([FromBody] OS_Space_BookingDTO Spacebooking)
        {
            return _areaint.GenerateOrderId(Spacebooking);
        }
        [Route("UserBookingDetails")]

        [Route("SaveBooking_Payment")]
        public OS_Space_BookingDTO SaveBooking_Payment([FromBody] OS_Space_BookingDTO Spacebooking)
        {
            return _areaint.SaveBooking_Payment(Spacebooking);
        }


        [Route("UserBookingDetails")]
        public OS_Space_BookingDTO UserBookingDetails([FromBody] OS_Space_BookingDTO Spacebooking)
        {
            return _areaint.UserBookingDetails(Spacebooking);
        }

        [Route("send_email")]
        public OS_Space_BookingDTO send_email([FromBody] OS_Space_BookingDTO Spacebooking)
        {
            return _areaint.send_email(Spacebooking);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
