using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.Tenants
{
    public class TenantDashboardDTO
    {
        public string Tenant_flag { get; set; }
        public long User_Id { get; set; }
        public long roleid { get; set; }
        public long MI_Id { get; set; }
        public Array tenants_details { get; set; }
        public Array Payment_details { get; set; }
        public bool dueday_flag { get; set; }
        public bool dueyear_flag { get; set; }
        public string TENANTAGR_TenantName { get; set; }
        public string TENANTAGR_EmailId { get; set; }
        public string TENANTAGR_ContactNo { get; set; }
        public string strForm { get; set; }
        public long ASMAY_Id { get; set; }

        public DateTime TENANTAGR_AgrFromDate { get; set; }
        public DateTime TENANTAGR_AgrToDate { get; set; }


        public long TENANTAGR_Id { get; set; }
        public decimal TENANTAGR_AdvanceAmount { get; set; }
        public decimal TENANTAGR_MonthlyRent { get; set; }
        public decimal TENANTAGRPAY_Amount { get; set; }
        public DateTime? TENANTAGRPAY_PaymentDate { get; set; }
        public DateTime last_paid_date { get; set; }
        public Array get_month_details { get; set; }
        public Array get_fine_details { get; set; }
        public Array get_tenant_details { get; set; }
        public Array get_tenant_paid_details { get; set; }
        public Array get_tenant_online_details { get; set; }
        public Array get_online_month_details { get; set; }
        public Array fillpaymentgateway { get; set; }
        //Offline Payment
        public Array building_array { get; set; }
        public Array floor_array { get; set; }
        public Array room_array { get; set; }

        public Array fill_bankdetails { get; set; }

        public long HRMF_Id { get; set; }
        public long Id { get; set; }
        public string HRMF_FloorName { get; set; }
        public long HRMB_Id { get; set; }
        public string HRMB_BuildingName { get; set; }
        public long HRMR_Id { get; set; }
        public string HRMR_RoomName { get; set; }
        public long TENANTAGRPAY_Id { get; set; }
        public long TENANTAGR_UserId { get; set; }
        public string message { get; set; }
        public long TENANTAGRPAY_ModeOfPayment { get; set; }
        public string TENANTAGRPAY_Remarks { get; set; }

        public string searchfilter { get; set; }
        public Array fillstudlist { get; set; }

        public Array modeofpayment_array { get; set; }

        public Array tenant_array { get; set; }
        public Array bank_array { get; set; }
        public Array editdata { get; set; }

        // public Array Month_detailsarray { get; set; }
        public string returnval { get; set; }
        public string IVRMMOD_ModeOfPayment { get; set; }
        public decimal TENANTAGRPAY_PaymentMonth { get; set; }
        public long TENANTAGRPAY_PaymentYear { get; set; }
        public string IVRM_Month_Name { get; set; }
        public long IVRMMOD_Id { get; set; }
        public string IVRMMOD_Flag { get; set; }

        public string TENANTAGRPAY_Bankname { get; set; }
        public string TENANTAGRPAY_Chequeno { get; set; }
        public string TENANTAGRPAY_ReceiptNo { get; set; }
        public DateTime? TENANTAGRPAY_Chequedate { get; set; }
        public Master_NumberingDTO transnumconfigsettings { get; set; }
        public Month_detailsarray1[] Month_detailsarray { get; set; }
        public string FPGD_PGName { get; set; }
        public string FPGD_Image { get; set; }
        public long FPGD_Id { get; set; }
        public string TENANTAGR_Address { get; set; }
        public long TENANTAGR_NoOfOccupants { get; set; }
        public string onlinepaygteway { get; set; }
        public Array paydet { get; set; }
        public Array recenocol { get; set; }
        public string merchantkey { get; set; }
        public string trans_id { get; set; }
        public string razorpay_payment_id { get; set; }
        public string FPGD_SubMerchantId { get; set; }
        public string order_id { get; set; }
        public decimal TENANTAGRONLPAY_Amount { get; set; }
        public DateTime? TENANTAGRONLPAY_PaymentDate { get; set; }
        public long TENANTAGRONLPAY_PaymentMonth { get; set; }
        public long TENANTAGRONLPAY_PaymentYear { get; set; }
        public long TENANTAGRONLPAY_Id { get; set; }
        public string status { get; set; }
        public Array institutiondet { get; set; }
        public string name { get; set; }
        public DateTime tenantagrpay_PaymentDate { get; set; }
        public string type { get; set; }
        public string TENANTAGRONLPAY_Flag { get; set; }
        public string MI_Logo { get; set; }

        //waterbill

        public long TENANTMWBILLRW_Id { get; set; }
        public long TENANTMWBILL_Id { get; set; }
        //public long HRMR_ID { get; set; }

        public decimal TENANTMWBILLRW_Amount { get; set; }
        public string TENANTMWBILLRW_Remarks { get; set; }
        public decimal TENANTMWBILLRW_AmountPaid { get; set; }
        public bool TENANTMWBILLRW_PaidFlg { get; set; }
        public bool TENANTMWBILLRW_ActiveFlg { get; set; }
        public DateTime? TENANTMWBILLRW_CreatedDate { get; set; }
        public DateTime? TENANTMWBILLRW_UpdatedDate { get; set; }
        public long TENANTMWBILLRW_CreatedBy { get; set; }
        public long TENANTMWBILLRW_UpdatedBy { get; set; }
        public string TENANTAGRPAY_Flag { get; set; }
        // public string message { get; set; }


        public string HRMBD_BankName { get; set; }
        public string HRMBD_BankAccountNo { get; set; }

        public string HRMBD_BankAddress { get; set; }
        public string HRMBD_BranchName { get; set; }
        public string HRMBD_IFSCCode { get; set; }
        public string HRMBD_AccountHolderName { get; set; }
        public string HRMBD_BankAccountType { get; set; }


        public string waterflag { get; set; }
        public string rentflag { get; set; }
        public string Electricityflag { get; set; }
        public string Advanceflag { get; set; }
        public string Otherflag { get; set; }

        public Array viewarray { get; set; }

        public decimal? TENANTAGRPAY_Fine { get; set; }

        public MonthdetailsarrayBal1[] MonthdetailsarrayBal { get; set; }
        public Array get_balnce_details { get; set; }
        public Array tenant_monthly_paid_details { get; set; }

        public long total_payable { get; set; }
        public decimal? TENANTAGRPAY_Balance { get; set; }
        public decimal? TENANTAGRPAY_ActualFineAmount { get; set; }
        public decimal? TENANTAGRPAY_FinePaidAmount { get; set; }


    }
    public class Month_detailsarray1
    {
        public long MonthId { get; set; }
        public long TENANTAGRPAY_PaymentYear { get; set; }
        public long TENANTAGR_MonthlyRent { get; set; }
        public decimal fine { get; set; }
        public string Name { get; set; }

    }


    public class MonthdetailsarrayBal1
    {
        public long MonthId { get; set; }
        public long TENANTAGRPAY_PaymentYear { get; set; }
        public long TENANTAGR_MonthlyRent { get; set; }
        public decimal fine { get; set; }
        public string Name { get; set; }
        public string yearid { get; set; }

    }
    public class PAYTM
    {
        public string MID { get; set; }
        public string ORDER_ID { get; set; }
        public string CUST_ID { get; set; }
        public decimal TXN_AMOUNT { get; set; }
        public string CHANNEL_ID { get; set; }

        public string INDUSTRY_TYPE_ID { get; set; }
        public string WEBSITE { get; set; }
        public string CHECKSUMHASH { get; set; }
        public long? MOBILE_NO { get; set; }
        public string EMAIL { get; set; }//till this are mandatory
        public string CALLBACK_URL { get; set; }
        public string PAYMENT_MODE_ONLY { get; set; }
        public string AUTH_MODE { get; set; }
        public string PAYMENT_TYPE_ID { get; set; }
        public string CARD_TYPE { get; set; }
        public string BANK_CODE { get; set; }
        public string PROMO_CAMP_ID { get; set; }
        public string THEME { get; set; }

        public string REQUEST_TYPE { get; set; }
        public string payu_URL { get; set; }

        public string MERC_UNQ_REF { get; set; }

        public string status { get; set; }
        public string txnid { get; set; }

        public string BANKTXNID { get; set; }
        public long UserId { get; set; }

    }
    public class EBSpayment
    {
        public int account_id { get; set; }
        public int channel { get; set; }
        public string currency { get; set; }
        public string ba_B25914CA4 { get; set; }
        public string ba_B259148DC { get; set; }
        public string split_profile_code { get; set; }

        public string reference_no { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string city { get; set; }

        public string state { get; set; }
        public string postal_code { get; set; }
        public string country { get; set; }
        public string return_url { get; set; }
        public string ship_name { get; set; }
        public string ship_address { get; set; }

        public string ship_city { get; set; }
        public string amount { get; set; }
        public string email { get; set; }
        public long phone { get; set; }
        public string mode { get; set; }
        public string ship_state { get; set; }
        public string ship_country { get; set; }
        public long ship_phone { get; set; }
        public string algo { get; set; }
        public string ship_postal_code { get; set; }
        public string name_on_card { get; set; }
        public long? card_number { get; set; }
        public long? card_expiry { get; set; }
        public long? card_cvv { get; set; }
        public string secure_hash { get; set; }
        public string V3URL { get; set; }

    }

    public class razorpaydetails
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}
