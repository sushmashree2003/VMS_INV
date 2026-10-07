using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager.TADA;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager;
using PreadmissionDTOs.com.vaps.VMS.TADA;
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
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class CheckBookEntryApprovalImpl : Interfaces.CheckBookEntryApprovalInterface
    {

        public IssueManagerContext _context;
        public DomainModelMsSqlServerContext _db;

        public CheckBookEntryApprovalImpl(IssueManagerContext context, DomainModelMsSqlServerContext db)
        {
            _context = context;
            _db = db;
        }


        public CheckBookEntryApprovalDTO getalldata(CheckBookEntryApprovalDTO data)
        {
            try
            {
                data.allcompany = _context.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();

              

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        // Getdata
        public CheckApprovalDTO Getdata(CheckApprovalDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                if (data.Apprval != null && data.Apprval.Length > 0)
                {
                    foreach (var i in data.Apprval)
                    {
                        var indlist = _context.VMS_Payment_VoucherDMO.Where(w => w.VPAYVOU_Id == i.VPAYVOU_Id).ToList();

                        if (indlist.Count > 0)
                        {
                            VMS_Payment_Voucher_AuthorisationDMO obj = new VMS_Payment_Voucher_AuthorisationDMO();
                            obj.VPAYVOU_Id = i.VPAYVOU_Id;
                            obj.IVRMUL_Id = data.UserId;
                            obj.MI_Id = indlist.FirstOrDefault().MI_Id;
                            obj.VPAYVOUAUT_SanctioningLevel = i.level;
                            obj.VPAYVOUAUT_Remarks = i.VPAYVOU_Remarks;
                            obj.VPAYVOUAUT_SanctionedAmount = i.VPAYVOUAUT_SanctionedAmount;
                            if (i.approvecnt > 0)
                            {
                                obj.VPAYVOUAUT_StatusFlg = "Approved";
                            }
                            else
                            {
                                obj.VPAYVOUAUT_StatusFlg = "Rejected";
                            }
                            obj.VPAYVOUAUT_Date = indianTime;
                            obj.VPAYVOUAUT_ActiveFlg = true;
                            obj.VPAYVOUAUT_CreatedBy = data.UserId;
                            obj.VPAYVOUAUT_UpdatedBy = data.UserId;
                            obj.VPAYVOUAUT_CreatedDate = indianTime;
                            obj.VPAYVOUAUT_UpdatedDate = indianTime;
                            obj.VPAYVOUAUT_OTP = data.VPAYVOUAUT_OTP;
                            _context.Add(obj);
                            int ext = _context.SaveChanges();
                            if (ext > 0)
                            {
                                var checkmaxorder = (from a in _context.HR_PROCESSDMO
                                                     from b in _context.HR_Process_Auth_OrderNoDMO
                                                     where a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "CHECKBOOK"
                                                     select b.HRPAON_SanctionLevelNo).Max();

                                if (checkmaxorder == i.level)
                                {
                                   
                                    var indentres = _context.VMS_Payment_VoucherDMO.Single(a => a.VPAYVOU_Id == i.VPAYVOU_Id);
                                   
                                    decimal? penatlties = 0;
                                    if (indentres.VPAYVOU_TDS > 0)
                                    {
                                        penatlties += indentres.VPAYVOU_TDS;
                                    }
                                    if (indentres.VPAYVOU_Penalty > 0)
                                    {
                                        penatlties += indentres.VPAYVOU_Penalty;
                                    }
                                    if (indentres.VPAYVOU_ESICAmount > 0)
                                    {
                                        penatlties += indentres.VPAYVOU_ESICAmount;
                                    }
                                    if (indentres.VPAYVOU_OtherAmount > 0)
                                    {
                                        penatlties += indentres.VPAYVOU_OtherAmount;
                                    }
                                    indentres.VPAYVOU_UpdatedBy = data.UserId;
                                   
                                    indentres.VPAYVOU_UpdatedDate = indianTime;

                                    if (i.approvecnt > 0)
                                    {
                                        indentres.VPAYVOU_StatusFlg = "Approved";
                                        indentres.VPAYVOU_ApprovedAmount = i.VPAYVOUAUT_SanctionedAmount + penatlties.Value;
                                       
                                    }
                                    else
                                    {
                                        indentres.VPAYVOU_StatusFlg = "Rejected";
                                        indentres.VPAYVOU_ApprovedAmount = 0;
                                    }
                                    indentres.VPAYVOU_Remarks = i.VTADAAA_Remarks;
                                    _context.Update(indentres);
                                   

                                    _context.SaveChanges();
                                 


                                }
                                else
                                {
                                    if (i.approvecnt > 0)
                                    {

                                    }
                                    else
                                    {
                                        var indentres = _context.VMS_Payment_VoucherDMO.Single(a => a.VPAYVOU_Id == i.VPAYVOU_Id);
                                        indentres.VPAYVOU_StatusFlg = "Rejected";
                                        indentres.VPAYVOU_ApprovedAmount = 0;
                                        indentres.VPAYVOU_UpdatedBy = data.UserId;
                                        indentres.VPAYVOU_UpdatedDate = indianTime;
                                        indentres.VPAYVOU_Remarks = i.VTADAAA_Remarks;
                                        _context.Update(indentres);
                                        _context.SaveChanges();

                                    }
                                }
                            }

                        }
                    }
                    data.returnvalue = true;                   
                    long Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                    string emailid = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();                   
                    sendmail(data.MI_Id, emailid, "CheckBookMaiApproval", data.Apprval);
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.returnvalue = false;
            }
            return data;
        }
        public CheckBookEntryApprovalDTO TadaAllwence(CheckBookEntryApprovalDTO data)
        {
            return data;
        }


        public CheckBookEntryApprovalDTO savedta(CheckBookEntryApprovalDTO data)
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
        //edit
        public CheckBookEntryApprovalDTO edit(CheckBookEntryApprovalDTO data)

        {
            try
            {

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CheckBookItemsForApproval";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@VPAYVOU_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.VPAYVOU_Id
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
                        data.EditArray = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                data.filetwo = _context.BankAccount_BRS_FilesDMO.Where(R => R.VPAYVOU_Id == data.VPAYVOU_Id).Distinct().ToArray();
                //GRnDetails

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CheckBookGRNDEtalsApproval";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@VPAYVOU_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.VPAYVOU_Id
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
                        data.GRnDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public CheckBookEntryApprovalDTO deactive(CheckBookEntryApprovalDTO data)
        {




            return data;
        }
        public string sendmail(long MI_Id, string Email, string Template, CheckBookEntryApprovalDTO[] Apprval)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "";

                }
                string str = "";
                string UserID = "0";
                int rownumber = 0;
                if (Apprval != null)
                {
                    str += "<h4> Dear Sir/ Madam</h3> </br>";
                    str += "<h5> Cheque Approved</h5> </br>";
                    str += "<table border=''0''>" +
                                                "<thead>" +
                                              "<tr>" +
                                              "<th align=" + "center" + " style=" + ">" + "SL No" + "</th> " +
                                               "<th align=" + "left" + ">" + "Company Name" + "</th>" +
                                               "<th align=" + "left" + ">" + "Bank Name / Account Number" + "</th>" +
                                                 "<th align=" + "left" + ">" + "Cheque Number" + "</th>" +
                                              "<th align=" + "left" + ">" + "Party Name" + "</th>" +
                                               "<th align=" + "left" + ">" + "Applied Date" + "</th>" +
                                                 "<th align=" + "left" + ">" + "Approved Date" + "</th>" +
                                               "<th align=" + "left" + ">" + "Applied Amount" + "</th>" +
                                               "<th align=" + "left" + ">" + "Approved Amount" + "</th>" +
                                                "<th align=" + "left" + ">" + "Narration " + "</th>" +
                                              "<th align=" + "left" + ">" + "Voucher Created name" + "</th>" +
                                             
                                                 "<th align=" + "left" + ">" + "Approval status" + "</th>" +
                                              "</tr>" +
                                               " <thead >" +
                                                " <tbody>";

                    //
                    foreach (var t in Apprval)
                    {
                        UserID = UserID + "," + t.VPAYVOU_Id;
                    }
                    List<CheckBookEntryDTO> Tasklist = new List<CheckBookEntryDTO>();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "SMSMAILCHECKBOOK";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.VarChar)
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
                                    Tasklist.Add(new CheckBookEntryDTO
                                    {
                                        VPAYVOU_AppliedAmount = Convert.ToInt64(dataReader["VPAYVOU_AppliedAmount"]),
                                        VPAYVOU_ApprovedAmount = Convert.ToInt64(dataReader["VPAYVOU_ApprovedAmount"]),
                                        institutioncode = Convert.ToString(dataReader["VPAYVOU_PaymentDate"]),
                                        INVMS_SupplierName = Convert.ToString(dataReader["SupplierName"]),
                                        VPAYVOU_ChequeNo = Convert.ToString(dataReader["VPAYVOU_ChequeNo"]),
                                        ISMCLTC_Name = Convert.ToString(dataReader["Applicationuser"]),
                                        HRMBD_BankName = Convert.ToString(dataReader["VPAYVOU_BankAccountNo"]),
                                        VPAYVOU_Remarks = Convert.ToString(dataReader["VPAYVOU_Remarks"]),
                                        VPAYVOU_StatusFlg = Convert.ToString(dataReader["VPAYVOU_StatusFlg"]),
                                        FTI_Name = Convert.ToString(dataReader["MI_Name"]),
                                    });
                                }
                            }
                        }
                        //SupplierName
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }

                    if (Tasklist != null)
                    {
                       // string ApprovedDate = DateTime.Now.ToString("yyyy-MM-dd");
                        string ApprovedDate = DateTime.Now.ToString("dd/MM/yyyy");
                        foreach (var t in Tasklist)
                        {
                            
                            //FTI_Name
                            rownumber += 1;
                            str += "<tr>" +
                                                 "<td align=" + "left" + ">" + rownumber + "</td> " +
                                                   "<td align=" + "left" + ">" + t.FTI_Name + "</td> " +
                                                "<td align=" + "left" + ">" + t.HRMBD_BankName + "</td>" +
                                                     "<td align=" + "left" + ">" + t.VPAYVOU_ChequeNo + "</td>" +
                                                         "<td align=" + "left" + ">" + t.INVMS_SupplierName + "</td>" +
                                                           "<td align=" + "left" + ">" + t.institutioncode + "</td>" +
                                                             "<td align=" + "left" + ">" + ApprovedDate + "</td>" +
                                                             "<td align=" + "left" + ">" + t.VPAYVOU_AppliedAmount + "</td>" +
                                                                "<td align=" + "left" + ">" + t.VPAYVOU_ApprovedAmount + "</td>" +
                                                                 "<td align=" + "left" + ">" + t.VPAYVOU_Remarks + "</td>" +
                                                "<td align=" + "left" + ">" + t.ISMCLTC_Name + "</td> " +
                                                  "<td align=" + "left" + ">" + t.VPAYVOU_StatusFlg + "</td> " +
                                                "</tr>";
                        }
                    }
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
               // string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                //string result = Mailmsg;
                string Mailmsg = str;
                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
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
                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);

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

                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
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
        //ApprovalData
        public CheckBookEntryApprovalDTO ApprovalData(CheckBookEntryApprovalDTO data)
        {
            try
            {

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_PAYMENT_VOCHER_APPROVAL";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_IdT",
                   SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    //

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
                        data.getadvancetada = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
    }
}
