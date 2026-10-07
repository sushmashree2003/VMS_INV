using CommonLibrary;
using DomainModel.Model;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Portals.Tenants
{
    public class TenantOnlinePaymentDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<TenantDashboardDTO, TenantDashboardDTO> COMMM = new CommonDelegate<TenantDashboardDTO, TenantDashboardDTO>();
        CommonDelegate<PaymentDetails, PaymentDetails> COMM = new CommonDelegate<PaymentDetails, PaymentDetails>();
        CommonDelegate<PaymentDetails.easybuzz, PaymentDetails.easybuzz> COMMeasebuzz = new CommonDelegate<PaymentDetails.easybuzz, PaymentDetails.easybuzz>();
        public TenantDashboardDTO getalldetails(TenantDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "TenantOnlinePaymentFacade/getalldetails/");
        }
        public TenantDashboardDTO onchangetype(TenantDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "TenantOnlinePaymentFacade/onchangetype/");
        }

        public TenantDashboardDTO getamountdetails(TenantDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "TenantOnlinePaymentFacade/getamountdetails/");
        }
        public TenantDashboardDTO getamtwaterdetails(TenantDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "TenantOnlinePaymentFacade/getamtwaterdetails/");
        }




        //Offline payment
        public TenantDashboardDTO getallofflinedetails(TenantDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "TenantOfflinePaymentFacade/getallofflinedetails/");
        }
        public TenantDashboardDTO getfloor(TenantDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "TenantOfflinePaymentFacade/getfloor/");
        }


        public TenantDashboardDTO getroom(TenantDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "TenantOfflinePaymentFacade/getroom/");
        }
        public TenantDashboardDTO searchfilter(TenantDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "TenantOfflinePaymentFacade/searchfilter/");
        }
        public TenantDashboardDTO getmonthdetails(TenantDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "TenantOfflinePaymentFacade/getmonthdetails/");
        }
        public TenantDashboardDTO gettenantdetails(TenantDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "TenantOfflinePaymentFacade/gettenantdetails/");
        }



        public TenantDashboardDTO savedetails(TenantDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "TenantOfflinePaymentFacade/savedetails/");
        }

        public TenantDashboardDTO GetSelectedRowDetails(TenantDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "TenantOfflinePaymentFacade/GetSelectedRowDetails/");
        }


        //Online Payment
        public TenantDashboardDTO generatehashseq(TenantDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "TenantOnlinePaymentFacade/generatehashsequence/");
        }

        public PaymentDetails razorgetpaymentresponse(PaymentDetails data)
        {
            return COMM.POSTPORTALData(data, "TenantOnlinePaymentFacade/razorgetpaymentresponse/");
        }

        public TenantDashboardDTO onchangetype_offline(TenantDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "TenantOfflinePaymentFacade/onchangetype_offline/");
        }

        public TenantDashboardDTO viewuploadflies(TenantDashboardDTO obj)
        {
            return COMMM.POSTPORTALData(obj, "TenantOfflinePaymentFacade/viewuploadflies/");
        }

        public TenantDashboardDTO getfinedetails(TenantDashboardDTO obj)
        {
            return COMMM.POSTPORTALData(obj, "TenantOfflinePaymentFacade/getfinedetails/");
        }

        public TenantDashboardDTO getbreakup(TenantDashboardDTO obj)
        {
            return COMMM.POSTPORTALData(obj, "TenantOfflinePaymentFacade/getbreakup/");
        }

        public TenantDashboardDTO getbalnce(TenantDashboardDTO obj)
        {
            return COMMM.POSTPORTALData(obj, "TenantOfflinePaymentFacade/getbalncedetails/");
        }


        public PaymentDetails.easybuzz paymentresponseeasybuzz(PaymentDetails.easybuzz data)
        {
            return COMMeasebuzz.POSTPORTALData(data, "TenantOnlinePaymentFacade/paymentresponseeasybuzz/");
        }

        public TenantDashboardDTO getMonthlyPaid_details(TenantDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "TenantOfflinePaymentFacade/getMonthlyPaid_details/");
        }

        public TenantDashboardDTO savefinedata(TenantDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "TenantOfflinePaymentFacade/savefinedata/");
        }
    }
}
    