using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.IssueManager;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class CheckBookPrintImpl : Interfaces.CheckBookPrintInterface
    {
        public IssueManagerContext _IssueManagercontext;
        public DomainModelMsSqlServerContext _db;
        public CheckBookPrintImpl(IssueManagerContext hh, DomainModelMsSqlServerContext db)
        {
            _IssueManagercontext = hh;
            _db = db;
            //
        }

        public CheckBookPrintDTO loaddata(CheckBookPrintDTO data)
        {
            try
            {
                data.allcompany = _IssueManagercontext.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public CheckBookPrintDTO companychange(CheckBookPrintDTO data)
        {
            try
            {

                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LOAD_CHECKBOOK_ENTRY_FOR_PRINT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.checkbooklist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LOAD_PRINTED_CHECKBOOK";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.alldata = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }

        public CheckBookPrintDTO getProject(CheckBookPrintDTO data)
        {
            try
            {

                data.projectlist = (from a in _IssueManagercontext.clientTable
                                    from b in _IssueManagercontext.MastersProject_DMO
                                    from c in _IssueManagercontext.ISM_Master_Client_ProjectDMO
                                    where (a.ISMMCLT_Id == c.ISMMCLT_Id && b.ISMMPR_Id == c.ISMMPR_Id && a.MI_Id == b.MI_Id && b.ISMMPR_ActiveFlg == true
                                    && c.ISMMCLT_Id == data.ISMMCLT_Id)
                                    select new CheckBookPrintDTO
                                    {
                                        ISMMPR_ProjectName = b.ISMMPR_ProjectName,
                                        ISMMPR_Id = c.ISMMPR_Id,
                                    }).Distinct().ToArray();


                data.clientlist = _IssueManagercontext.clientTable.Where(w => w.ISMMCLT_Id == data.ISMMCLT_Id).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CheckBookPrintDTO getbom(CheckBookPrintDTO data)
        {
            try
            {
                data.projectlist = (from a in _IssueManagercontext.clientTable
                                    from b in _IssueManagercontext.MastersProject_DMO
                                    from c in _IssueManagercontext.ISM_Master_Client_ProjectDMO
                                    from d in _IssueManagercontext.ISM_Client_Master_Components_DMO
                                    from e in _IssueManagercontext.ISM_Client_Project_BOM_DMO
                                    where (a.ISMMCLT_Id == c.ISMMCLT_Id && b.ISMMPR_Id == c.ISMMPR_Id && a.MI_Id == b.MI_Id && b.ISMMPR_ActiveFlg == true
                                    && c.ISMMCLT_Id == data.ISMMCLT_Id && c.ISMMCLTPR_Id == e.ISMMCLTPR_Id && d.ISMCLTC_Id == e.ISMCLTC_Id && e.ISMCLTPRBOM_ActiveFlag == true && d.ISMCLTC_ActiveFlag == true && a.MI_Id == d.MI_Id)
                                    select new CheckBookPrintDTO
                                    {
                                        ISMCLTC_Id = d.ISMCLTC_Id,
                                        ISMCLTC_Name = d.ISMCLTC_Name,
                                        ISMCLTPRBOM_Qty = e.ISMCLTPRBOM_Qty,
                                    }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CheckBookPrintDTO savedata(CheckBookPrintDTO data)
        {
            try
            {
                //   if(data.message == "Success")
                //  {
                string VPAYVOU_ChequeNo = "";
              //  string VPAYVOU_Remarks = "";
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
               
                var update = _IssueManagercontext.VMS_Payment_VoucherDMO.Single(e => e.VPAYVOU_Id == data.VPAYVOU_Id);
                VPAYVOU_ChequeNo = update.VPAYVOU_ChequeNo;
                if (data.AccPayName == "ACCOUNT PAY")
                {
                    update.VPAYVOU_SelfPayFlg = false;
                }
                else
                {
                    update.VPAYVOU_SelfPayFlg = true;
                }
                if(data.VPAYVOU_ChequeNo !=null && data.VPAYVOU_ChequeNo !="")
                {
                    update.VPAYVOU_ChequeNo = data.VPAYVOU_ChequeNo;
                }
                update.VPAYVOU_ChequePrintedDate = data.VPAYVOU_ChequePrintedDate;
                update.VPAYVOU_EmailOTP = data.EMAILOTP;
                update.VPAYVOU_ChequePrintedFlg = true;
                update.VPAYVOU_UpdatedDate = indianTime;
                update.VPAYVOU_UpdatedBy = data.UserId;

                _IssueManagercontext.Update(update);
                int cxt = _IssueManagercontext.SaveChanges();
                var CheckbookSource = (from a in _IssueManagercontext.VMSMaster_ChequeBookDMO
                                       from b in _IssueManagercontext.VMSMaster_ChequeBook_SLNoDMO
                                       where a.MI_Id == update.MI_Id && a.VMCHEBK_Id == b.VMCHEBK_Id && a.HRMBD_Id == update.HRMBD_Id && b.VMCHEBK_LeafNo == VPAYVOU_ChequeNo
                                       select b).FirstOrDefault();
                                       
                if (cxt > 0)
                {
                    data.returnval = true;
                    CheckbookSource.VMCHEBKSLN_Remarks = data.VPAYVOU_Remarks;
                    CheckbookSource.VMCHEBKSLN_UpdatedBy = data.UserId;
                    CheckbookSource.VMCHEBKSLN_UpdatedDate = DateTime.Now;
                    CheckbookSource.VMCHEBKSLN_CancelledFlg = true;
                    _IssueManagercontext.Update(CheckbookSource);
                    int pp = _IssueManagercontext.SaveChanges();
                    string emailid = _IssueManagercontext.ApplicationUser.Where(t => t.Id == data.UserId).Select(t => t.Email).FirstOrDefault();
                    sendmail(update.MI_Id, emailid, "CheckPrinted", data.VPAYVOU_Id);
                }
                else
                {
                    data.returnval = true;
                }

                // }


            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public CheckBookPrintDTO getinvoiceno(CheckBookPrintDTO data)
        {
            try
            {
                if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.asmaY_Id;
                    data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        public CheckBookPrintDTO Editdata(CheckBookPrintDTO data)
        {
            try
            {

                var editlist = _IssueManagercontext.VMS_Payment_VoucherDMO.Where(w => w.VPAYVOU_Id == data.VPAYVOU_Id).Distinct().ToList();

                data.editlist = editlist.ToArray();


                data.clientlist = _IssueManagercontext.INV_Master_SupplierDMO.Where(d => d.INVMS_Id == editlist.FirstOrDefault().INVMS_Id).ToArray();

                data.geteditclient = (from a in _IssueManagercontext.INV_Master_SupplierDMO
                                      from b in _IssueManagercontext.VMS_Payment_VoucherDMO
                                      where (a.INVMS_Id == b.INVMS_Id && a.MI_Id == data.MI_Id
                                      && a.INVMS_Id == editlist.FirstOrDefault().INVMS_Id && b.VPAYVOU_Id == data.VPAYVOU_Id)
                                      select new CheckBookPrintDTO
                                      {
                                          INVMS_SupplierName = a.INVMS_SupplierName,
                                          VPAYVOU_Id = b.VPAYVOU_Id
                                      }).Distinct().OrderBy(a => a.INVMS_SupplierName).ToArray();


                data.editbanklist = (from a in _IssueManagercontext.VMS_BankAccount_BRSDMO
                                     from b in _IssueManagercontext.HR_Master_BankDeatils
                                     where a.HRMBD_Id == b.HRMBD_Id && a.MI_Id == data.MI_Id && b.HRMBD_BankAccountNo == editlist.FirstOrDefault().VPAYVOU_BankAccountNo
                                     select new CheckBookPrintDTO
                                     {
                                         HRMBD_Id = a.HRMBD_Id,
                                         VPAYVOU_BankBalalnce = a.VBABRS_ActualBankBalance,
                                         VBABRS_Id = a.VBABRS_Id,

                                     }

                                     ).Distinct().OrderByDescending(d => d.VBABRS_Id).Take(1).ToArray();








            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CheckBookPrintDTO clientDecative(CheckBookPrintDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var u = _IssueManagercontext.VMS_Payment_VoucherDMO.Where(t => t.VPAYVOU_Id == data.VPAYVOU_Id).SingleOrDefault();
                if (u.VPAYVOU_ActiveFlg == true)
                {
                    u.VPAYVOU_ActiveFlg = false;
                }
                else if (u.VPAYVOU_ActiveFlg == false)
                {
                    u.VPAYVOU_ActiveFlg = true;
                }

                u.VPAYVOU_UpdatedBy = data.UserId;
                u.VPAYVOU_UpdatedDate = indianTime;
                _IssueManagercontext.Update(u);
                int o = _IssueManagercontext.SaveChanges();
                if (o > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public CheckBookPrintDTO viewdetails(CheckBookPrintDTO data)
        {
            try
            {
                data.paymentmodedetails = _IssueManagercontext.IVRM_ModeOfPayment.Where(a => a.MI_Id == data.MI_Id
                && a.IVRMMOD_ActiveFlag == true).Distinct().ToArray();

                data.getpaymentdetails = _IssueManagercontext.ISM_Client_Project_Payment_DetailsDMO.Where(a => a.ISMCLTPRP_Id == data.ISMCLTPRP_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CheckBookPrintDTO savepaymentdetailsrecord(CheckBookPrintDTO data)
        {

            return data;
        }

        // Payment Configuration
        public CheckBookPrintDTO loaddataconfig(CheckBookPrintDTO data)
        {
            try
            {
                data.getconfigloaddata = _IssueManagercontext.ISM_Client_Payment_ConfigurationDMO.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CheckBookPrintDTO savedataconfig(CheckBookPrintDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.ISMCPC_Id > 0)
                {
                    var checkresult = _IssueManagercontext.ISM_Client_Payment_ConfigurationDMO.Single(a => a.ISMCPC_Id == data.ISMCPC_Id);
                    checkresult.ISMCPC_RemainderDays = data.ISMCPC_RemainderDays;
                    checkresult.ISMCPC_FullORPartialPayment = data.ISMCPC_FullORPartialPayment;
                    checkresult.UpdatedDate = indiantime0;
                    _IssueManagercontext.Update(checkresult);
                    var i = _IssueManagercontext.SaveChanges();
                    if (i > 0)
                    {
                        data.msg = "updated";
                    }
                    else
                    {
                        data.msg = "Failed";
                    }
                }
                else
                {
                    ISM_Client_Payment_ConfigurationDMO iSM_Client_Payment_ConfigurationDMO = new ISM_Client_Payment_ConfigurationDMO();
                    iSM_Client_Payment_ConfigurationDMO.ISMCPC_RemainderDays = data.ISMCPC_RemainderDays;
                    iSM_Client_Payment_ConfigurationDMO.ISMCPC_FullORPartialPayment = data.ISMCPC_FullORPartialPayment;
                    iSM_Client_Payment_ConfigurationDMO.UpdatedDate = indiantime0;
                    iSM_Client_Payment_ConfigurationDMO.CreatedDate = indiantime0;
                    _IssueManagercontext.Add(iSM_Client_Payment_ConfigurationDMO);

                    var i = _IssueManagercontext.SaveChanges();
                    if (i > 0)
                    {
                        data.msg = "saved";
                    }
                    else
                    {
                        data.msg = "Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                data.msg = "Failed";
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Payment subscription audit report
        public CheckBookPrintDTO loaddatareport(CheckBookPrintDTO data)
        {
            try
            {
                data.clientlist = (from a in _IssueManagercontext.clientTable
                                   from b in _IssueManagercontext.ISM_Master_Client_ProjectDMO
                                   where (a.ISMMCLT_Id == b.ISMMCLT_Id && a.MI_Id == data.MI_Id && a.ISMMCLT_ActiveFlag == true)
                                   select new CheckBookPrintDTO
                                   {
                                       ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                       ISMMCLT_Id = b.ISMMCLT_Id
                                   }).Distinct().OrderBy(a => a.ISMMCLT_ClientName).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CheckBookPrintDTO getreport(CheckBookPrintDTO data)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //payment notification
        public CheckBookPrintDTO paymentnotification(CheckBookPrintDTO data)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //mailTrigred
        public string sendmail(long MI_Id, string Email, string Template, long UserID)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP" || Template == "EMAILOTP2F")
                {
                    result = Mailmsg.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    Mailmsg = result;
                    Mailcontent = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "SMSMAILCHECKBOOK";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
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
                }

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

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
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        string mailcc = "";
                        string mailbcc = "";
                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            string[] ccmail = template[0].ISES_MailCCId.Split(',');

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


                        //Sending mail using SendGrid API.
                        //Date:07-02-2017.

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);
                        if (UserID > 0)
                        {
                            var couts = _IssueManagercontext.VMS_Payment_VoucherDMO.Where(R => R.VPAYVOU_Id == UserID).ToList();
                            if(couts.FirstOrDefault().INVMS_Id > 0)
                            {
                                var coutstwo = _IssueManagercontext.INV_Master_SupplierDMO.Where(R => R.INVMS_Id == couts.FirstOrDefault().INVMS_Id).ToList();
                                if(coutstwo.Count > 0 && coutstwo.FirstOrDefault().INVMS_EmailId !="")
                                {
                                    message.AddTo(coutstwo.FirstOrDefault().INVMS_EmailId);
                                }
                            }
                            
                        }
                        if (Attechement.Equals("1"))
                        {
                            var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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
                        // var client = new Web("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");

                        //if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                        //{
                        //    message.AddAttachment(template.FirstOrDefault().ISES_MailHTMLTemplate);

                        //}
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


                                emailMessage.To.Add(new MailAddress(Email));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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


                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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

    }
}