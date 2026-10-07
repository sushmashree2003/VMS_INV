using AutoMapper;
using DataAccessMsSqlServerProvider;

using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using CommonLibrary;
using DataAccessMsSqlServerProvider.com.vapstech.SpaceBooking;
using PreadmissionDTOs.com.vaps.SpaceBooking;
using DomainModel.Model.com.vapstech.SpaceBooking;
using DomainModel.Model.com.vapstech.Fee;
using SendGrid.Helpers.Mail;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Dynamic;
using DomainModel.Model;
using System.Net;
using SendGrid;
using System.Net.Mail;
using System.IO;
using DomainModel.Model.com.vaps.Fee;
using Razorpay.Api;

namespace SpaceBooking.Services
{
    public class MasterSpaceImpl : Interfaces.MasterSpaceInterface
    {
        public SpaceBookingContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;

        public MasterSpaceImpl(SpaceBookingContext VMSContext, DomainModelMsSqlServerContext OrganisationContext)
        {
            _VMSContext = VMSContext;
            _Context = OrganisationContext;
        }

        public OS_EnquiryDTO getdata(int id)
        {
            OS_EnquiryDTO data = new OS_EnquiryDTO();
            try
            {
                data.enqlist = _VMSContext.OS_EnquiryDMO.Where(a => a.OSENQ_ActiveFlag == true && a.MI_Id == data.MI_Id).Distinct().OrderBy(a => a.OSENQ_CreatedDate).ToArray();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public OS_EnquiryDTO saveEnquiry(OS_EnquiryDTO data)
        {
            try
            {
                if (data.OSENQ_Id > 0)
                {

                    var update = _VMSContext.OS_EnquiryDMO.Single(r => r.OSENQ_Id == data.OSENQ_Id);

                    update.OSUREG_Id = data.OSUREG_Id;
                    update.OSENQ_Name = data.OSENQ_Name;
                    update.OSENQ_EmailId = data.OSENQ_EmailId;
                    update.OSENQ_MobileNo = data.OSENQ_MobileNo;


                    update.OSENQ_Type = data.OSENQ_Type;
                    if (data.OSENQ_Type == "V")
                    {
                        update.OSENQ_VisitDate = data.OSENQ_VisitDate;
                        update.OSENQ_VisitTime = data.OSENQ_VisitTime;
                    }
                    update.OSENQ_Enquiry = data.OSENQ_Enquiry;
                    update.OSENQ_Status = data.OSENQ_Status;
                    update.OSENQ_ActiveFlag = data.OSENQ_ActiveFlag;
                    update.OSENQ_UpdatedDate = DateTime.Now;
                    update.OSENQ_UpdatedBy = data.User_Id;
                    _VMSContext.Update(update);
                }
                else
                {
                    OS_EnquiryDMO obj = new OS_EnquiryDMO();
                    obj.MI_Id = data.MI_Id;
                    obj.OSUREG_Id = data.OSUREG_Id;
                    obj.OSENQ_Name = data.OSENQ_Name;
                    obj.OSENQ_EmailId = data.OSENQ_EmailId;
                    obj.OSENQ_MobileNo = data.OSENQ_MobileNo;


                    obj.OSENQ_Type = data.OSENQ_Type;
                    if (data.OSENQ_Type == "V")
                    {
                        obj.OSENQ_VisitDate = data.OSENQ_VisitDate;
                        obj.OSENQ_VisitTime = data.OSENQ_VisitTime;
                    }
                    obj.OSENQ_Enquiry = data.OSENQ_Enquiry;
                    obj.OSENQ_Status = data.OSENQ_Status;
                    obj.OSENQ_ActiveFlag = data.OSENQ_ActiveFlag;
                    obj.OSENQ_CreatedDate = DateTime.Now;
                    obj.OSENQ_UpdatedDate = DateTime.Now;
                    obj.OSENQ_CreatedBy = data.User_Id;
                    obj.OSENQ_UpdatedBy = data.User_Id;
                    _VMSContext.Add(obj);

                }


                int n = _VMSContext.SaveChanges();
                if (n > 0)
                {
                    data.retrval = true;
                }
                var Template2 ="SpaceBooking_Enquire";
                onlineSpace_booking_enquiry_sendmail(data.OSUREG_Id, data.MI_Id, Template2);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }
        public OS_EnquiryDTO savedata(OS_EnquiryDTO data)
        {
            //try
            //{
            //    if (data.TRMA_Id > 0)
            //    {
            //        var check_duplicate_areaname_update = _context.MasterAreaDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMA_AreaName.Equals(data.TRMA_AreaName) && a.TRMA_Id != data.TRMA_Id).ToList();
            //        var check_duplicate_areaaliasname_update = _context.MasterAreaDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMA_AliasName.Equals(data.TRMA_AliasName) && a.TRMA_Id != data.TRMA_Id).ToList();

            //        if (check_duplicate_areaname_update.Count == 0 && check_duplicate_areaaliasname_update.Count == 0)
            //        {
            //            var result = _context.MasterAreaDMO.Single(a => a.MI_Id == data.MI_Id && a.TRMA_Id == data.TRMA_Id);
            //            result.TRMA_AreaName = data.TRMA_AreaName;
            //            result.TRMA_AliasName = data.TRMA_AliasName;
            //            result.CreatedDate = result.CreatedDate;
            //            result.UpdatedDate = DateTime.Now;
            //            _context.Update(result);
            //            int n = _context.SaveChanges();
            //            if (n > 0)
            //            {
            //                data.message = "Update";
            //                data.retrval = true;
            //            }
            //            else
            //            {
            //                data.message = "Update";
            //                data.retrval = false;
            //            }
            //        }
            //        else
            //        {
            //            data.message = "Duplicate";
            //        }
            //    }
            //    else
            //    {
            //        var check_duplicate_areaname = _context.MasterAreaDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMA_AreaName.Equals(data.TRMA_AreaName)).ToList();
            //        var check_duplicate_areaaliasname = _context.MasterAreaDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMA_AliasName.Equals(data.TRMA_AliasName)).ToList();

            //        if (check_duplicate_areaaliasname.Count == 0 && check_duplicate_areaname.Count == 0)
            //        {
            //            MasterAreaDMO areadmo = new MasterAreaDMO();
            //            areadmo.TRMA_AreaName = data.TRMA_AreaName;
            //            areadmo.TRMA_AliasName = data.TRMA_AliasName;
            //            areadmo.MI_Id = data.MI_Id;
            //            areadmo.TRMA_ActiveFlg = true;
            //            areadmo.CreatedDate = DateTime.Now;
            //            areadmo.UpdatedDate = DateTime.Now;

            //            _context.Add(areadmo);
            //            int n = _context.SaveChanges();
            //            if (n > 0)
            //            {
            //                data.message = "Add";
            //                data.retrval = true;
            //            }
            //            else
            //            {
            //                data.message = "Update";
            //                data.retrval = true;
            //            }


            //        }
            //        else
            //        {
            //            data.message = "Duplicate";
            //        }


            //    }
            //}
            //catch (Exception ex)
            //{
            //    _areaimpl.LogInformation("Transport Error Master Area savedata" + ex.Message);
            //}
            return data;
        }

        public OS_EnquiryDTO geteditdata(OS_EnquiryDTO data)
        {
            //try
            //{
            //    data.geteditdataarea = _context.MasterAreaDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMA_Id == data.TRMA_Id).ToArray();
            //}
            //catch (Exception ex)
            //{
            //    _areaimpl.LogInformation("Transport Error Master Area geteditdata " + ex.Message);
            //}
            return data;
        }

        public OS_EnquiryDTO activedeactive(OS_EnquiryDTO data)
        {
            //try
            //{
            //    var check_Area_Used = (from a in _context.MasterRouteDMO
            //                           from b in _context.MasterAreaDMO
            //                           where (a.TRMA_Id == b.TRMA_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.TRMA_ActiveFlg == true && a.TRMA_Id == data.TRMA_Id && a.TRMR_ActiveFlg == true)
            //                           select new OS_EnquiryDTO
            //                           {
            //                               TRMA_Id = a.TRMA_Id
            //                           }).ToList();


            //    if (check_Area_Used.Count > 0)
            //    {
            //        data.message = "You Can Not Deactivate This Record Its Already Mapped";
            //    }
            //    else
            //    {
            //        var result = _context.MasterAreaDMO.Single(a => a.MI_Id == data.MI_Id && a.TRMA_Id == data.TRMA_Id);
            //        if (result.TRMA_ActiveFlg == false)
            //        {
            //            result.TRMA_ActiveFlg = true;
            //        }
            //        else
            //        {
            //            result.TRMA_ActiveFlg = false;
            //        }
            //        result.UpdatedDate = DateTime.Now;
            //        _context.Update(result);
            //        int n = _context.SaveChanges();
            //        if (n > 0)
            //        {
            //            data.retrval = true;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    data.message = "You Can Not Deactivate This Record Its Already Mapped";
            //    _areaimpl.LogInformation("Transport Error Master Area activedeactive" + ex.Message);
            //}
            return data;
        }

        public OS_EnquiryDTO loadcountry(OS_EnquiryDTO data)
        {
            try
            {
                data.countrylist = _VMSContext.IVRM_Master_Country.Distinct().OrderBy(f => f.IVRMMC_CountryName).ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }


        public OS_EnquiryDTO get_state(OS_EnquiryDTO dto)
        {
            try
            {
                dto.statelist = _VMSContext.IVRM_Master_State.Where(a => a.IVRMMC_Id == dto.IVRMMC_Id).Distinct().OrderBy(f => f.IVRMMS_Name).ToArray(); ;

            }
            catch (Exception ex)
            {

                throw ex;
            }


            return dto;
        }

        public OS_Space_BookingDTO SaveBooking(OS_Space_BookingDTO spaceBooking)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            Int64 s2 = Convert.ToInt64(DateTime.Now.ToString("ddMMyyyyHHmmss"));


            try
            {
                var Transcount = _VMSContext.OS_Space_Booking.Where(t => t.OSSPBOOK_TransactionNo == s2.ToString()).ToList();
                if (Transcount.Count() == 0)
                {
                    OS_Space_Booking SpaceBook = new OS_Space_Booking();

                    SpaceBook.MI_Id = spaceBooking.MI_Id;
                    SpaceBook.OSUREG_Id = spaceBooking.OSUREG_Id;
                    SpaceBook.HRMR_Id = spaceBooking.HRMR_Id;
                    spaceBooking.OSSPBOOK_TransactionNo = s2.ToString();
                    SpaceBook.OSSPBOOK_ActiveFlag = true;
                    SpaceBook.OSSPBOOK_BillingAdd1 = spaceBooking.OSSPBOOK_BillingAdd1;
                    SpaceBook.OSSPBOOK_BillingAdd2 = spaceBooking.OSSPBOOK_BillingAdd2;
                    SpaceBook.OSSPBOOK_BillingAdd3 = spaceBooking.OSSPBOOK_BillingAdd3;
                    SpaceBook.OSSPBOOK_BillingAdd4 = spaceBooking.OSSPBOOK_BillingAdd4;
                    SpaceBook.OSSPBOOK_BillingCity = spaceBooking.OSSPBOOK_BillingCity;
                    SpaceBook.OSSPBOOK_BillingName = spaceBooking.OSSPBOOK_BillingName;
                    SpaceBook.OSSPBOOK_BillingPinCode = spaceBooking.OSSPBOOK_BillingPinCode;
                    SpaceBook.OSSPBOOK_DurationType = spaceBooking.OSSPBOOK_DurationType;
                    SpaceBook.OSSPBOOK_StartDate = spaceBooking.OSSPBOOK_StartDate;
                    SpaceBook.OSSPBOOK_StartTime = spaceBooking.OSSPBOOK_StartTime;
                    SpaceBook.OSSPBOOK_EndDate = spaceBooking.OSSPBOOK_EndDate;
                    SpaceBook.OSSPBOOK_EndTime = spaceBooking.OSSPBOOK_EndTime;
                    SpaceBook.OSSPBOOK_GSTNo = spaceBooking.OSSPBOOK_GSTNo;
                    SpaceBook.OSSPBOOK_NoOfPeople = spaceBooking.OSSPBOOK_NoOfPeople;
                    SpaceBook.OSSPBOOK_Status = "INPROGRESS";
                    SpaceBook.IVRMMS_Id = spaceBooking.IVRMMS_Id;
                    SpaceBook.IVRMMC_Id = spaceBooking.IVRMMC_Id;
                    SpaceBook.OSSPBOOK_CreatedBy = spaceBooking.OSUREG_Id;
                    SpaceBook.OSSPBOOK_CreatedDate = indianTime;
                    SpaceBook.OSSPBOOK_UpdatedBy = spaceBooking.OSUREG_Id;
                    SpaceBook.OSSPBOOK_UpdatedDate = indianTime;
                    _VMSContext.Add(SpaceBook);
                    var checksave = _VMSContext.SaveChanges();
                    if (checksave > 0)
                    {
                        var hrmfid = (from a in _VMSContext.OS_Space_Booking
                                      from b in _VMSContext.HR_Master_Room
                                      where (a.HRMR_Id == b.HRMR_Id && a.HRMR_Id == spaceBooking.HRMR_Id && a.OSSPBOOK_Id == SpaceBook.OSSPBOOK_Id)
                                      select new OS_Space_BookingDTO
                                      {
                                          HRMF_Id = b.HRMF_Id
                                      }).ToList();
                        spaceBooking.OSSPBOOK_Id = SpaceBook.OSSPBOOK_Id;
                        if (hrmfid.Count > 0)
                        {
                            spaceBooking.HRMF_Id = hrmfid.FirstOrDefault().HRMF_Id;
                        }

                        var checktypelist = (from a in _VMSContext.Office_Space_User_Registration
                                             from b in _VMSContext.OS_User_Registration_InternalDMO
                                             where a.OSUREG_ID == b.OSUREG_Id && a.OSUREG_ID == spaceBooking.OSUREG_Id
                                             select a).ToList();
                        if (checktypelist.Count == 0)
                        {


                            spaceBooking.response = "E";
                        }
                        else
                        {
                            var bookdetails = _VMSContext.OS_Space_Booking.Single(e => e.OSSPBOOK_Id == SpaceBook.OSSPBOOK_Id);
                            bookdetails.OSSPBOOK_Status = "CONFIRM";
                            _VMSContext.Update(bookdetails);
                            _VMSContext.SaveChanges();
                            spaceBooking.response = "I";
                            var emailidlist = _VMSContext.Office_Space_User_Registration.Where(e => e.OSUREG_ID == spaceBooking.OSUREG_Id).ToList();
                            if (emailidlist.Count > 0)
                            {
                                if (emailidlist[0].OSUREG_EmailId != "")
                                {
                                    string s = sendmail_space(spaceBooking.MI_Id, emailidlist[0].OSUREG_EmailId, "SPACEBOOKING", spaceBooking.OSUREG_Id, SpaceBook.OSSPBOOK_Id, spaceBooking.HRMR_Id);
                                }

                            }



                        }
                        var bookid = _VMSContext.OS_Space_Booking.Where(a => a.MI_Id == spaceBooking.MI_Id && a.HRMR_Id == spaceBooking.HRMR_Id && a.OSUREG_Id == spaceBooking.OSUREG_Id).LastOrDefault();

                        var Template = "SpaceBooking_conform_I";
                        var getmail = _VMSContext.Office_Space_User_Registration.Single(a => a.OSUREG_ID == spaceBooking.OSUREG_Id).OSUREG_EmailId;
                        onlineSpace_booking_conformation_sendmail(getmail, spaceBooking.MI_Id, spaceBooking.OSUREG_Id, spaceBooking.HRMR_Id, Template, bookid.OSSPBOOK_Id);




                    }
                }
                else
                {
                    s2 = s2 + 1;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return spaceBooking;
        }

        public OS_Space_BookingDTO Availability(OS_Space_BookingDTO spaceBooking)
        {
            try
            {
                var CheckAvailability = _VMSContext.OS_Space_Booking.Where(t => t.HRMR_Id == spaceBooking.HRMR_Id && t.OSSPBOOK_StartDate == spaceBooking.OSSPBOOK_StartDate && t.OSSPBOOK_StartTime == spaceBooking.OSSPBOOK_StartTime).ToList();
                if (CheckAvailability.Count() == 0)
                {
                    spaceBooking.response = "Available";
                }
                else
                {
                    spaceBooking.response = "NotAvailable";
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return spaceBooking;
        }

        public OS_Space_BookingDTO OnlinePaymentDetails(OS_Space_BookingDTO data)
        {
            try
            {

                data.onlinepaymentdata = (from a in _VMSContext.HR_Master_Account_DetailsDMO
                                          from b in _VMSContext.PAYUDETAILS
                                          where (a.HRMF_Id == data.HRMF_Id && a.IMPG_Id == b.IMPG_Id && b.IMPG_PGFlag == data.IMPG_PGFlag)
                                          select new OS_Space_BookingDTO
                                          {
                                              HRMA_Merchant_Id = a.HRMA_Merchant_Id,
                                              HRMA_Salt_Id = a.HRMA_Salt_Id,
                                              HRMA_Authorisation_Id = a.HRMA_Authorisation_Id,
                                              HRMF_URL = a.HRMF_URL,
                                              HRMF_Submerchant_Id = a.HRMF_Submerchant_Id
                                          }).Distinct().ToArray();
                GenerateOrderId(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public OS_Space_BookingDTO GenerateOrderId(OS_Space_BookingDTO data)
        {
            try
            {
                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _VMSContext.Fee_PaymentGateway_DetailsDMO.Where(t => t.MI_Id == data.MI_Id && t.FPGD_PGName == data.IMPG_PGFlag).Distinct().ToList();

                Dictionary<String, object> transfersnotes = new Dictionary<String, object>();

                transfersnotes.Add("notes_1", data.OSSPBOOK_BillingName);
                transfersnotes.Add("notes_2", data.OSSPBOOK_BillingAdd1);
                transfersnotes.Add("notes_3", data.OSSPBOOK_BillingAdd2);
                transfersnotes.Add("notes_4", data.OSSPBOOK_BillingAdd3);
                transfersnotes.Add("notes_5", data.OSSPBOOK_BillingAdd4);
                transfersnotes.Add("notes_6", data.OSSPBOOK_BillingCity);
                transfersnotes.Add("notes_7", data.OSSPBOOK_BillingPinCode);

                Dictionary<string, object> input = new Dictionary<string, object>();
                //input.Add("amount", 1 * 100);
                input.Add("amount", data.payable_Amount * 100); // this amount should be same as transaction amount
                input.Add("currency", "INR");
                input.Add("payment_capture", 1);
                input.Add("notes", transfersnotes);

                string key = paymentdetails.FirstOrDefault().FPGD_SaltKey;
                string secret = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;

                RazorpayClient client = new RazorpayClient(key, secret);
                Razorpay.Api.Order order = client.Order.Create(input);
                data.orderId = order["id"].ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public string sendmail_space(long MI_Id, string Email, string Template, long UserID, long bookingId, long HRMR_Id)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "";
                }
                var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = Mailmsg.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    Mailmsg = result;
                    Mailcontent = result;
                }
                else
                {
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "SMSMAILPARAMETER_SPACEBOOKING";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@bookingId",
                           SqlDbType.BigInt)
                        {
                            Value = bookingId
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });



                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }
                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[val.Keys.ToArray()[p]]);
                                Mailmsg = result;
                            }
                        }
                    }
                    Mailmsg = result;
                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val[val.Keys.ToArray()[p]]);
                                Mailcontent = Resultsms;
                            }
                        }
                    }
                    Mailcontent = Resultsms;
                }

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();


                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _Context.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);


                        var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                if (img[i].IVRM_Att_Path != null && img[i].IVRM_Att_Path != "")
                                {
                                    var webClient = new WebClient();
                                    byte[] imageBytes = webClient.DownloadData(img[i].IVRM_Att_Path);
                                    string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                    message.AddAttachment(img[i].IVRM_Att_Name, fileContentsAsBase64, null, null, null);
                                }
                            }
                        }

                        var cclist = (from a in _VMSContext.HR_Master_Room_ContactsDMO
                                      from b in _VMSContext.Multiple_Email_DMO
                                      where a.HRME_Id == b.HRME_Id && a.HRMRCO_ActiveFlag == true && b.HRMEM_DeFaultFlag == "default" && a.HRMR_Id == HRMR_Id
                                      select b).ToList();
                        if (cclist.Count > 0)
                        {
                            foreach (var item in cclist)
                            {
                                if (item.HRMEM_EmailId != "")
                                {
                                    message.AddCc(item.HRMEM_EmailId);
                                }
                            }
                        }

                        //if (template[0].ISES_EnableMailCCFlg == true && template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        //{
                        //    string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');
                        //    foreach (var c in ccmaildetails)
                        //    {
                        //        if (c != Email)
                        //        {
                        //            message.AddCc(c);
                        //        }
                        //    }
                        //}
                        //if (template[0].ISES_EnableMailBCCFlg == true && template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        //{
                        //    string[] bccmaildetails = template[0].ISES_MailBCCId.Split(',');
                        //    foreach (var c in bccmaildetails)
                        //    {
                        //        if (c != Email)
                        //        {
                        //            message.AddBcc(c);
                        //        }
                        //    }
                        //}




                        message.HtmlContent = Mailmsg;



                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        string mailcc = "";
                        string mailbcc = "";
                        //if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        //{
                        //    string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                        //    mailcc = ccmail[0].ToString();

                        //    if (ccmail.Length > 1)
                        //    {
                        //        if (ccmail[1] != null || ccmail[1] != "")
                        //        {
                        //            mailbcc = ccmail[1].ToString();
                        //        }
                        //    }

                        //}

                        var cclist = (from a in _VMSContext.HR_Master_Room_ContactsDMO
                                      from b in _VMSContext.Multiple_Email_DMO
                                      where a.HRME_Id == b.HRME_Id && a.HRMRCO_ActiveFlag == true && b.HRMEM_DeFaultFlag == "default" && a.HRMR_Id == HRMR_Id
                                      select b).ToList();
                        if (cclist.Count > 0)
                        {
                            foreach (var item in cclist)
                            {
                                if (item.HRMEM_EmailId != "")
                                {
                                    mailbcc = item.HRMEM_EmailId;
                                }
                            }
                        }


                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                                Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                            };

                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                            clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                            clientsmtp.EnableSsl = true;

                            using (var emailMessage = new MailMessage())
                            {


                                emailMessage.To.Add(new MailAddress(Email));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {



                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }



                                        }
                                    }
                                }


                                if (mailcc != null && mailcc != "")
                                {
                                    emailMessage.CC.Add(mailcc);
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    emailMessage.Bcc.Add(mailbcc);
                                }
                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }


                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailcontent
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }


        public OS_Space_BookingDTO UserBookingDetails(OS_Space_BookingDTO data)
        {
            try
            {
                using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SPACE_BOOKING_DETAILS_USERWISE";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                        SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@UserID",
                        SqlDbType.BigInt)
                    {
                        Value = data.OSUREG_Id
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
                        data.bookingdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return data;
        }

        public OS_Space_BookingDTO SaveBooking_Payment(OS_Space_BookingDTO dto)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            DateTime dt = DateTime.Now;
            DateTime tdate = dt.Date;

            var acd_Id = _VMSContext.AcademicYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();
            try
            {
                var c1 = 0;
                var count = _VMSContext.FeePaymentDetailsDMO_con.Where(a => a.MI_Id == dto.MI_Id).ToList();
                if (count.Count > 0)
                {
                    c1 = count.Count;
                    c1 = c1 + 1;
                }
                else
                {
                    c1 = 1;
                }

                {

                    FeePaymentDetailsDMO fee = new FeePaymentDetailsDMO();
                    fee.FTCU_Id = 1;
                    fee.FYP_Receipt_No = c1.ToString();
                    fee.FYP_Bank_Name = dto.FYP_Bank_Name;
                    fee.FYP_Bank_Or_Cash = "O";
                    fee.FYP_DD_Cheque_Date = indianTime;
                    fee.FYP_Date = indianTime;
                    fee.FYP_Tot_Amount = dto.FYP_Tot_Amount;
                    fee.FYP_Tot_Waived_Amt = 0;
                    fee.FYP_Tot_Fine_Amt = 0;
                    fee.FYP_Tot_Concession_Amt = 0;
                    fee.FYP_Remarks = "SpaceBooking";
                    fee.MI_Id = dto.MI_Id;
                    fee.DOE = indianTime;
                    fee.CreatedDate = indianTime;
                    fee.UpdatedDate = indianTime;
                    fee.user_id = dto.OSUREG_Id;
                    fee.fyp_transaction_id = dto.fyp_transaction_id;
                    fee.FYP_PaymentReference_Id = dto.orderId;
                    fee.ASMAY_ID = acd_Id;

                    _VMSContext.Add(fee);
                }

                var checksave = _VMSContext.SaveChanges();
                if (checksave > 0)
                {
                    var bookdetails = _VMSContext.OS_Space_Booking.Single(e => e.OSSPBOOK_Id == dto.OSSPBOOK_Id);
                    bookdetails.OSSPBOOK_Status = "CONFIRM";
                    _VMSContext.Update(bookdetails);
                    _VMSContext.SaveChanges();
                    var Template1 = "SpaceBooking_conform_P";
                    var getmail1 = _VMSContext.Office_Space_User_Registration.Single(a => a.OSUREG_ID == dto.OSUREG_Id).OSUREG_EmailId;
                    onlineSpace_booking_payment_conformation_sendmail(getmail1, dto.MI_Id, dto.OSSPBOOK_Id, dto.fyp_transaction_id, dto.FYP_Bank_Name, dto.HRMR_Id, Template1);

                    var Template = "SpaceBooking_conform_E";
                    var getmail = _VMSContext.Office_Space_User_Registration.Single(a => a.OSUREG_ID == dto.OSUREG_Id).OSUREG_EmailId;
                    onlineSpace_booking_conformation_sendmail(getmail, dto.MI_Id, dto.OSUREG_Id, dto.HRMR_Id, Template, dto.OSSPBOOK_Id);

                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return dto;
        }


        public string onlineSpace_booking_conformation_sendmail(string mail_id, long MI_Id, long OSUREG_Id, long HRMR_Id, string Template, long OSSPBOOK_Id)
        {
            try
            {


                Dictionary<string, string> val = new Dictionary<string, string>();
                // val.Add("[PWD]", OSSPBOOK_Id);
                var template = _VMSContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }


                var institutionName = _VMSContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _VMSContext.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _VMSContext.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_MailBody;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "Spacebookingconform_mail";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                    {
                        Value = MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@OSUREG_Id",
                        SqlDbType.BigInt)
                    {
                        Value = OSSPBOOK_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMR_Id",
                        SqlDbType.BigInt)
                    {
                        Value = HRMR_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@template",
                       SqlDbType.VarChar)
                    {
                        Value = Template
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

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
                                    var datatype = dataReader.GetFieldType(iFiled);
                                    if (datatype.Name == "DateTime")
                                    {
                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                    }
                                    else
                                    {
                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }

                }



                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
                            Mailmsg = result;
                        }
                    }
                }
                Mailmsg = result;

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
                            Mailcontent = Resultsms;
                        }
                    }
                }
                Mailcontent = Resultsms;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _VMSContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _VMSContext.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        string mailcc = "";
                        string mailbcc = "";
                        //if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        //{
                        //    string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                        //    mailcc = ccmail[0].ToString();

                        //    if (ccmail.Length > 1)
                        //    {
                        //        if (ccmail[1] != null || ccmail[1] != "")
                        //        {
                        //            mailbcc = ccmail[1].ToString();
                        //        }
                        //    }

                        //}

                        var cclist = (from a in _VMSContext.HR_Master_Room_ContactsDMO
                                      from b in _VMSContext.Multiple_Email_DMO
                                      where a.HRME_Id == b.HRME_Id && a.HRMRCO_ActiveFlag == true && b.HRMEM_DeFaultFlag == "default" && a.HRMR_Id == HRMR_Id
                                      select b).ToList();
                        if (cclist.Count > 0)
                        {
                            foreach (var item in cclist)
                            {
                                if (item.HRMEM_EmailId != "")
                                {
                                    mailbcc = item.HRMEM_EmailId;
                                }
                            }
                        }


                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


                        //Sending mail using SendGrid API.
                        //Date:07-02-2017.

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(mail_id);

                        if (Attechement.Equals("1"))
                        {
                            var img = _VMSContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                            if (img.Count > 0)
                            {
                                for (int i = 0; i < img.Count; i++)
                                {
                                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                    Stream stream = response.GetResponseStream();
                                    message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                                }
                            }
                        }

                        if (mailcc != null && mailcc != "")
                        {
                            message.AddCc(mailcc);
                        }
                        if (mailbcc != null && mailbcc != "")
                        {
                            message.AddBcc(mailbcc);
                        }

                        message.HtmlContent = Mailmsg;

                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {


                        string mailcc = "";
                        string mailbcc = "";
                        if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        {
                            string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                            mailcc = ccmail[0].ToString();

                            if (ccmail.Length > 1)
                            {
                                if (ccmail[1] != null || ccmail[1] != "")
                                {
                                    mailbcc = ccmail[1].ToString();
                                }
                            }

                        }
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                                Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                            };

                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                            clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                            clientsmtp.EnableSsl = true;

                            using (var emailMessage = new MailMessage())
                            {


                                emailMessage.To.Add(new MailAddress(mail_id));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _VMSContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {

                                                //emailMessage.Attachments.Add(new System.Net.Mail.Attachment("https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf"));

                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }


                                            //var attachment = new MimePart("image", "gif")
                                            //{
                                            //    ContentObject = new ContentObject(File.OpenRead(img[i].IVRM_Att_Path), ContentEncoding.Default),
                                            //    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                                            //    ContentTransferEncoding = ContentEncoding.Base64,
                                            //    FileName = Path.GetFileName(img[i].IVRM_Att_Path)
                                            //};
                                        }
                                    }
                                }


                                if (mailcc != null && mailcc != "")
                                {
                                    emailMessage.CC.Add(mailcc);
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    emailMessage.Bcc.Add(mailbcc);
                                }
                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }



                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
         public string onlineSpace_booking_payment_conformation_sendmail(string mail_id, long MI_Id,  long OSSPBOOK_Id, string fyp_transaction_id, string FYP_Bank_Name, long HRMR_Id, string Template)
        {
            try
            {


                Dictionary<string, string> val = new Dictionary<string, string>();
                // val.Add("[PWD]", OSSPBOOK_Id);
                var template = _VMSContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }


                var institutionName = _VMSContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _VMSContext.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _VMSContext.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_MailBody;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "Spacebookingpaymentconform_mail";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                    {
                        Value = MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@OSSPBOOK_Id",
                        SqlDbType.BigInt)
                    {
                        Value = OSSPBOOK_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMR_Id",
                        SqlDbType.BigInt)
                    {
                        Value = HRMR_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@template",
                       SqlDbType.VarChar)
                    {
                        Value = Template
                    });

                     cmd.Parameters.Add(new SqlParameter("@transactionId",
                       SqlDbType.VarChar)
                    {
                        Value = fyp_transaction_id
                     });

                     cmd.Parameters.Add(new SqlParameter("@Bank",
                       SqlDbType.VarChar)
                    {
                        Value = FYP_Bank_Name
                     });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

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
                                    var datatype = dataReader.GetFieldType(iFiled);
                                    if (datatype.Name == "DateTime")
                                    {
                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                    }
                                    else
                                    {
                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }

                }



                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
                            Mailmsg = result;
                        }
                    }
                }
                Mailmsg = result;

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
                            Mailcontent = Resultsms;
                        }
                    }
                }
                Mailcontent = Resultsms;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _VMSContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _VMSContext.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        string mailcc = "";
                        string mailbcc = "";
                        //if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        //{
                        //    string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                        //    mailcc = ccmail[0].ToString();

                        //    if (ccmail.Length > 1)
                        //    {
                        //        if (ccmail[1] != null || ccmail[1] != "")
                        //        {
                        //            mailbcc = ccmail[1].ToString();
                        //        }
                        //    }

                        //}

                        var cclist = (from a in _VMSContext.HR_Master_Room_ContactsDMO
                                      from b in _VMSContext.Multiple_Email_DMO
                                      where a.HRME_Id == b.HRME_Id && a.HRMRCO_ActiveFlag == true && b.HRMEM_DeFaultFlag == "default" && a.HRMR_Id == HRMR_Id
                                      select b).ToList();
                        if (cclist.Count > 0)
                        {
                            foreach (var item in cclist)
                            {
                                if (item.HRMEM_EmailId != "")
                                {
                                    mailbcc = item.HRMEM_EmailId;
                                }
                            }
                        }


                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


                        //Sending mail using SendGrid API.
                        //Date:07-02-2017.

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(mail_id);

                        if (Attechement.Equals("1"))
                        {
                            var img = _VMSContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                            if (img.Count > 0)
                            {
                                for (int i = 0; i < img.Count; i++)
                                {
                                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                    Stream stream = response.GetResponseStream();
                                    message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                                }
                            }
                        }

                        if (mailcc != null && mailcc != "")
                        {
                            message.AddCc(mailcc);
                        }
                        if (mailbcc != null && mailbcc != "")
                        {
                            message.AddBcc(mailbcc);
                        }

                        message.HtmlContent = Mailmsg;

                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {


                        string mailcc = "";
                        string mailbcc = "";
                        if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        {
                            string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                            mailcc = ccmail[0].ToString();

                            if (ccmail.Length > 1)
                            {
                                if (ccmail[1] != null || ccmail[1] != "")
                                {
                                    mailbcc = ccmail[1].ToString();
                                }
                            }

                        }
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                                Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                            };

                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                            clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                            clientsmtp.EnableSsl = true;

                            using (var emailMessage = new MailMessage())
                            {


                                emailMessage.To.Add(new MailAddress(mail_id));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _VMSContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {

                                                //emailMessage.Attachments.Add(new System.Net.Mail.Attachment("https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf"));

                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }


                                            //var attachment = new MimePart("image", "gif")
                                            //{
                                            //    ContentObject = new ContentObject(File.OpenRead(img[i].IVRM_Att_Path), ContentEncoding.Default),
                                            //    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                                            //    ContentTransferEncoding = ContentEncoding.Base64,
                                            //    FileName = Path.GetFileName(img[i].IVRM_Att_Path)
                                            //};
                                        }
                                    }
                                }


                                if (mailcc != null && mailcc != "")
                                {
                                    emailMessage.CC.Add(mailcc);
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    emailMessage.Bcc.Add(mailbcc);
                                }
                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }



                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
           public string onlineSpace_booking_enquiry_sendmail(long? OSUREG_Id, long MI_Id, string Template2)
        {
            try
            {

                var re_mail = _VMSContext.Office_Space_User_Registration.Where(a => a.OSUREG_ID == OSUREG_Id && a.MI_ID == MI_Id).ToList();
                Dictionary<string, string> val = new Dictionary<string, string>();
                // val.Add("[PWD]", OSSPBOOK_Id);
                var template = _VMSContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template2 && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }


                var institutionName = _VMSContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _VMSContext.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _VMSContext.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_MailBody;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "Spacebookingpaymentconform_mail";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                    {
                        Value = MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@OSSPBOOK_Id",
                        SqlDbType.BigInt)
                    {
                        Value = OSUREG_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMR_Id",
                        SqlDbType.BigInt)
                    {
                        Value = 1
                    });

                    cmd.Parameters.Add(new SqlParameter("@template",
                       SqlDbType.VarChar)
                    {
                        Value = Template2
                    });

                     cmd.Parameters.Add(new SqlParameter("@transactionId",
                       SqlDbType.VarChar)
                    {
                        Value = 1
                     });

                     cmd.Parameters.Add(new SqlParameter("@Bank",
                       SqlDbType.VarChar)
                    {
                        Value = 1
                     });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

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
                                    var datatype = dataReader.GetFieldType(iFiled);
                                    if (datatype.Name == "DateTime")
                                    {
                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                    }
                                    else
                                    {
                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }

                }



                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
                            Mailmsg = result;
                        }
                    }
                }
                Mailmsg = result;

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
                            Mailcontent = Resultsms;
                        }
                    }
                }
                Mailcontent = Resultsms;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _VMSContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _VMSContext.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        string mailcc = "";
                        string mailbcc = "";
                 

                        //var cclist = (from a in _VMSContext.HR_Master_Room_ContactsDMO
                        //              from b in _VMSContext.Multiple_Email_DMO
                        //              where a.HRME_Id == b.HRME_Id && a.HRMRCO_ActiveFlag == true && b.HRMEM_DeFaultFlag == "default" && a.HRMR_Id == HRMR_Id
                        //              select b).ToList();
                        //if (cclist.Count > 0)
                        //{
                        //    foreach (var item in cclist)
                        //    {
                        //        if (item.HRMEM_EmailId != "")
                        //        {
                        //            mailbcc = item.HRMEM_EmailId;
                        //        }
                        //    }
                        //}


                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


                        //Sending mail using SendGrid API.
                        //Date:07-02-2017.

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(re_mail[0].OSUREG_EmailId);

                        if (Attechement.Equals("1"))
                        {
                            var img = _VMSContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                            if (img.Count > 0)
                            {
                                for (int i = 0; i < img.Count; i++)
                                {
                                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                    Stream stream = response.GetResponseStream();
                                    message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                                }
                            }
                        }

                        if (mailcc != null && mailcc != "")
                        {
                            message.AddCc(mailcc);
                        }
                        if (mailbcc != null && mailbcc != "")
                        {
                            message.AddBcc(mailbcc);
                        }

                        message.HtmlContent = Mailmsg;

                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {


                        string mailcc = "";
                        string mailbcc = "";
                        if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        {
                            string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                            mailcc = ccmail[0].ToString();

                            if (ccmail.Length > 1)
                            {
                                if (ccmail[1] != null || ccmail[1] != "")
                                {
                                    mailbcc = ccmail[1].ToString();
                                }
                            }

                        }
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                                Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                            };

                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                            clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                            clientsmtp.EnableSsl = true;

                            using (var emailMessage = new MailMessage())
                            {


                                emailMessage.To.Add(new MailAddress(re_mail[0].OSUREG_EmailId));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _VMSContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {

                                                //emailMessage.Attachments.Add(new System.Net.Mail.Attachment("https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf"));

                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }


                                            //var attachment = new MimePart("image", "gif")
                                            //{
                                            //    ContentObject = new ContentObject(File.OpenRead(img[i].IVRM_Att_Path), ContentEncoding.Default),
                                            //    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                                            //    ContentTransferEncoding = ContentEncoding.Base64,
                                            //    FileName = Path.GetFileName(img[i].IVRM_Att_Path)
                                            //};
                                        }
                                    }
                                }


                                if (mailcc != null && mailcc != "")
                                {
                                    emailMessage.CC.Add(mailcc);
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    emailMessage.Bcc.Add(mailbcc);
                                }
                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }



                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public OS_Space_BookingDTO send_email(OS_Space_BookingDTO dto)
        {
            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();


                string Mailmsg = dto.OS_message;

             
                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _VMSContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(dto.MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = dto.OS_subject;
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _VMSContext.GenConfig.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        string mailcc = "";
                        string mailbcc = "";

                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }
                        
                        var message = new SendGridMessage();
                        message.From = new EmailAddress(dto.OS_email);
                        message.Subject = Subject;
                        message.AddTo(SendingEmail);
                        
                        message.HtmlContent = Mailmsg;

                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();

                        dto.retrval = true;
                    }
                    else
                    {


                        string mailcc = "";
                        string mailbcc = "";
                       
                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                                Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                            };

                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                            clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                            clientsmtp.EnableSsl = true;

                            using (var emailMessage = new MailMessage())
                            {


                                emailMessage.To.Add(new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName));
                                emailMessage.From = new MailAddress(dto.OS_email);
                                emailMessage.Subject = dto.OS_subject;
                                emailMessage.Body = dto.OS_message;
                                emailMessage.IsBodyHtml = true;
                                
                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }



                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
               
            }

            return dto;
        }
    }
}
