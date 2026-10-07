using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.Fee;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.Tenants;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml.Linq;
using Razorpay.Api;
using DomainModel.Model.com.vapstech.Portals.Tenants;
using DomainModel.Model;
using easebuzz_.net;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Security.Cryptography;

namespace PortalHub.com.vaps.Tenant.Services
{
    public class TenantOnlinePaymentImpl : Interface.TenantOnlinePaymentInterface
    {
        private static ConcurrentDictionary<string, TenantDashboardDTO> _login =
        new ConcurrentDictionary<string, TenantDashboardDTO>();
        private PortalContext _portalDashboardContext;
        private readonly DomainModelMsSqlServerContext _db;

        public TenantOnlinePaymentImpl(PortalContext portalDashboardContext, DomainModelMsSqlServerContext db)
        {
            _portalDashboardContext = portalDashboardContext;
            _db = db;
        }
        public TenantDashboardDTO getalldetails(TenantDashboardDTO data)
        {
            try
            {
                //t.TENANTAGR_UserId == data.User_Id
                // var tenant_user = _portalDashboardContext.TenantAgreement.Where(t => t.TENANTAGR_UserId == data.User_Id && t.TENANTAGR_ActiveFlg == true).ToList();
                data.get_tenant_details = _portalDashboardContext.TenantAgreement.Where(t => t.TENANTAGR_UserId == data.User_Id && t.TENANTAGR_ActiveFlg == true).ToArray();

                //  data.TENANTAGR_Id= _portalDashboardContext.TenantAgreement.Select(s=>s.TENANTAGR_Id).Where(t => t.TENANTAGR_UserId == data.User_Id && t.TENANTAGR_ActiveFlg == true);
                data.institutiondet = _portalDashboardContext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                //for (var i = 0; i <= data.get_tenant_details.Length; i++)
                //{

                //    data.TENANTAGR_Id = data.get_tenant_details[i].TENANTAGR_Id;
                //}
                data.TENANTAGR_Id = _portalDashboardContext.TenantAgreement.Where(a => a.TENANTAGR_UserId == data.User_Id).Select(a => a.TENANTAGR_Id).FirstOrDefault();

                //using (var cmd = _portalDashboardContext.Database.GetDbConnection().CreateCommand())
                // {
                //     cmd.CommandText = "tenant_online_month_details";
                //     cmd.CommandType = CommandType.StoredProcedure;
                //     cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                //     {
                //         Value = data.MI_Id
                //     });
                //     cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id", SqlDbType.VarChar)
                //     {
                //         Value = data.TENANTAGR_Id
                //     });
                //     cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                //     {
                //         Value = data.type
                //     });



                //     if (cmd.Connection.State != ConnectionState.Open)
                //         cmd.Connection.Open();
                //     var retObject = new List<dynamic>();
                //     try
                //     {
                //         using (var dataReader = cmd.ExecuteReader())
                //         {
                //             while (dataReader.Read())
                //             {
                //                 var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                 for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                 {
                //                     dataRow.Add(
                //                     dataReader.GetName(iFiled),
                //                     dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                     );
                //                 }
                //                 retObject.Add((ExpandoObject)dataRow);
                //             }
                //         }
                //         data.get_online_month_details = retObject.ToArray();

                //     }
                //     catch (Exception e)
                //     {
                //         Console.WriteLine(e.Message);
                //     }


                // }



                data.fillpaymentgateway = (from a in _portalDashboardContext.PAYUDETAILS
                                           from b in _portalDashboardContext.Fee_PaymentGateway_Details
                                           where (a.IMPG_ActiveFlg == true && b.FPGD_PGActiveFlag ==
                                           "1" && a.IMPG_Id == b.IMPG_Id && b.MI_Id == data.MI_Id)
                                           select new TenantDashboardDTO
                                           {
                                               FPGD_Id = a.IMPG_Id,
                                               FPGD_PGName = a.IMPG_PGFlag,
                                               FPGD_Image = b.FPGD_Image
                                           }).ToArray();


                data.Payment_details = (from a in _portalDashboardContext.TenantPaymentDetailsDMO
                                        from b in _portalDashboardContext.TenantAgreement
                                        where (a.TENANTAGR_Id == b.TENANTAGR_Id && b.MI_Id == data.MI_Id && b.TENANTAGR_Id == data.TENANTAGR_Id && b.TENANTAGR_ActiveFlg == true)
                                        select new TenantDashboardDTO
                                        {
                                            TENANTAGR_AdvanceAmount = b.TENANTAGR_AdvanceAmount,
                                            TENANTAGR_MonthlyRent = b.TENANTAGR_MonthlyRent,
                                            TENANTAGRPAY_Amount = a.TENANTAGRPAY_Amount,
                                            TENANTAGRPAY_PaymentDate = a.TENANTAGRPAY_PaymentDate
                                        }).ToArray();



                using (var cmd = _portalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "tenant_online_month_details_multiple";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id", SqlDbType.VarChar)
                    {
                        Value = data.TENANTAGR_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@waterflag", SqlDbType.VarChar)
                    {
                        Value = data.waterflag
                    });
                    cmd.Parameters.Add(new SqlParameter("@rentflag", SqlDbType.VarChar)
                    {
                        Value = data.rentflag

                    });
                    cmd.Parameters.Add(new SqlParameter("@Electricityflag", SqlDbType.VarChar)
                    {
                        Value = data.Electricityflag
                    });
                    cmd.Parameters.Add(new SqlParameter("@Advanceflag", SqlDbType.VarChar)
                    {
                        Value = data.Advanceflag
                    });
                    cmd.Parameters.Add(new SqlParameter("@Otherflag", SqlDbType.VarChar)
                    {
                        Value = data.Otherflag
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_online_month_details = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }


                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public TenantDashboardDTO onchangetype(TenantDashboardDTO data)
        {
            try
            {
                //t.TENANTAGR_UserId == data.User_Id
                // var tenant_user = _portalDashboardContext.TenantAgreement.Where(t => t.TENANTAGR_UserId == data.User_Id && t.TENANTAGR_ActiveFlg == true).ToList();
                data.get_tenant_details = _portalDashboardContext.TenantAgreement.Where(t => t.TENANTAGR_UserId == data.User_Id && t.TENANTAGR_ActiveFlg == true).ToArray();
                var hrmr_id = _portalDashboardContext.TenantAgreement.Where(t => t.TENANTAGR_UserId == data.User_Id).Select(a => a.HRMR_Id).FirstOrDefault();
                //  data.TENANTAGR_Id= _portalDashboardContext.TenantAgreement.Select(s=>s.TENANTAGR_Id).Where(t => t.TENANTAGR_UserId == data.User_Id && t.TENANTAGR_ActiveFlg == true);
                data.institutiondet = _portalDashboardContext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                //for (var i = 0; i <= data.get_tenant_details.Length; i++)
                //{

                //    data.TENANTAGR_Id = data.get_tenant_details[i].TENANTAGR_Id;
                //}
                data.TENANTAGR_Id = _portalDashboardContext.TenantAgreement.Where(a => a.TENANTAGR_UserId == data.User_Id).Select(a => a.TENANTAGR_Id).FirstOrDefault();



                using (var cmd = _portalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "tenant_online_month_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id", SqlDbType.VarChar)
                    {
                        Value = data.TENANTAGR_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                    {
                        Value = data.type
                    });
                    //cmd.Parameters.Add(new SqlParameter("@waterflag", SqlDbType.VarChar)
                    //{
                    //    Value = data.waterflag
                    //});
                    //cmd.Parameters.Add(new SqlParameter("@rentflag", SqlDbType.VarChar)
                    //{
                    //    Value = data.rentflag

                    //});
                    //cmd.Parameters.Add(new SqlParameter("@Electricityflag", SqlDbType.VarChar)
                    //{
                    //    Value = data.Electricityflag
                    //});
                    //cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                    //{
                    //    Value = data.type
                    //});


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_online_month_details = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }


                }


                var fill_bankdetails = (from a in _portalDashboardContext.HR_Master_BankDeatils
                                        from b in _portalDashboardContext.RoomBankDeatilsDMO
                                        where (a.HRMBD_Id == b.HRMBD_Id && a.MI_Id == data.MI_Id && b.HRMR_Id == hrmr_id)
                                        select new TenantDashboardDTO
                                        {
                                            //FPGD_Id = a.FPGD_Id,
                                            HRMBD_BankName = a.HRMBD_BankName,
                                            HRMBD_BankAccountNo = a.HRMBD_BankAccountNo,
                                            HRMBD_BankAddress = a.HRMBD_BankAddress,
                                            HRMBD_IFSCCode = a.HRMBD_IFSCCode
                                        }).ToList();






                data.fillpaymentgateway = (from a in _portalDashboardContext.PAYUDETAILS
                                           from b in _portalDashboardContext.Fee_PaymentGateway_Details
                                           where (a.IMPG_ActiveFlg == true && b.FPGD_PGActiveFlag ==
                                           "1" && a.IMPG_Id == b.IMPG_Id && b.MI_Id == data.MI_Id && b.FPGD_MobileActiveFlag == true && b.FPGD_Id == fill_bankdetails.FirstOrDefault().FPGD_Id)
                                           select new TenantDashboardDTO
                                           {
                                               FPGD_Id = a.IMPG_Id,
                                               FPGD_PGName = a.IMPG_PGFlag,
                                               FPGD_Image = b.FPGD_Image
                                           }).ToArray();

                data.fill_bankdetails = fill_bankdetails.ToArray();


                data.Payment_details = (from a in _portalDashboardContext.TenantPaymentDetailsDMO
                                        from b in _portalDashboardContext.TenantAgreement
                                        where (a.TENANTAGR_Id == b.TENANTAGR_Id && b.MI_Id == data.MI_Id && b.TENANTAGR_Id == data.TENANTAGR_Id && b.TENANTAGR_ActiveFlg == true)
                                        select new TenantDashboardDTO
                                        {
                                            TENANTAGR_AdvanceAmount = b.TENANTAGR_AdvanceAmount,
                                            TENANTAGR_MonthlyRent = b.TENANTAGR_MonthlyRent,
                                            TENANTAGRPAY_Amount = a.TENANTAGRPAY_Amount,
                                            TENANTAGRPAY_PaymentDate = a.TENANTAGRPAY_PaymentDate
                                        }).ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public TenantDashboardDTO getamountdetails(TenantDashboardDTO data)
        {
            try
            {
                //t.TENANTAGR_UserId == data.User_Id
                var tenant_user = _portalDashboardContext.TenantAgreement.Where(t => t.TENANTAGR_Id == 36 && t.MI_Id == 17 && t.TENANTAGR_ActiveFlg == true).ToArray();
                var tenants_detail = (dynamic)null;
                if (tenant_user.Length > 0)
                {
                    //t.TENANTAGR_UserId == data.User_Id
                    tenants_detail = _portalDashboardContext.TenantAgreement.Where(t => t.TENANTAGR_Id == 36 && t.MI_Id == 17 && t.TENANTAGR_ActiveFlg == true).ToArray();
                    data.tenants_details = tenants_detail;
                }
                DateTime DueDay = new DateTime();
                DateTime DueYear = new DateTime();
                DateTime tdy_date = new DateTime();
                for (var i = 0; i <= tenants_detail.Length; i++)
                {
                    DueDay = tenants_detail[i].TENANTAGR_RentDueDay;
                    DueYear = tenants_detail[i].TENANTAGR_AgrToDate;
                    data.TENANTAGR_Id = tenants_detail[i].TENANTAGR_Id;
                }

                // var last_paid_date= _portalDashboardContext.TenantPaymentDetailsDMO.Where(t => t.TENANTAGR_Id == data.TENANTAGR_Id).ToArray();

                var last_paid_date = _portalDashboardContext.TenantPaymentDetailsDMO.Where(t => t.TENANTAGR_Id == data.TENANTAGR_Id).Select(t => t.TENANTAGRPAY_PaymentDate).LastOrDefault();
                data.Payment_details = (from a in _portalDashboardContext.TenantPaymentDetailsDMO
                                        from b in _portalDashboardContext.TenantAgreement
                                        where (a.TENANTAGR_Id == b.TENANTAGR_Id && b.MI_Id == data.MI_Id && b.TENANTAGR_ActiveFlg == true && b.TENANTAGR_Id == data.TENANTAGR_Id && a.TENANTAGRPAY_PaymentDate >= last_paid_date && a.TENANTAGRPAY_PaymentDate <= tdy_date)
                                        select new TenantDashboardDTO
                                        {
                                            TENANTAGR_AdvanceAmount = b.TENANTAGR_AdvanceAmount,
                                            TENANTAGR_MonthlyRent = b.TENANTAGR_MonthlyRent,
                                            TENANTAGRPAY_Amount = a.TENANTAGRPAY_Amount,
                                            TENANTAGRPAY_PaymentDate = a.TENANTAGRPAY_PaymentDate
                                        }).ToArray();



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public TenantDashboardDTO generatehashsequence(TenantDashboardDTO data)
        {
            try
            {
                //data.ASMAY_Id = getacademicyearcongig(data);
                data.get_tenant_details = _portalDashboardContext.TenantAgreement.Where(t => t.TENANTAGR_UserId == data.User_Id && t.TENANTAGR_ActiveFlg == true).ToArray();


                if (data.onlinepaygteway == "RAZORPAY")
                {
                    data.paydet = paymentPartRAZORPAY(data, data.TENANTAGRPAY_Amount);
                    var dd = _portalDashboardContext.Master_Numbering.Where(s => s.MI_Id == data.MI_Id && s.IMN_Flag == "TenantOnlinePayNo").ToList();
                    // data.transnumconfigsettings = dd.ToList();
                }
                else if (data.onlinepaygteway == "EASEBUZZ")
                {

                    data.paydet = EasebuzzInititatePayment(data, data.TENANTAGRPAY_Amount);
                    var dd = _portalDashboardContext.Master_Numbering.Where(s => s.MI_Id == data.MI_Id && s.IMN_Flag == "TenantOnlinePayNo").ToList();


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public Array paymentPartRAZORPAY(TenantDashboardDTO enq, decimal totamount)
        {
            PaymentDetails PaymentDetailsDto = new PaymentDetails();

            try
            {
                long totconclist = 0;

                var details = _portalDashboardContext.TenantAgreement.Where(t => t.TENANTAGR_UserId == enq.User_Id && t.TENANTAGR_ActiveFlg == true).ToArray();


                List<TenantDashboardDTO> finelst = new List<TenantDashboardDTO>();


                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _portalDashboardContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();
                PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey.ToString();
                enq.FPGD_SubMerchantId = paymentdetails.FirstOrDefault().FPGD_SubMerchantId.ToString();
                PaymentDetailsDto.Seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";


                string orderId;
                Dictionary<String, object> transfersnotes = new Dictionary<String, object>();

                transfersnotes.Add("notes_1", details[0].TENANTAGR_TenantName);
                //transfersnotes.Add("notes_2", details[0].AMST_AdmNo);
                //transfersnotes.Add("notes_3", details[0].Amst_Id);
                transfersnotes.Add("notes_2", details[0].TENANTAGR_ContactNo);
                transfersnotes.Add("notes_3", details[0].TENANTAGR_EmailId);
                transfersnotes.Add("account", enq.FPGD_SubMerchantId);
                transfersnotes.Add("amount", (Convert.ToInt32(enq.TENANTAGRPAY_Amount.ToString()) * 100).ToString());
                transfersnotes.Add("currency", "INR");
                //transfersnotes.Add("notes", transfersnotes);
                Dictionary<string, object> input = new Dictionary<string, object>();
                //input.Add("amount", 1 * 100);
                input.Add("amount", enq.TENANTAGRPAY_Amount * 100); // this amount should be same as transaction amount
                input.Add("currency", "INR");
                //input.Add("receipt", PaymentDetailsDto.trans_id);
                input.Add("payment_capture", 1);
                input.Add("notes", transfersnotes);

                string key = paymentdetails.FirstOrDefault().FPGD_SaltKey;
                string secret = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;

                RazorpayClient client = new RazorpayClient(key, secret);

                Razorpay.Api.Order order = client.Order.Create(input);
                orderId = order["id"].ToString();
                enq.trans_id = orderId;
                enq.TENANTAGR_Id = details.FirstOrDefault().TENANTAGR_Id;
                enq.merchantkey = paymentdetails.FirstOrDefault().FPGD_SaltKey;
                get_grp_reptno(enq);

                if (enq.recenocol != null)
                {
                    Array.Clear(enq.recenocol, 0, enq.recenocol.Length);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return PaymentDetailsDto.PaymentDetailsList;

        }


        public Array EasebuzzInititatePayment(TenantDashboardDTO enq, decimal totamount)
        {
            PaymentDetails PaymentDetailsDto = new PaymentDetails();

            try
            {
                long totconclist = 0;

                var details = _portalDashboardContext.TenantAgreement.Where(t => t.TENANTAGR_UserId == enq.User_Id && t.TENANTAGR_ActiveFlg == true).ToArray();


                List<TenantDashboardDTO> finelst = new List<TenantDashboardDTO>();

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _portalDashboardContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();
                PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey.ToString();
                enq.FPGD_SubMerchantId = paymentdetails.FirstOrDefault().FPGD_SubMerchantId.ToString();


                string surl = "http://localhost:57606/api/TenantOnlinePayment/paymentresponseeasybuzz/";
                string furl = "http://localhost:57606/api/TenantOnlinePayment/paymentresponseeasybuzz/";

                //enq.trans_id = enq.MI_Id + indianTime.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (indianTime.Millisecond)).Trim();
                enq.trans_id = enq.MI_Id + indianTime.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (indianTime.Millisecond)).Trim();

                string Txnid = (enq.trans_id).ToString();
                string UDF1 = (details.FirstOrDefault().TENANTAGR_Id).ToString();

                string UDF2 = (enq.User_Id).ToString();

                string UDF3 = (enq.MI_Id).ToString();

                string UDF4 = (enq.ASMAY_Id).ToString();

                string UDF5 = "";

                string UDF6 = "";

                string UDF7 = "";
                string UDF8 = "";
                string UDF9 = "";
                string UDF10 = "";
                string productinfo = (enq.MI_Id).ToString();
                string Show_payment_mode = "";
                string split_payments = "";
                string sub_merchant_id = "";

                string amount = enq.TENANTAGRPAY_Amount.ToString();
                string firstname = details[0].TENANTAGR_TenantName;
                string email = details[0].TENANTAGR_EmailId;
                string phone = details[0].TENANTAGR_ContactNo;


                string key = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;
                string secret = paymentdetails.FirstOrDefault().FPGD_SaltKey;

                string env = "test";
                Easebuzz t1 = new Easebuzz(secret, key, env);

                string strForm = t1.initiatePaymentAPI(amount, firstname, email, phone, productinfo, surl, furl, Txnid, UDF1, UDF2, UDF3, UDF4, UDF5, UDF6, UDF7, UDF8, UDF9, UDF10, Show_payment_mode, split_payments, sub_merchant_id);




                enq.order_id = enq.trans_id;
                enq.TENANTAGR_Id = details.FirstOrDefault().TENANTAGR_Id;
                enq.merchantkey = paymentdetails.FirstOrDefault().FPGD_SaltKey;
                enq.strForm = strForm;
                get_grp_reptno(enq);

                if (enq.recenocol != null)
                {
                    Array.Clear(enq.recenocol, 0, enq.recenocol.Length);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return PaymentDetailsDto.PaymentDetailsList;

        }




        public TenantDashboardDTO get_grp_reptno(TenantDashboardDTO data)
        {
            try
            {
                var details = _portalDashboardContext.TenantAgreement.Where(t => t.TENANTAGR_UserId == data.User_Id && t.TENANTAGR_ActiveFlg == true).ToArray();
                var dynamicrecgen = new List<dynamic>();

                string recno = "";
                int returnval = 0;

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);




                if (data.Month_detailsarray.Length > 0 && data.Month_detailsarray != null)
                {
                    long hrmr_id = 0;
                    long TENANTMWBILL_Id = 0;
                    long duplicate = 0;

                    hrmr_id = _portalDashboardContext.TenantAgreement.Where(a => a.TENANTAGR_Id == data.TENANTAGR_Id).Select(g => g.HRMR_Id).FirstOrDefault();

                    //var bill_id=_portalDashboardContext.tenan

                    for (int r = 0; r < data.Month_detailsarray.Length; r++)
                    {
                        //    decimal monthly_amt = data.Month_detailsarray[r].TENANTAGR_MonthlyRent;
                        //    long month_id = data.Month_detailsarray[r].MonthId;
                        //    long month_year = data.Month_detailsarray[r].TENANTAGRPAY_PaymentYear;
                        //    var month_name = _portalDashboardContext.IVRM_Month_DMO.Where(s => s.IVRM_Month_Id == month_id).Select(d => d.IVRM_Month_Name).FirstOrDefault();
                        //    if (data.type == "Rent")
                        //    {
                        //        duplicate = _portalDashboardContext.Tenant_Agreement_Online_PaymentDetailsDMO.Where(s => s.TENANTAGRONLPAY_PaymentMonth == month_id && s.TENANTAGRONLPAY_PaymentYear == month_year && s.TENANTAGR_Id == data.TENANTAGR_Id).Count();
                        //    }
                        //    else if (data.type == "WaterBill")
                        //    {
                        //        duplicate = (from a in _portalDashboardContext.Tenant_Monthly_WaterBillsDMO
                        //                     from b in _portalDashboardContext.Tenant_Monthly_WaterBills_RoomWiseDMO
                        //                     where (a.TENANTMWBILL_Id == b.TENANTMWBILL_Id && a.TENANTMWBILL_Year == month_year && b.HRMR_ID == hrmr_id)
                        //                     select new TenantDashboardDTO
                        //                     {
                        //                         TENANTMWBILLRW_Id = b.TENANTMWBILLRW_Id
                        //                     }).Count();


                        //        //_portalDashboardContext.Tenant_Monthly_WaterBillsDMO.Where(s => s. == month_id && s.TENANTAGRONLPAY_PaymentYear == month_year && s.TENANTAGR_Id == data.TENANTAGR_Id).Count();
                        //    }
                        //    if (duplicate == 0)
                        //    {
                        //        data.message = "Already paid for the month " + month_name;
                        //    }
                        //    else
                        //    {



                        //if (hrmr_id > 0)
                        //{
                        //    TENANTMWBILL_Id = (from a in _portalDashboardContext.Tenant_Monthly_WaterBillsDMO
                        //                       from b in _portalDashboardContext.Tenant_Monthly_WaterBills_RoomWiseDMO
                        //                       where (a.TENANTMWBILL_Id == b.TENANTMWBILL_Id && a.TENANTMWBILL_Month == month_id && a.TENANTMWBILL_Year == month_year && b.HRMR_ID == hrmr_id)
                        //                       select new TenantDashboardDTO
                        //                       {
                        //                           TENANTMWBILL_Id = a.TENANTMWBILL_Id
                        //                       }).Select(a => a.TENANTMWBILL_Id).FirstOrDefault();


                        //}
                        //if (data.type == "WaterBill")
                        //{
                        //    try
                        //    {

                        //        var TENANTMWBILLRW_Id = _portalDashboardContext.Tenant_Monthly_WaterBills_RoomWiseDMO.Where(s => s.TENANTMWBILL_Id == TENANTMWBILL_Id).FirstOrDefault();
                        //        //Tenant_Monthly_WaterBills_RoomWiseDMO waterdto = new Tenant_Monthly_WaterBills_RoomWiseDMO();
                        //        TENANTMWBILLRW_Id.TENANTMWBILLRW_AmountPaid = monthly_amt;
                        //        TENANTMWBILLRW_Id.TENANTMWBILL_Id = TENANTMWBILL_Id;
                        //        TENANTMWBILLRW_Id.TENANTMWBILLRW_Remarks = data.TENANTAGRPAY_Remarks;
                        //        TENANTMWBILLRW_Id.TENANTMWBILLRW_PaidFlg = true;
                        //        TENANTMWBILLRW_Id.TENANTMWBILLRW_UpdatedBy = data.User_Id;
                        //        TENANTMWBILLRW_Id.TENANTMWBILLRW_UpdatedDate = DateTime.Now;
                        //        _portalDashboardContext.Tenant_Monthly_WaterBills_RoomWiseDMO.Update(TENANTMWBILLRW_Id);
                        //        returnval = _portalDashboardContext.SaveChanges();
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        Console.WriteLine(ex.Message);
                        //    }
                        //}



                        Tenant_Agreement_Online_PaymentDetailsDMO onlinemtrans = new Tenant_Agreement_Online_PaymentDetailsDMO();

                        onlinemtrans.TENANTAGRONLPAY_TransactionReferenceId = data.trans_id;
                        onlinemtrans.TENANTAGR_Id = data.TENANTAGR_Id;
                        onlinemtrans.TENANTAGRONLPAY_Amount = data.Month_detailsarray[r].TENANTAGR_MonthlyRent;
                        onlinemtrans.TENANTAGRONLPAY_PaymentDate = data.TENANTAGRPAY_PaymentDate;
                        //  onlinemtrans.TENANTAGRONLPAY_PaymentStatus = "P";
                        //                                                                                                                                                                                      onlinemtrans.TENANTAGRONLPAY_PaymentMonth = month_id;
                        onlinemtrans.TENANTAGRONLPAY_PaymentYear = data.Month_detailsarray[r].TENANTAGRPAY_PaymentYear;
                        onlinemtrans.TENANTAGRONLPAY_PaymentMonth = data.Month_detailsarray[r].MonthId;
                        //   onlinemtrans.FMOT_Receipt_no = rece_amt.receiptno;
                        onlinemtrans.TENANTAGRONLPAY_ActiveFlg = true;
                        onlinemtrans.TENANTAGRONLPAY_CreatedDate = indianTime;
                        onlinemtrans.TENANTAGRONLPAY_UpdatedDate = indianTime;
                        onlinemtrans.TENANTAGRONLPAY_CreatedBy = data.User_Id;
                        onlinemtrans.TENANTAGRONLPAY_UpdatedBy = data.User_Id;
                        onlinemtrans.TENANTAGRONLPAY_Flag = data.Month_detailsarray[r].Name;



                        _portalDashboardContext.Tenant_Agreement_Online_PaymentDetailsDMO.Add(onlinemtrans);


                    }

                    // }
                    //if(duplicate==0)
                    //{
                    returnval = _portalDashboardContext.SaveChanges();
                    if (returnval > 0)
                    {
                    }
                    // }

                }



                //if (dynamicrecgen.Count() > 0)
                //          {
                //              data.recenocol = dynamicrecgen.ToArray();
                //          }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }



        public PaymentDetails razorresponse(PaymentDetails response)
        {
            try
            {
                TenantDashboardDTO pgmod = new TenantDashboardDTO();
                PaymentDetails dto = new PaymentDetails();

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                string recno = "";

                //single account added on 17/12/2019

                var accountvalidation = (from a in _portalDashboardContext.Fee_PaymentGateway_Details
                                         where (a.MI_Id == response.IVRMOP_MIID && a.FPGD_PGName == "RAZORPAY")
                                         select new TenantDashboardDTO
                                         {
                                             FPGD_SubMerchantId = a.FPGD_SubMerchantId
                                         }).Distinct().ToArray();

                //single account added on 17/12/2019


                //TRANSFER API

                string url = "https://api.razorpay.com/v1/payments/" + response.razorpay_payment_id + "/transfers";

                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();

                paymentdetails = _portalDashboardContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == response.IVRMOP_MIID && t.FPGD_PGName == "RAZORPAY").Distinct().ToList();

                RazorpayClient client = new RazorpayClient(paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);
                Razorpay.Api.Payment payment = client.Payment.Fetch(response.razorpay_payment_id);
                response.order_id = payment.Attributes["order_id"];


                var fetchfmhotid = (from a in _portalDashboardContext.Tenant_Agreement_Online_PaymentDetailsDMO
                                    where (a.TENANTAGRONLPAY_TransactionReferenceId == response.order_id.ToString() && a.TENANTAGRONLPAY_Amount > 0)
                                    select new TenantDashboardDTO
                                    {
                                        TENANTAGR_Id = a.TENANTAGR_Id,
                                        TENANTAGRONLPAY_Amount = a.TENANTAGRONLPAY_Amount,
                                        TENANTAGRONLPAY_PaymentDate = a.TENANTAGRONLPAY_PaymentDate,
                                        TENANTAGRONLPAY_PaymentMonth = a.TENANTAGRONLPAY_PaymentMonth,
                                        TENANTAGRONLPAY_PaymentYear = a.TENANTAGRONLPAY_PaymentYear,
                                        TENANTAGRONLPAY_Id = a.TENANTAGRONLPAY_Id,
                                        TENANTAGRONLPAY_Flag = a.TENANTAGRONLPAY_Flag



                                    }).ToArray();


                var transnum = _portalDashboardContext.Master_Numbering.Where(s => s.MI_Id == response.IVRMOP_MIID && s.IMN_Flag == "TenantOnlinePayNo").Select(d => d.IMN_AutoManualFlag).ToArray();

                var details = _portalDashboardContext.TenantAgreement.Where(t => t.TENANTAGR_UserId == response.User_Id && t.TENANTAGR_ActiveFlg == true && t.TENANTAGR_Id == fetchfmhotid.FirstOrDefault().TENANTAGR_Id).ToArray();

                var mode_of_payemnt = _portalDashboardContext.IVRM_ModeOfPaymentDMO.Where(m => m.MI_Id == response.IVRMOP_MIID && m.IVRMMOD_ModeOfPayment_Code == "O").Select(s => s.IVRMMOD_Id).FirstOrDefault();

                Dictionary<String, object> transfers = new Dictionary<String, object>();
                Dictionary<String, object> transfersnotes = new Dictionary<String, object>();

                List<TenantDashboardDTO> fetchaccountid = new List<TenantDashboardDTO>();

                for (int r = 0; r < fetchfmhotid.Count(); r++)
                {

                    decimal month_amt = fetchfmhotid[r].TENANTAGRONLPAY_Amount;
                    //  long month_id = data.Month_detailsarray[r].MonthId;
                    transfers.Clear();
                    transfersnotes.Clear();


                    transfersnotes.Add("notes_1", details[0].TENANTAGR_TenantName);
                    transfersnotes.Add("notes_2", details[0].TENANTAGR_ContactNo);
                    transfersnotes.Add("notes_3", details[0].TENANTAGR_EmailId);
                    transfersnotes.Add("notes_4", details[0].TENANTAGR_Address);
                    transfersnotes.Add("notes_5", details[0].TENANTAGR_Id);

                    transfers.Add("account", (paymentdetails.FirstOrDefault().FPGD_SubMerchantId));
                    transfers.Add("amount", (Convert.ToInt32(fetchfmhotid[r].TENANTAGRONLPAY_Amount.ToString()) * 100).ToString());
                    transfers.Add("currency", "INR");
                    transfers.Add("notes", transfersnotes);


                    if (accountvalidation.Count() >= 1)
                    {

                        Razorpay.Api.Transfer payment1 = client.Transfer.Create(transfers);

                        PreadmissionDTOs.transferAPI trapay = new PreadmissionDTOs.transferAPI();

                        if (payment1.Attributes["id"] != "")
                        {
                            trapay.transfer_id = payment1.Attributes["id"];
                            trapay.entity = payment1.Attributes["entity"];
                            trapay.source = payment1.Attributes["source"];
                            trapay.recipient = payment1.Attributes["recipient"];
                            trapay.amount = payment1.Attributes["amount"];

                            if (response.AutoManualFlag == "Manual")
                            {
                                recno = response.TENANTAGRPAY_ReceiptNo;
                            }
                            else
                            {
                                var numbering_detail = _portalDashboardContext.Master_Numbering.Where(d => d.MI_Id == response.IVRMOP_MIID && d.IMN_Flag == "TenantOnlinePayNo").ToArray();
                                Master_NumberingDTO dd = new Master_NumberingDTO();
                                GenerateTransactionNumbering a = new GenerateTransactionNumbering(_portalDashboardContext);
                                dd.MI_Id = response.IVRMOP_MIID;
                                dd.ASMAY_Id = response.ASMAY_Id;
                                //transnumconfigsettings.IMN_AutoManualFlag = auto_maual_flag;
                                dd.IMN_Flag = numbering_detail.FirstOrDefault().IMN_Flag;
                                dd.IMN_PrefixAcadYearCode = numbering_detail.FirstOrDefault().IMN_PrefixAcadYearCode;
                                dd.IMN_PrefixCalYearCode = numbering_detail.FirstOrDefault().IMN_PrefixCalYearCode;
                                dd.IMN_PrefixFinYearCode = numbering_detail.FirstOrDefault().IMN_PrefixFinYearCode;
                                dd.IMN_PrefixParticular = numbering_detail.FirstOrDefault().IMN_PrefixParticular;
                                dd.IMN_RestartNumFlag = numbering_detail.FirstOrDefault().IMN_RestartNumFlag;
                                dd.IMN_StartingNo = numbering_detail.FirstOrDefault().IMN_StartingNo;
                                dd.IMN_SuffixAcadYearCode = numbering_detail.FirstOrDefault().IMN_SuffixAcadYearCode;
                                dd.IMN_SuffixCalYearCode = numbering_detail.FirstOrDefault().IMN_SuffixCalYearCode;
                                dd.IMN_SuffixFinYearCode = numbering_detail.FirstOrDefault().IMN_SuffixFinYearCode;
                                dd.IMN_SuffixParticular = numbering_detail.FirstOrDefault().IMN_SuffixParticular;
                                dd.IMN_WidthNumeric = numbering_detail.FirstOrDefault().IMN_WidthNumeric;
                                dd.IMN_ZeroPrefixFlag = numbering_detail.FirstOrDefault().IMN_ZeroPrefixFlag;
                                dd.IMN_AutoManualFlag = numbering_detail.FirstOrDefault().IMN_AutoManualFlag;
                                dd.IMN_DuplicatesFlag = numbering_detail.FirstOrDefault().IMN_DuplicatesFlag;

                                recno = a.GenerateNumberTenant(dd);
                            }



                            
                            TenantPaymentDetailsDMO fet = new TenantPaymentDetailsDMO();

                            fet.TENANTAGRPAY_PaymentMonth = fetchfmhotid[r].TENANTAGRONLPAY_PaymentMonth;
                            fet.TENANTAGRPAY_PaymentYear = fetchfmhotid[r].TENANTAGRONLPAY_PaymentYear;
                            fet.TENANTAGRPAY_ReceiptNo = recno;
                            fet.TENANTAGRPAY_ModeOfPayment = 'O';
                            fet.TENANTAGRPAY_Amount = Convert.ToInt32(fetchfmhotid[r].TENANTAGRONLPAY_Amount);
                            fet.TENANTAGRPAY_CreatedDate = DateTime.Now;
                            fet.TENANTAGRPAY_UpdatedDate = DateTime.Now;
                            fet.TENANTAGRPAY_PaymentReferenceId = response.razorpay_payment_id;
                            fet.TENANTAGRONLPAY_Id = Convert.ToInt32(fetchfmhotid[0].TENANTAGRONLPAY_Id);
                            fet.TENANTAGR_Id = Convert.ToInt32(fetchfmhotid[0].TENANTAGR_Id);
                            fet.TENANTAGRPAY_PaymentDate = DateTime.Now;
                          
                            fet.TENANTAGRPAY_Chequedate = DateTime.Now;
                            fet.TENANTAGRPAY_Bankname = response.TENANTAGRPAY_Bankname;
                            fet.TENANTAGRPAY_Chequeno = response.TENANTAGRPAY_Chequeno;
                            fet.TENANTAGRPAY_CreatedBy = response.User_Id;
                            fet.TENANTAGRPAY_UpdatedBy = response.User_Id;
                            fet.TENANTAGRPAY_Remarks = response.TENANTAGRONLPAY_Remarks;
                            fet.TENANTAGRPAY_Flag = fetchfmhotid[r].TENANTAGRONLPAY_Flag.ToString();
                  
                            _portalDashboardContext.Add(fet);
                            var contactExists = _portalDashboardContext.SaveChanges();
                            if (contactExists == 1)
                            {
                                response.status = "success";
                            }
                            else
                            {
                                response.status = "Failure";
                            }
                        }
                  //  }

                }

                if (response.status == "success")
                {
                    //response.returnval = "success";
                    //var groups = (from a in _FeeGroupHeadContext.FeePaymentDetailsDMO
                    //              where (a.MI_Id == Convert.ToInt32(fetchfmhotid[0].MI_Id) && a.ASMAY_ID == Convert.ToInt32(fetchfmhotid[0].ASMAY_Id) && a.fyp_transaction_id == response.order_id)
                    //              select new FeeStudentTransactionDTO
                    //              {
                    //                  FYP_Receipt_No = a.FYP_Receipt_No
                    //              }
                    //      ).Distinct().ToList();

                    //if (groups.Count == 0)
                    //{
                    //    var confirmstatus = insertdatainfeetables(fetchfmhotid[0].MI_Id.ToString(), "0", fetchfmhotid[0].Amst_Id.ToString(), "0", fetchfmhotid[0].FMA_Amount, response.order_id, response.razorpay_payment_id, fetchfmhotid[0].ASMAY_Id.ToString(), indianTime, "0");

                    //    if (Convert.ToInt32(confirmstatus) > 0)
                    //    {
                    //        SMS sms = new SMS(_context);

                    //        sms.sendSms(Convert.ToInt32(fetchfmhotid[0].MI_Id), Convert.ToInt64(fetchstudentdeatils[0].amst_mobile), "FEEONLINEPAYMENT", Convert.ToInt32(fetchfmhotid[0].Amst_Id));

                    //        Email Email = new Email(_context);

                    //        Email.sendmail(Convert.ToInt32(fetchfmhotid[0].MI_Id), fetchstudentdeatils[0].amst_email_id, "FEEONLINEPAYMENT", Convert.ToInt32(fetchfmhotid[0].Amst_Id));
                    //    }
                    //}
                }
                //else
                //{
                //    SMS sms = new SMS(_context);

                //    sms.sendSms(Convert.ToInt32(fetchfmhotid[0].MI_Id), Convert.ToInt64(fetchstudentdeatils[0].amst_mobile), "FEEONLINEPAYMENTFAIL", Convert.ToInt32(fetchfmhotid[0].Amst_Id));

                //    Email Email = new Email(_context);

                //    Email.sendmail(Convert.ToInt32(fetchfmhotid[0].MI_Id), fetchstudentdeatils[0].amst_email_id, "FEEONLINEPAYMENTFAIL", Convert.ToInt32(fetchfmhotid[0].Amst_Id));

                //    dto.status = response.status;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public PaymentDetails.easybuzz paymentresponseeasybuzz(PaymentDetails.easybuzz response)
        {
            try
            {
                TenantDashboardDTO pgmod = new TenantDashboardDTO();
                PaymentDetails dto = new PaymentDetails();

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                string recno = "";

                if (response.status == "success")
                {


                    List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();

                    paymentdetails = _portalDashboardContext.Fee_PaymentGateway_Details.Where(q => q.MI_Id == Convert.ToInt64(response.UDF3) && q.FPGD_PGName == "EASEBUZZ").Distinct().ToList();

                    //RazorpayClient client = new RazorpayClient(paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);
                    //Razorpay.Api.Payment payment = client.Payment.Fetch(response.razorpay_payment_id);
                    //response.order_id = payment.Attributes["order_id"];








                    string txnid = response.txnid.ToString();
                    string key = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;
                    string secret = paymentdetails.FirstOrDefault().FPGD_SaltKey;

                    string env = "test";
                    string paymentref = "";
                    Easebuzz t = new Easebuzz(secret, key, env);

                    //var res = t.transactionAPI(txnid, response.amount, response.email, response.phone);

                    //JObject joResponse1 = JObject.Parse(res);

                    //paymentref = joResponse1["msg"]["easepayid"].ToString();



                    var fetchfmhotid = (from z in _portalDashboardContext.Tenant_Agreement_Online_PaymentDetailsDMO
                                        where (z.TENANTAGRONLPAY_TransactionReferenceId == response.txnid.ToString() && z.TENANTAGRONLPAY_Amount > 0)
                                        select new TenantDashboardDTO
                                        {
                                            TENANTAGR_Id = z.TENANTAGR_Id,
                                            TENANTAGRONLPAY_Amount = z.TENANTAGRONLPAY_Amount,
                                            TENANTAGRONLPAY_PaymentDate = z.TENANTAGRONLPAY_PaymentDate,
                                            TENANTAGRONLPAY_PaymentMonth = z.TENANTAGRONLPAY_PaymentMonth,
                                            TENANTAGRONLPAY_PaymentYear = z.TENANTAGRONLPAY_PaymentYear,
                                            TENANTAGRONLPAY_Id = z.TENANTAGRONLPAY_Id,
                                            TENANTAGRONLPAY_Flag = z.TENANTAGRONLPAY_Flag



                                        }).ToList();


                    var transnum = _portalDashboardContext.Master_Numbering.Where(s => s.MI_Id == Convert.ToInt64(response.UDF3) && s.IMN_Flag == "TenantOnlinePayNo").Select(d => d.IMN_AutoManualFlag).ToArray();

                    var details = _portalDashboardContext.TenantAgreement.Where(w => w.TENANTAGR_UserId == Convert.ToInt64(response.UDF2) && w.TENANTAGR_ActiveFlg == true && w.TENANTAGR_Id == fetchfmhotid.FirstOrDefault().TENANTAGR_Id).ToArray();

                    //  var mode_of_payemnt = _portalDashboardContext.IVRM_ModeOfPaymentDMO.Where(m => m.MI_Id == Convert.ToInt64(response.UDF3) && m.IVRMMOD_ModeOfPayment_Code == "O").Select(s => s.IVRMMOD_Id).FirstOrDefault();

                    Dictionary<String, object> transfers = new Dictionary<String, object>();
                    Dictionary<String, object> transfersnotes = new Dictionary<String, object>();

                    List<TenantDashboardDTO> fetchaccountid = new List<TenantDashboardDTO>();
                    var numbering_detail = _portalDashboardContext.Master_Numbering.Where(d => d.MI_Id == Convert.ToInt64(response.UDF3) && d.IMN_Flag == "TenantOnlinePayNo").ToArray();
                    Master_NumberingDTO dd = new Master_NumberingDTO();
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_portalDashboardContext);
                    dd.MI_Id = Convert.ToInt64(response.UDF3);
                    dd.ASMAY_Id = Convert.ToInt64(response.UDF4);
                    //transnumconfigsettings.IMN_AutoManualFlag = auto_maual_flag;
                    dd.IMN_Flag = numbering_detail.FirstOrDefault().IMN_Flag;
                    dd.IMN_PrefixAcadYearCode = numbering_detail.FirstOrDefault().IMN_PrefixAcadYearCode;
                    dd.IMN_PrefixCalYearCode = numbering_detail.FirstOrDefault().IMN_PrefixCalYearCode;
                    dd.IMN_PrefixFinYearCode = numbering_detail.FirstOrDefault().IMN_PrefixFinYearCode;
                    dd.IMN_PrefixParticular = numbering_detail.FirstOrDefault().IMN_PrefixParticular;
                    dd.IMN_RestartNumFlag = numbering_detail.FirstOrDefault().IMN_RestartNumFlag;
                    dd.IMN_StartingNo = numbering_detail.FirstOrDefault().IMN_StartingNo;
                    dd.IMN_SuffixAcadYearCode = numbering_detail.FirstOrDefault().IMN_SuffixAcadYearCode;
                    dd.IMN_SuffixCalYearCode = numbering_detail.FirstOrDefault().IMN_SuffixCalYearCode;
                    dd.IMN_SuffixFinYearCode = numbering_detail.FirstOrDefault().IMN_SuffixFinYearCode;
                    dd.IMN_SuffixParticular = numbering_detail.FirstOrDefault().IMN_SuffixParticular;
                    dd.IMN_WidthNumeric = numbering_detail.FirstOrDefault().IMN_WidthNumeric;
                    dd.IMN_ZeroPrefixFlag = numbering_detail.FirstOrDefault().IMN_ZeroPrefixFlag;
                    dd.IMN_AutoManualFlag = numbering_detail.FirstOrDefault().IMN_AutoManualFlag;
                    dd.IMN_DuplicatesFlag = numbering_detail.FirstOrDefault().IMN_DuplicatesFlag;

                    recno = a.GenerateNumberTenant(dd);


                    for (int r = 0; r < fetchfmhotid.Count(); r++)
                    {








                        TenantPaymentDetailsDMO fet = new TenantPaymentDetailsDMO();

                        fet.TENANTAGRPAY_PaymentMonth = fetchfmhotid[r].TENANTAGRONLPAY_PaymentMonth;
                        fet.TENANTAGRPAY_PaymentYear = fetchfmhotid[r].TENANTAGRONLPAY_PaymentYear;
                        fet.TENANTAGRPAY_ReceiptNo = recno;
                        fet.TENANTAGRPAY_ModeOfPayment = 'O';
                        fet.TENANTAGRPAY_Amount = Convert.ToDecimal(response.amount);
                        fet.TENANTAGRPAY_CreatedDate = DateTime.Now;
                        fet.TENANTAGRPAY_UpdatedDate = DateTime.Now;
                        fet.TENANTAGRPAY_PaymentReferenceId = paymentref;
                        fet.TENANTAGRONLPAY_Id = Convert.ToInt64(fetchfmhotid[r].TENANTAGRONLPAY_Id);
                        fet.TENANTAGR_Id = Convert.ToInt64(fetchfmhotid[r].TENANTAGR_Id);
                        fet.TENANTAGRPAY_PaymentDate = DateTime.Now;

                        fet.TENANTAGRPAY_Chequedate = DateTime.Now;
                        fet.TENANTAGRPAY_Bankname = "";
                        fet.TENANTAGRPAY_Chequeno = "";
                        fet.TENANTAGRPAY_CreatedBy = Convert.ToInt64(response.UDF2);
                        fet.TENANTAGRPAY_UpdatedBy = Convert.ToInt64(response.UDF2);
                        fet.TENANTAGRPAY_Remarks = "";
                        fet.TENANTAGRPAY_Flag = fetchfmhotid[r].TENANTAGRONLPAY_Flag.ToString();
                        //fet.TENANTAGRONLPAY_Flag = response.Pay_type;


                        _portalDashboardContext.Add(fet);
                    }
                    var contactExists = _portalDashboardContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        response.status = "success";
                    }
                    else
                    {
                        response.status = "Failure";
                    }
                }




                else
                {
                    response.status = "Failure";
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public string Easebuzz_Generatehash512(string text)
        {

            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;

        }


        public TenantDashboardDTO getamtwaterdetails(TenantDashboardDTO data)
        {
            try
            {

                var tenant_user = _portalDashboardContext.TenantAgreement.Where(t => t.TENANTAGR_Id == data.TENANTAGR_Id && t.MI_Id == data.MI_Id && t.TENANTAGR_ActiveFlg == true).ToArray();
                var tenants_detail = (dynamic)null;
                //if (tenant_user.Length > 0)
                //{
                //    tenants_detail = _portalDashboardContext.TenantAgreement.Where(t => t.TENANTAGR_Id == data.TENANTAGR_Id && t.MI_Id == data.MI_Id && t.TENANTAGR_ActiveFlg == true).ToArray();
                //    data.tenants_details = tenants_detail;
                //}
                //DateTime DueDay = new DateTime();
                //DateTime DueYear = new DateTime();
                //DateTime tdy_date = new DateTime();
                //for (var i = 0; i <= tenants_detail.Length; i++)
                //{
                //    DueDay = tenants_detail[i].TENANTAGR_RentDueDay;
                //    DueYear = tenants_detail[i].TENANTAGR_AgrToDate;
                //    data.TENANTAGR_Id = tenants_detail[i].TENANTAGR_Id;
                //}



                //var last_paid_date = _portalDashboardContext.TenantPaymentDetailsDMO.Where(t => t.TENANTAGR_Id == data.TENANTAGR_Id).Select(t => t.TENANTAGRPAY_PaymentDate).LastOrDefault();
                //data.Payment_details = (from a in _portalDashboardContext.TenantPaymentDetailsDMO
                //                        from b in _portalDashboardContext.TenantAgreement
                //                        where (a.TENANTAGR_Id == b.TENANTAGR_Id && b.MI_Id == data.MI_Id && b.TENANTAGR_ActiveFlg == true && b.TENANTAGR_Id == data.TENANTAGR_Id && a.TENANTAGRPAY_PaymentDate >= last_paid_date && a.TENANTAGRPAY_PaymentDate <= tdy_date)
                //                        select new TenantDashboardDTO
                //                        {
                //                            TENANTAGR_AdvanceAmount = b.TENANTAGR_AdvanceAmount,
                //                            TENANTAGR_MonthlyRent = b.TENANTAGR_MonthlyRent,
                //                            TENANTAGRPAY_Amount = a.TENANTAGRPAY_Amount,
                //                            TENANTAGRPAY_PaymentDate = a.TENANTAGRPAY_PaymentDate
                //                        }).ToArray();



                using (var cmd = _portalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "tenant_online_monthamount_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id", SqlDbType.VarChar)
                    {
                        Value = data.TENANTAGR_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                    {
                        Value = data.type
                    });



                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_online_month_details = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
