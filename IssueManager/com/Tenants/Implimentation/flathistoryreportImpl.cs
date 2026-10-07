using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager.Rating;
using DomainModel.Model.com.vapstech.IssueManager.Tenants;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager.Rating;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace IssueManager.com.Tenants.Implimentation
{
    public class flathistoryreportImpl : Interface.flathistoryreportInterface
    {
        public IssueManagerContext _context;

        public flathistoryreportImpl(IssueManagerContext _cont)
        {
            _context = _cont;
        }

        //Master Type
        public TenantAgreementDTO OnLoadRatingMasterType(TenantAgreementDTO data)
        {
            try
            {
                data.building = _context.HR_Master_Building.Where(t => t.MI_Id == data.MI_Id && t.HRMB_Id == data.HRMB_Id).ToArray();
                data.rooms = _context.HR_Master_Room.Where(t => t.MI_Id == data.MI_Id && t.HRMR_Id ==data.HRMR_Id).ToArray();
               
                data.getloaddata = (from a in _context.TenantAgreement
                                    from b in _context.HR_Master_Building
                                    from c in _context.HR_Master_Room
                                    where (a.HRMB_Id == data.HRMB_Id && a.HRMR_Id == data.HRMR_Id && a.MI_Id == data.MI_Id)
                                    select new TenantAgreementDTO
                                    {
                                        TENANTAGR_Address = a.TENANTAGR_Address,
                                        TENANTAGR_AdvanceAmount = a.TENANTAGR_AdvanceAmount,
                                        TENANTAGR_AgreementDuration = a.TENANTAGR_AgreementDuration,
                                        TENANTAGR_Id = a.TENANTAGR_Id,
                                        TENANTAGR_AgrFromDate = a.TENANTAGR_AgrFromDate,
                                        TENANTAGR_AgrToDate = a.TENANTAGR_AgrToDate,
                                        TENANTAGR_ContactNo = a.TENANTAGR_ContactNo,
                                        TENANTAGR_EmailId = a.TENANTAGR_EmailId,
                                        TENANTAGR_MonthlyRent = a.TENANTAGR_MonthlyRent,
                                        TENANTAGR_NoOfOccupants = a.TENANTAGR_NoOfOccupants,
                                        TENANTAGR_Remarks = a.TENANTAGR_Remarks,
                                        TENANTAGR_RenewedFlag = a.TENANTAGR_RenewedFlag,
                                        TENANTAGR_RentDueDay = a.TENANTAGR_RentDueDay,
                                        //TENANTAGR_RentDueMonth = a.TENANTAGR_RentDueMonth,
                                        TENANTAGR_TenantName = a.TENANTAGR_TenantName,
                                        HRMB_BuildingName=b.HRMB_BuildingName,
                                        HRMR_RoomName=c.HRMR_RoomName,
                                        count = _context.TenantAgreementFilesDMO.Where(d => a.TENANTAGR_Id == d.TENANTAGR_Id).Count()

                                    }).Distinct().ToArray();


             return RenewalRemainder(data);
             //  return PaymentRemainder(data);

               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
//added by anvesha
        public TenantAgreementDTO get_report (TenantAgreementDTO data)
        {
            try
            {
            
               
              

                data.getloaddata = (from a in _context.TenantAgreement
                                    from b in _context.HR_Master_Building
                                    from c in _context.HR_Master_Room
                                    where (a.HRMB_Id == b.HRMB_Id && a.HRMR_Id == c.HRMR_Id && a.MI_Id == data.MI_Id && a.HRMB_Id==data.HRMB_Id && a.HRMR_Id==data.HRMR_Id)
                                    select new TenantAgreementDTO
                                    {
                                        TENANTAGR_Address = a.TENANTAGR_Address,
                                        TENANTAGR_AdvanceAmount = a.TENANTAGR_AdvanceAmount,
                                        TENANTAGR_AgreementDuration = a.TENANTAGR_AgreementDuration,
                                        TENANTAGR_Id = a.TENANTAGR_Id,
                                        TENANTAGR_AgrFromDate = a.TENANTAGR_AgrFromDate,
                                        TENANTAGR_AgrToDate = a.TENANTAGR_AgrToDate,
                                        TENANTAGR_ContactNo = a.TENANTAGR_ContactNo,
                                        TENANTAGR_EmailId = a.TENANTAGR_EmailId,
                                        TENANTAGR_MonthlyRent = a.TENANTAGR_MonthlyRent,
                                        TENANTAGR_NoOfOccupants = a.TENANTAGR_NoOfOccupants,
                                        TENANTAGR_Remarks = a.TENANTAGR_Remarks,
                                        TENANTAGR_RenewedFlag = a.TENANTAGR_RenewedFlag,
                                        TENANTAGR_RentDueDay = a.TENANTAGR_RentDueDay,
                                        //TENANTAGR_RentDueMonth = a.TENANTAGR_RentDueMonth,
                                        TENANTAGR_TenantName = a.TENANTAGR_TenantName,
                                        HRMB_BuildingName = b.HRMB_BuildingName,
                                        HRMR_RoomName = c.HRMR_RoomName,
                                        count = _context.TenantAgreementFilesDMO.Where(d => a.TENANTAGR_Id == d.TENANTAGR_Id).Count()

                                    }).Distinct().ToArray();


                return RenewalRemainder(data);
                //  return PaymentRemainder(data);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //added by anvesha

        public TenantAgreementDTO ViewHealthCheckUpFiles(TenantAgreementDTO data)
        {
            try
            {
                //TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                //DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                data.getviewfilesdetails = (from a in _context.TenantAgreementFilesDMO
                                            where (a.TENANTAGR_Id == data.TENANTAGR_Id)
                                            select new TenantAgreementDTO
                                            {
                                                TENANTAGR_Id = a.TENANTAGR_Id,
                                                TENANTAGRFL_Id = a.TENANTAGRFL_Id,
                                                TENANTAGRFL_FileName = a.TENANTAGRFL_FileName,
                                                TENANTAGRFL_FileLocation = a.TENANTAGRFL_FileLocation
                                            }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public TenantAgreementDTO OnChangeMasteTypeQuesInsitution(TenantAgreementDTO data)
        {

            return data;
        }
        public TenantAgreementDTO PaymentRemainder(TenantAgreementDTO data)
        {
            List<TenantAgreementDTO> devicelist = new List<TenantAgreementDTO>();
            List<TenantAgreementDTO> deviceidsnew = new List<TenantAgreementDTO>();
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "RenewalRemainder";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value =17
                    });
                    cmd.Parameters.Add(new SqlParameter("@TYPE",
               SqlDbType.BigInt)
                    {
                        Value = 1
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
                                devicelist.Add(new TenantAgreementDTO
                                {

                                    MI_Id = Convert.ToInt64(dataReader["MI_Id"]),
                                    TENANTAGR_EmailId =Convert.ToString(dataReader["TENANTAGR_EmailId"]),
                                    TENANTAGR_Id = Convert.ToInt64(dataReader["TENANTAGR_Id"]),
                               
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }


                }

                int k = 0;
                foreach (var deviceid in devicelist)
                {
                    string m = sendmail(deviceid.MI_Id, "PAYMENTREMAINDER", 1,deviceid.TENANTAGR_EmailId,deviceid.TENANTAGR_Id);
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public TenantAgreementDTO RenewalRemainder(TenantAgreementDTO data)
        {

            List<TenantAgreementDTO> devicelist = new List<TenantAgreementDTO>();
            List<TenantAgreementDTO> deviceidsnew = new List<TenantAgreementDTO>();
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "RenewalRemainder";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = 17
                    });
                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                 SqlDbType.BigInt)
                    {
                        Value = 1
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
                                devicelist.Add(new TenantAgreementDTO
                                {


                                    MI_Id = Convert.ToInt64(dataReader["MI_Id"]),
                                    TENANTAGR_EmailId = Convert.ToString(dataReader["TENANTAGR_EmailId"]),
                                    TENANTAGR_Id = Convert.ToInt64(dataReader["TENANTAGR_Id"]),

                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }


                }

                int k = 0;
                foreach (var deviceid in devicelist)
                {
                    string m = sendmail(deviceid.MI_Id, "TENANTRENEWAL", 2, deviceid.TENANTAGR_EmailId, deviceid.TENANTAGR_Id);

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public string sendmail(long MI_Id, string Template, long type,string Email,long TENANTID)
        {
            try
            {
                MI_Id = 17;
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template
                && e.ISES_MailActiveFlag == true).ToList();

                var Paramaeters = _context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _context.Institution.Where(i => i.MI_Id == 17).ToList();

               

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "REMAINDERS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id",
                  SqlDbType.VarChar)
                    {
                        Value = TENANTID
                    });
                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                 SqlDbType.VarChar)
                    {
                        Value = type
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
                            result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
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
                            Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                            Mailcontent = Resultsms;
                        }
                    }
                }
                Mailcontent = Resultsms;

             


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(17)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();

                    var message = new SendGridMessage();

                  
                    string mailcc = "";
                        string mailbcc = "";

                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email)
;
                        //****************** EMAIL CC DETAILS ***************//

                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                message.AddCc(c);
                            }
                        }

                        //****************** EMAIL BCC DETAILS ***************//

                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailBCCId.Split(',');

                            foreach (var d in ccmaildetails)
                            {
                                message.AddCc(d);
                            }
                        }

                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        try
                        {
                            client.SendEmailAsync(message).Wait();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


                


                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _context.smsEmailSetting.Where(e => e.MI_Id == 17 && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _context.MasterModules.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "success";


        }
        public TenantAgreementDTO Editrenewal(TenantAgreementDTO data)
        {
            data.edittenantrenewal=(from a in _context.TenantAgreement
                              from b in _context.TenantAgreementRenewalDMO
                              where (a.TENANTAGR_Id == b.TENANTAGR_Id && a.TENANTAGR_Id==data.TENANTAGR_Id && b.TENANTAGRREN_Id==data.TENANTAGRREN_Id)
                              select new TenantAgreementDTO
                              {
                                  TENANTAGR_Id = a.TENANTAGR_Id,
                                  TENANTAGRREN_Id = b.TENANTAGRREN_Id,
                                  TENANTAGRREN_PreAgrFromDate = b.TENANTAGRREN_PreAgrFromDate,
                                  TENANTAGRREN_Remarks = b.TENANTAGRREN_Remarks,
                                  TENANTAGRREN_PreAgrToDate = b.TENANTAGRREN_PreAgrToDate,
                                  TENANTAGR_TenantName = a.TENANTAGR_TenantName

                              }
                                    ).Distinct().ToArray();
            return data;
        }
        public TenantAgreementDTO Deleterenewal(TenantAgreementDTO data)
        {
            try
            {
                var renewaldelete = _context.TenantAgreementRenewalDMO.Where(t => t.TENANTAGRREN_Id == data.TENANTAGRREN_Id).ToList();
                if (renewaldelete.Count > 0)
                {
                    foreach (var fl in renewaldelete)
                    {
                        _context.Remove(fl);


                    }
                }
                var renewalupdate = _context.TenantAgreement.Single(t => t.TENANTAGR_Id == data.TENANTAGR_Id);
                renewalupdate.TENANTAGR_RenewedFlag = false;
                renewalupdate.TENANTAGR_UpdatedBy = data.UserId;
                renewalupdate.TENANTAGR_UpdatedDate = DateTime.Now;

                _context.Update(renewalupdate);
                var contactExists = _context.SaveChanges();

                if (contactExists > 0)
                {
                    data.message = "Deleted";

                }
                else
                {
                    data.message = "Not Deleted";
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }


            return data;
        }
        public TenantAgreementDTO tenantchange(TenantAgreementDTO data)
        {

            data.tenantchange = _context.TenantAgreement.Where(t => t.TENANTAGR_Id == data.TENANTAGR_Id).ToArray();


            return data;
        }
        public TenantAgreementDTO saverenewal(TenantAgreementDTO data)
        {
            try
            {


                if (data.TENANTAGRREN_Id > 0)
                {
                    var resultduplicate = _context.TenantAgreementRenewalDMO.Where(a => a.TENANTAGR_Id == data.TENANTAGR_Id && a.TENANTAGRREN_PreAgrToDate == data.TENANTAGRREN_PreAgrToDate
                    && a.TENANTAGRREN_Id != data.TENANTAGRREN_Id && a.TENANTAGRREN_PreAgrFromDate == data.TENANTAGRREN_PreAgrFromDate && a.TENANTAGRREN_Remarks==data.TENANTAGRREN_Remarks).ToList();

                    if (resultduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {


                        var result = _context.TenantAgreementRenewalDMO.Single(t => t.TENANTAGR_Id == data.TENANTAGR_Id && t.TENANTAGRREN_Id == data.TENANTAGRREN_Id);
                        result.TENANTAGRREN_Remarks = data.TENANTAGRREN_Remarks;
                        result.TENANTAGRREN_PreAgrFromDate = data.TENANTAGRREN_PreAgrFromDate;
                        result.TENANTAGRREN_PreAgrToDate = data.TENANTAGRREN_PreAgrToDate;
                        result.TENANTAGRREN_Id = data.TENANTAGRREN_Id;

                        //result.TENANTAGRREN_CreatedBy = data.UserId;
                        //result.TENANTAGRREN_CreatedDate = DateTime.Now;

                        result.TENANTAGRREN_UpdatedBy = data.UserId;


                        result.TENANTAGRREN_UpdatedDate = DateTime.Now;
                        result.TENANTAGR_Id = data.TENANTAGR_Id;



                        _context.Update(result);

                        var resultval = _context.TenantAgreement.Single(t => t.TENANTAGR_Id == data.TENANTAGR_Id);
                        resultval.TENANTAGR_RenewedFlag = true;
                        resultval.TENANTAGR_UpdatedBy = data.UserId;
                        resultval.TENANTAGR_UpdatedDate = DateTime.Now;
                        resultval.TENANTAGR_AgrFromDate = data.TENANTAGR_AgrFromDate;
                        resultval.TENANTAGR_AgrToDate = data.TENANTAGR_AgrToDate;
                        resultval.TENANTAGR_AgreementDuration = data.TENANTAGR_AgreementDuration;
                        _context.Update(resultval);


                        var contactExists = _context.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;
                            data.message = "Update";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "Update";
                        }
                    }
                }
                else
                {

                    var resultduplicate = _context.TenantAgreementRenewalDMO.Where(t => t.TENANTAGR_Id == data.TENANTAGR_Id && t.TENANTAGRREN_PreAgrFromDate==data.TENANTAGRREN_PreAgrFromDate && t.TENANTAGRREN_PreAgrToDate==data.TENANTAGRREN_PreAgrToDate).ToList();



                    if (resultduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        TenantAgreementRenewal result = new TenantAgreementRenewal();
                        result.TENANTAGRREN_Remarks = data.TENANTAGRREN_Remarks;
                        result.TENANTAGRREN_PreAgrFromDate = data.TENANTAGRREN_PreAgrFromDate;
                        result.TENANTAGRREN_PreAgrToDate = data.TENANTAGRREN_PreAgrToDate;
                        result.TENANTAGRREN_Id = data.TENANTAGRREN_Id;

                        result.TENANTAGRREN_CreatedBy = data.UserId;
                        result.TENANTAGRREN_CreatedDate = DateTime.Now;

                        result.TENANTAGRREN_UpdatedBy = data.UserId;


                        result.TENANTAGRREN_UpdatedDate = DateTime.Now;
                        result.TENANTAGR_Id = data.TENANTAGR_Id;

                        _context.Add(result);


                        var resultval = _context.TenantAgreement.Single(t => t.TENANTAGR_Id == data.TENANTAGR_Id);
                        resultval.TENANTAGR_RenewedFlag = true;
                        resultval.TENANTAGR_AgrFromDate = data.TENANTAGR_AgrFromDate;
                        resultval.TENANTAGR_AgrToDate = data.TENANTAGR_AgrToDate;
                        resultval.TENANTAGR_UpdatedBy = data.UserId;
                        resultval.TENANTAGR_UpdatedDate = DateTime.Now;
                        resultval.TENANTAGR_AgreementDuration = data.TENANTAGR_AgreementDuration;




                        _context.Update(resultval);


                      


                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                            data.message = "Add";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "Add";
                        }
                    }
                    data.tenantrenewal = (from a in _context.TenantAgreement
                                          from b in _context.TenantAgreementRenewalDMO
                                          where (a.TENANTAGR_Id == b.TENANTAGR_Id)
                                          select new TenantAgreementDTO
                                          {
                                              TENANTAGR_Id = a.TENANTAGR_Id,
                                              TENANTAGRREN_Id = b.TENANTAGRREN_Id,
                                              TENANTAGRREN_PreAgrFromDate = b.TENANTAGRREN_PreAgrFromDate,
                                              TENANTAGRREN_Remarks = b.TENANTAGRREN_Remarks,
                                              TENANTAGRREN_PreAgrToDate = b.TENANTAGRREN_PreAgrToDate,
                                              TENANTAGR_TenantName = a.TENANTAGR_TenantName

                                          }
                                  ).Distinct().ToArray();


                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public TenantAgreementDTO delete(TenantAgreementDTO data)
        {
            try
            {
                var renewal = _context.TenantAgreementRenewalDMO.Where(t => t.TENANTAGR_Id == data.TENANTAGR_Id).ToList();

                if (renewal.Count == 0)
                {
                    var files = _context.TenantAgreementFilesDMO.Where(t => t.TENANTAGR_Id == data.TENANTAGR_Id).ToList();

                    if (files.Count > 0)
                    {
                        foreach (var fl in files)
                        {
                            _context.Remove(fl);


                        }
                        var tenant = _context.TenantAgreement.Where(t => t.TENANTAGR_Id == data.TENANTAGR_Id).ToList();
                        if (tenant.Count > 0)
                        {
                            foreach (var f2 in tenant)
                            {
                                _context.Remove(f2);
                               

                            }

                        }


                    }
                    else
                    {
                        var tenant = _context.TenantAgreement.Where(t => t.TENANTAGR_Id == data.TENANTAGR_Id).ToList();
                        if (tenant.Count > 0)
                        {
                            foreach (var fl in tenant)
                            {
                                _context.Remove(fl);
                              
                            }

                        }

                    }

                }
                else
                {
                    data.message = "Not Deleted";
                }
               var con= _context.SaveChanges();
                if (con > 0)
                {
                    data.message = "Deleted";
                }
                else
                {
                    data.message = "Not Deleted";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.message = "Request Failed";
            }

            return data;
        }
        public TenantAgreementDTO Edit(TenantAgreementDTO data)
        {
            try
            {

                data.geteditdetails = (from a in _context.TenantAgreement

                                       where (a.TENANTAGR_Id == data.TENANTAGR_Id)
                                       select new TenantAgreementDTO
                                       {
                                           TENANTAGR_Address = a.TENANTAGR_Address,
                                           TENANTAGR_AdvanceAmount = a.TENANTAGR_AdvanceAmount,
                                           TENANTAGR_AgreementDuration = a.TENANTAGR_AgreementDuration,
                                           TENANTAGR_Id = a.TENANTAGR_Id,
                                           TENANTAGR_AgrFromDate = a.TENANTAGR_AgrFromDate,
                                           TENANTAGR_AgrToDate = a.TENANTAGR_AgrToDate,
                                           TENANTAGR_ContactNo = a.TENANTAGR_ContactNo,
                                           TENANTAGR_EmailId = a.TENANTAGR_EmailId,
                                           TENANTAGR_MonthlyRent = a.TENANTAGR_MonthlyRent,
                                           TENANTAGR_NoOfOccupants = a.TENANTAGR_NoOfOccupants,
                                           TENANTAGR_Remarks = a.TENANTAGR_Remarks,
                                           TENANTAGR_RenewedFlag = a.TENANTAGR_RenewedFlag,
                                           TENANTAGR_RentDueDay = a.TENANTAGR_RentDueDay,
                                          // TENANTAGR_RentDueMonth = a.TENANTAGR_RentDueMonth,
                                           TENANTAGR_TenantName = a.TENANTAGR_TenantName,
                                           HRMB_Id = a.HRMB_Id,
                                           HRMR_Id = a.HRMR_Id,


                                       }).Distinct().ToArray();



                data.geteditfilesdetails = (from a in _context.TenantAgreementFilesDMO
                                            where (a.TENANTAGR_Id == data.TENANTAGR_Id)
                                            select new TenantAgreementDTO
                                            {
                                                TENANTAGRFL_Id = a.TENANTAGRFL_Id,
                                                TENANTAGRFL_Remarks = a.TENANTAGRFL_Remarks,
                                                TENANTAGRFL_FileName = a.TENANTAGRFL_FileName,
                                                TENANTAGRFL_FileLocation = a.TENANTAGRFL_FileLocation,
                                                TENANTAGR_Id = a.TENANTAGR_Id,
                                            }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public TenantAgreementDTO ViewFiles(TenantAgreementDTO data)
        {
            data.getviewfilesdetails = (from a in _context.TenantAgreementFilesDMO
                                        where (a.TENANTAGR_Id == data.TENANTAGR_Id)
                                        select new TenantAgreementDTO
                                        {
                                            TENANTAGRFL_Id = a.TENANTAGRFL_Id,
                                            TENANTAGRFL_Remarks = a.TENANTAGRFL_Remarks,
                                            TENANTAGRFL_FileName = a.TENANTAGRFL_FileName,
                                            TENANTAGRFL_FileLocation = a.TENANTAGRFL_FileLocation
                                        }).Distinct().ToArray();



            return data;
        }
    public TenantAgreementDTO buildingchange(TenantAgreementDTO data)
        {
             data.rooms = _context.HR_Master_Room.Where(t => t.MI_Id == data.MI_Id && t.HRMB_Id==data.HRMB_Id).ToArray();

            data.getfloordetails = _context.HR_Master_Floor.Where(t => t.MI_Id == data.MI_Id && t.HRMB_Id == data.HRMB_Id).ToArray();


            return data;
        }
        public TenantAgreementDTO roomchange(TenantAgreementDTO data)
        {
            data.getamenitiesdetails = (from a in _context.HR_Master_Amenities_DMO
                                        from b in _context.HR_Master_Room_AmenitiesDMO
                                        where (a.HRMAM_Id == b.HRMAM_Id && b.HRMR_Id==data.HRMR_Id)
                                        select new TenantAgreementDTO
                                        {
                                            HRMAM_Id = a.HRMAM_Id,
                                            HRMAM_AmenitiesName = a.HRMAM_AmenitiesName,
                                            HRMRAM_RentPerDay = b.HRMRAM_RentPerDay,
                                            HRMRAM_NoOfHrs = b.HRMRAM_NoOfHrs,
                                            HRMRAM_RentPerHour=b.HRMRAM_RentPerHour,

                                        }).Distinct().ToArray();



            return data;
        }


        public TenantAgreementDTO Floorchange(TenantAgreementDTO data)
        {
            data.rooms = _context.HR_Master_Room.Where(t => t.MI_Id == data.MI_Id && t.HRMB_Id == data.HRMB_Id && t.HRMF_Id==data.HRMF_Id).ToArray();




            return data;
        }



        public string sendmailToManagement(string Template, string Username, string password, string email, string mobilenumber,long mi_id)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _context.smsEmailSetting.Where(e => e.MI_Id == mi_id && e.ISES_Template_Name == Template
                && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _context.Institution.Where(i => i.MI_Id == 17).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;


                Mailmsg = Mailmsg.Replace("[USR]", Username);
                Mailmsg = Mailmsg.Replace("[PASSWORD]", password);
                Mailcontent = Template;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(17)).ToList();

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
                    smstpdetails = _context.GenConfig.Where(t => t.MI_Id.Equals(17)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "SMTP")
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

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(email)
;


                        if (Attechement.Equals("1"))
                        {
                            var img = _context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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
                        //****************** EMAIL CC DETAILS ***************//

                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                message.AddCc(c);
                            }
                        }

                        //****************** EMAIL BCC DETAILS ***************//

                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailBCCId.Split(',');

                            foreach (var d in ccmaildetails)
                            {
                                message.AddCc(d);
                            }
                        }




                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        try
                        {
                            client.SendEmailAsync(message).Wait();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                    else
                    {
                        string mailcc = "";
                        string mailbcc = "";

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(email)
;
                        //****************** EMAIL CC DETAILS ***************//

                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                message.AddCc(c);
                            }
                        }

                        //****************** EMAIL BCC DETAILS ***************//

                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailBCCId.Split(',');

                            foreach (var d in ccmaildetails)
                            {
                                message.AddCc(d);
                            }
                        }

                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        try
                        {
                            client.SendEmailAsync(message).Wait();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


                        //using (var clientsmtp = new SmtpClient())
                        //{
                        //    var credential = new NetworkCredential
                        //    {
                        //        UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                        //        Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                        //    };

                        //    clientsmtp.Credentials = credential;
                        //    clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                        //    clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                        //    clientsmtp.EnableSsl = true;
                        //    clientsmtp.UseDefaultCredentials = false;

                        //    using (var emailMessage = new MailMessage())
                        //    {


                        //        emailMessage.To.Add(new MailAddress(email));
                        //        emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                        //        emailMessage.Subject = Subject;
                        //        emailMessage.Body = Mailmsg;
                        //        emailMessage.IsBodyHtml = true;


                        //        if (Attechement.Equals("1"))
                        //        {
                        //            var img = _context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                        //            if (img.Count > 0)
                        //            {
                        //                for (int i = 0; i < img.Count; i++)
                        //                {

                        //                    foreach (var attache in img.ToList())
                        //                    {
                        //                        System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                        //                        System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                        //                        Stream stream = response.GetResponseStream();
                        //                        emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                        //                    }

                        //                }
                        //            }
                        //        }




                        //        if (mailcc != null && mailcc != "")
                        //        {
                        //            emailMessage.CC.Add(mailcc);
                        //        }
                        //        if (mailbcc != null && mailbcc != "")
                        //        {
                        //            emailMessage.Bcc.Add(mailbcc);
                        //        }

                        //        clientsmtp.Send(emailMessage);
                        //    }
                        //}

                    }


                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _context.smsEmailSetting.Where(e => e.MI_Id == 17 && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _context.MasterModules.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = email
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
                            Value = 17
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
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "success";


        }

    }
}