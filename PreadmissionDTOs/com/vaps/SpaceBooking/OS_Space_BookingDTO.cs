using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.SpaceBooking
{
    public class OS_Space_BookingDTO
    {
        public long MI_Id { get; set; }
        public long OSUREG_Id { get; set; }
        public long HRMR_Id { get; set; }
        public long OSSPBOOK_NoOfPeople { get; set; }
        public long OSSPBOOK_BillingPinCode { get; set; }
        public long IVRMMS_Id { get; set; }
        public long IVRMMC_Id { get; set; }
        public long? OSSPBOOK_CreatedBy { get; set; }
        public long? OSSPBOOK_UpdatedBy { get; set; }
        public string OSSPBOOK_TransactionNo { get; set; }
        public string OSSPBOOK_DurationType { get; set; }
        public string OSSPBOOK_StartTime { get; set; }
        public string OSSPBOOK_EndTime { get; set; }
        public string OSSPBOOK_BillingName { get; set; }
        public string OSSPBOOK_GSTNo { get; set; }
        public string OSSPBOOK_BillingAdd1 { get; set; }
        public string OSSPBOOK_BillingAdd2 { get; set; }
        public string OSSPBOOK_BillingAdd3 { get; set; }
        public string OSSPBOOK_BillingAdd4 { get; set; }
        public string OSSPBOOK_BillingCity { get; set; }
        public string OSSPBOOK_Status { get; set; }
        public string OSUREG_UserName { get; set; }
        public string OS_username { get; set; }
        public string OS_email { get; set; }
        public string OS_subject { get; set; }
        public string OS_message { get; set; }
        public bool retrval { get; set; }
     
        public DateTime? OSSPBOOK_StartDate { get; set; }
        public DateTime? OSSPBOOK_EndDate { get; set; }
        public DateTime? OSSPBOOK_CreatedDate { get; set; }
        public DateTime? OSSPBOOK_UpdatedDate { get; set; }
        public bool OSSPBOOK_ActiveFlag { get; set; }
        public string response { get; set; }
        public long OSSPBOOK_Id { get; set; }
        public Array onlinepaymentdata { get; set; }
     
        public Array bookingdetails { get; set; }

        public long HRMF_Id { get; set; }

        //online payment booking

        public string HRMA_Merchant_Id { get; set; }
        public string HRMA_Salt_Id { get; set; }
        public string HRMA_Authorisation_Id { get; set; }
        public string HRMF_URL { get; set; }
        public string HRMF_Submerchant_Id { get; set; }

        public string IMPG_PGFlag { get; set; }

        public long payable_Amount { get; set; }

        public string onlinepaygteway { get; set; }

        public string orderId { get; set; }

        //===================payment save
        public string FYP_Bank_Name { get; set; }
        public string fyp_transaction_id { get; set; }
        public string HRMF_FloorName { get; set; }
        public string HRMB_BuildingName { get; set; }
        public string HRMR_RoomName { get; set; }
        public long FYP_Tot_Amount { get; set; }
    }
}
