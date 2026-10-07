using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.SpaceBooking
{
   public class Booking_DetailsDTO
    {
        public long HRMR_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMR_RoomName { get; set; }
        public long HRMB_Id { get; set; }
        public long HRMF_Id { get; set; }
        public bool HRMR_ActiveFlag { get; set; }
        public long HRMR_CreatedBy { get; set; }
        public long HRMR_UpdatedBy { get; set; }
        public bool HRMR_OutSideBookingFlg { get; set; }
        public long HRMR_Capacity { get; set; }
        public decimal HRMR_RentPerDay { get; set; }
        public long HRMR_NoOfHrs { get; set; }
        public decimal HRMR_RentPerHour { get; set; }
        public string HRMR_TypeFlag { get; set; }
        public string HRMR_Desc { get; set; }

        /////
        public long OSSPBOOK_Id { get; set; }
        //  public long HRMR_Id { get; set; }
        public long OSUREG_Id { get; set; }
        public long OSSPBOOK_TransactionNo { get; set; }
        public string OSSPBOOK_DurationType { get; set; }
        public long OSSPBOOK_NumberOfPeople { get; set; }
        public string OSSPBOOK_BillingName { get; set; }
        public long OSSPBOOK_GSTNo { get; set; }
        public string OSSPBOOK_BillingAdd1 { get; set; }
        public string OSSPBOOK_BillingAdd2 { get; set; }
        public string OSSPBOOK_BillingAdd3 { get; set; }
        public string OSSPBOOK_BillingAdd4 { get; set; }
        public long OSSPBOOK_BillingPinCode { get; set; }
        public string OSSPBOOK_BillingCity { get; set; }
        public long IVRMMS_Id { get; set; }
        public long IVRMMC_Id { get; set; }
        public bool OSSPBOOK_ActiveFlag { get; set; }
        public string OSSPBOOK_StartTime { get; set; }
        public string OSSPBOOK_EndTime { get; set; }
        public string OSSPBOOK_Status { get; set; }
        public DateTime? OSSPBOOK_StartDate { get; set; }
        public DateTime? OSSPBOOK_EndDate { get; set; }
        public DateTime? fromdate1 { get; set; }
        public DateTime? fromdate2 { get; set; }
        public DateTime? OSSPBOOK_CreatedDate { get; set; }
        public DateTime? OSSPBOOK_UpdatedDate { get; set; }
        public long? OSSPBOOK_CreatedBy { get; set; }
        public long? OSSPBOOK_UpdatedBy { get; set; }
        //////////////
        public Array bookingList { get; set; }
        public Array bookinglist_Report { get; set; }
        public Array enqlist_Report { get; set; }
        public string bookingtype { get; set; }
    }
}
