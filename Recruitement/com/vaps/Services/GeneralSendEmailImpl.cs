using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using PreadmissionDTOs.com.vaps.VMS.Training;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class GeneralSendEmailImpl : Interfaces.GeneralSendEmailInterface
    {
        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _db;
        public GeneralSendEmailImpl(VMSContext VMSContext, DomainModelMsSqlServerContext Context)
        {
            _VMSContext = VMSContext;
            _db = Context;
        }
        public SalesSMSEMAILDTO sendsmsemail(SalesSMSEMAILDTO data)
        {

            try
            {
                if (data.Emailtemp != null && data.Emailtemp.Length > 0)
                {

                    foreach (var c in data.Emailtemp)
                    {
                        if (c.EmailId != "test@gmail.com")
                        {
                            string a = sendmailWT(data.MI_Id, c.EmailId, data.msg, data.esubject, data.Footer, data.filelist, data.athflag, data.FHEAD);
                        }
                    }
                    data.retrunMsg = "true";
                }

            }
            catch (Exception ee)
            {
                data.retrunMsg = "false";
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        //sendsmsemailsales
        public async Task<SalesSMSEMAILDTO> sendsmsemailsales(SalesSMSEMAILDTO data)
        {

            try
            {
                whatsapp whtsappnew = new whatsapp(_db); string filename = ""; string fileid = "";
                string MI_Name = _db.Institute.Where(R => R.MI_Id == data.MI_Id).FirstOrDefault().MI_Name;
                if (data.Emailtemp != null && data.Emailtemp.Length > 0 && data.rdopunch == "C")
                {
                    if (data.filelist != null && data.filelist.Length > 0)
                    {
                        data.athflag = true;
                        foreach (var d in data.filelist)
                        {
                            filename = d.cfilename;
                            fileid = d.cfilepath;
                        }
                    }
                    else
                    {
                        data.athflag = false;
                    }
                    foreach (var c in data.Emailtemp)
                    {
                        if (c.EmailId != "test@gmail.com" && data.EmailFlag == true)
                        {
                            //Thanks letter for visiting stall.

                            string a = sendmailWT(data.MI_Id, c.EmailId, c.msg, c.esubject, c.Footer, data.filelist, data.athflag, c.FHEAD);
                        }
                        if (c.ISMSLE_ContactNo != "" && data.Whats_Flag == true && data.athflag == true)
                        {
                            string ss = await whtsappnew.Qutation(data.MI_Id, c.ISMSLE_ContactNo, data.TemplateName, filename, fileid, "Dear Sir/Madam ,", MI_Name);
                        }
                    }
                    data.retrunMsg = "true";
                }
                if (data.Emailtemp != null && data.Emailtemp.Length > 0 && data.rdopunch == "R")
                {
                    data.athflag = true;
                    foreach (var c in data.Emailtemp)
                    {
                        if (c.EmailId != "test@gmail.com" && data.EmailFlag == true)
                        {
                            var files = new NAACCriteriaFivefileDTO[]
                        {
                        new NAACCriteriaFivefileDTO
                          {
                           cfilepath = "https://dcampusstrg.blob.core.windows.net/presales/VTS Brochure Final Mail.pdf",
                                  cfilename = "VTS_Brochure.pdf",

                            },
            new NAACCriteriaFivefileDTO
            {
                 cfilepath = "https://dcampusstrg.blob.core.windows.net/presales/Science Park Mail.pdf",
                                  cfilename = "Science_Park.pdf",
            },
            new NAACCriteriaFivefileDTO
            {
                 cfilepath = "https://dcampusstrg.blob.core.windows.net/presales/Advanced E-Lab 2023 Web.pdf",
                                  cfilename = "ELab.pdf",
            },

        };
                            data.athflag = true;
                            //
                            // string ISMSLERaw_ExibitionDate = c.ISMSLERaw_ExibitionDate.Value.ToString("yyyy-MM-dd");
                            string ISMSLERaw_ExibitionDate = c.ISMSLERaw_ExibitionDate.Value.ToString("dd-MM-yyyy");
                            var place = _db.State.Where(R => R.IVRMMS_Id == Convert.ToInt64(c.State)).ToList();
                            //addded
                            string references = "";
                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                            {
                               
                                cmd.CommandText = "FMS_Correspondence_Raw_Insert";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@ISMSLE_LeadName", SqlDbType.VarChar) { Value = c.ISMSLE_LeadName });
                                cmd.Parameters.Add(new SqlParameter("@Subject", SqlDbType.VarChar) { Value = "Thank you for visiting our stall during the "+c.ISMSLERaw_Exibition+"   held in Bhubaneshwar on 27 to 29 Nov 2024" });
                                cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.userid });
                                cmd.Parameters.Add(new SqlParameter("@ContactPerson", SqlDbType.VarChar) { Value = c.Contact_Person });
                                cmd.Parameters.Add(new SqlParameter("@Result", SqlDbType.VarChar, 4000)
                                {
                                    Direction = ParameterDirection.Output
                                });
                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var data1 = cmd.ExecuteNonQuery();

                                references = cmd.Parameters["@Result"].Value.ToString();
                            }

                            string str = $@"
<div>
   
    <div class='col-sm-12'>
        <p><strong>Ref : {references} </strong></p>
         <p><strong>Date: {ISMSLERaw_ExibitionDate}</strong></p>
        
        <p>
            To <br> The {c.ISMSLE_ContactDesignation}
            <br> {c.ISMSLE_LeadName}
            <br> {c.LeadAddress}, {place.FirstOrDefault().IVRMMS_Name} <br>
            <br> Contact No: {c.ISMSLE_ContactNo}
            <br> Email: {c.EmailId}
        </p>

        <p><strong>Sub: Thank you for visiting our stall during the {c.ISMSLERaw_Exibition}   held in {c.Lead_heldOn} .</strong></p>

        <p>Dear Sir/Madam,</p>

        <p>We convey our sincere thanks to you and your team for visiting our stall during  {c.ISMSLERaw_Exibition}  held in {c.Lead_heldOn}. During our interaction with you, <strong>we explained and demonstrated our Digital Campus on SAAS (Software as Service) Model / Mobile Apps Version of IOS & Android / GPRS System / E-Lab Solutions / Science Park Models.</strong></p>

        <p>We have also explained you the innovative new features we have added in our Digital Campus ERP Software:</p>

        <p>
            a) WhatsApp and tally integration with our IVRM ERP<br>
            b) New Mobile App for parents and staff<br>
            c) ChatGPT integration<br>
            d) ChatBot integration<br>
            e) Smartwatch Integration<br>
            f) Implementation of Digital Library<br>
            g) Social Media Integration<br>
            h) QR code for accessing student data, admission data, canteen and library, etc.
        </p>

        <p>These features will have added advantages to manage the campus better and more efficiently.</p>

        <p>We, VAPS Technosoft are an ISO 9001 & NASSCOM Certified Company specialized in digitalization, customization, integration solution providing from the last 24+ years. VAPS manages, secures and automates data providing you with a comprehensive solution, revolutionizing your institutional operations. VAPS designs and implements IVRM ERP software for educational institutions to function to its maximum potential by eliminating all human errors.</p>

        <p>VAPS is the country's pioneer in providing a centralized information storage system with complete end to end IT solutions including ERP software, Hi-end technology infrastructure and network solutions, all under one roof. Our SME's have carefully designed a unique system called BOOT model (Built Own Operate Transfer) and SAAS (Software As A Service) module which are tailor made for your unique need. VAPS is the first company which owns world class private data centre infrastructure for the education domain. VAPS is the only company which owns both Public Data Centre AZURE and Private in-house Data Centre.</p>

        <p>During our interaction in the seminar, we have also explained you about our Unique Scient Park Models and its impact and contributions to the student community as a whole. We are sure that the Science Park Models shall certainly transform your institution in to a scientific campus.</p>

        <p>During our demonstration in our stall, you expressed the zeal for implementing the Digital Campus Solutions at your esteemed organization and also to create Science Park and you had requested us to contact you for detailed demo of our product.</p>

        <p>In this context, our Sales Team will get in touch with you and we request your good self to confirm your convenient date and time for us to demonstrate and discuss in detail about our Digital Campus / Mobile Apps Version of IOS & Android / GPRS/ eLab Solutions / Science Park Models so that the products can be implemented for the Academic Year 2024-25 at your esteemed organization/ institutions.</p>

        <p>We are enclosing herewith the following brochures:</p>

        <p>
            1) Digital Campus - SAAS Model<br>
            2) eLab - eClassroom (Interactive White Board), Robotic Lab, Language Lab<br>
            3) Science Park Models
        </p>

        <p>For more information about us do visit our company website <a href='http://www.vapstech.com'>www.vapstech.com</a>.</p>

        <p>In case you need any clarifications in this regard, we request you to kindly get in touch with our team:</p>

        <p>
            1. Sales Coordinator at Ph: 080-68112931<br>
            2. Project Manager at Ph: 080-63112935 / 9550951686
        </p>

        <p>Awaiting your early response, so that we can move forward in this direction.</p>

        <p>
            Thanking you,<br>
            Yours faithfully,
        </p>

        <p><strong>for VAPS Technosoft Pvt. Ltd.,</strong></p>

        <p>
            <strong>
                Siddesh Kumar R<br>
                Managing Director<br>
                Mob: 9448355314 / 080 68112900-903/930/934/935<br>
                email: <a href='mailto:pstomd@vapstech.com'>pstomd@vapstech.com</a> / <a href='mailto:mktgcoord@vapstech.com'>mktgcoord@vapstech.com</a> / <a href='mailto:pstomd@vapsknowledge.com'>pstomd@vapsknowledge.com</a>
            </strong>
        </p>
    </div>
</div>";


                            //

                            string a = sendmailWTT(data.MI_Id, c.EmailId, str, "Thanks letter for visiting stall.", "", files, data.athflag, "");
                        }
                        if (c.ISMSLE_ContactNo != "" && data.Whats_Flag == true && data.athflag == true)
                        {

                            var contactExistsP = _VMSContext.Database.ExecuteSqlCommand("Lead_Raw_Data_Insert @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10",
                            c.ISMSLE_LeadName, c.ISMSLE_ContactNo, c.EmailId, c.LeadAddress, c.State, c.ISMSLE_ContactDesignation, data.userid, c.ISMSLERaw_Exibition, c.ISMSLERaw_ExibitionDate, c.Contact_Person, c.ISMSMSO_Id);
                            if (contactExistsP > 0)
                            {
                                string ss = await whtsappnew.QutationRodashow(data.MI_Id, c.ISMSLE_ContactNo, "vapsroadshow", "VTSBrochure", "https://dcampusstrg.blob.core.windows.net/presales/VTSBrochure.pdf", c.Contact_Person, c.ISMSLERaw_ExibitionDate.Value.ToString("yyyy-MM-dd"), c.ISMSLERaw_Exibition);

                            }



                        }
                    }
                    data.retrunMsg = "true";
                }
            }

            catch (Exception ee)
            {
                data.retrunMsg = "false";
                Console.WriteLine(ee.Message);
                return data;
            }

            return data;
        }

        public string sendmailWT(long MI_Id, string Email, string sms, string sub, string footer, NAACCriteriaFivefileDTO[] files, bool aflag, string fhead)
        {
            try
            {
                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                string Mailcontent = sms;
                string Mailmsg = sms;
                Mailcontent = "<br />";
                Mailmsg = "<br />";

                Mailcontent = Mailcontent + sms;
                Mailmsg = Mailmsg + sms;
                if (fhead != null && fhead != "")
                {
                    Mailcontent = Mailcontent + "<br />" + fhead + "<br />";
                    Mailmsg = Mailmsg + "<br />" + fhead + "<br />";
                }
                if (footer != "" && footer != null)
                {
                    Mailcontent = Mailcontent + "<br />" + footer;
                    Mailmsg = Mailmsg + "<br />" + footer;
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
                    string Subject = sub;
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {

                        string mailcc = "";
                        string mailcc1 = "";
                        string mailcc2 = "";
                        string mailbcc = "";


                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);


                        if (aflag == true)
                        {
                            if (files.Length > 0)
                            {
                                foreach (var item in files)
                                {
                                    if (item.cfilepath != null && item.cfilepath != "")
                                    {
                                        var webClient = new WebClient();
                                        byte[] imageBytes = webClient.DownloadData(item.cfilepath);
                                        // byte[] bytes = File.ReadAllBytes(a);
                                        string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                        //message.AddAttachment(item.cfilepath, "", null, null, null);
                                        message.AddAttachment(item.cfilename, fileContentsAsBase64, null, null, null);
                                    }
                                }
                            }
                        }

                        try
                        {
                            string TempName = "SALESWITHOUTTEMPL"; 
                          
                            List<ISM_PlannerReportsDTO> dtonewtemp = new List<ISM_PlannerReportsDTO>();
                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "FETCH_GETGREETINGCCBCC";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                                SqlDbType.BigInt)
                                {
                                    Value = 1
                                });
                                cmd.Parameters.Add(new SqlParameter("@Template",
                               SqlDbType.Char)
                                {
                                    Value = TempName
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
                                            dtonewtemp.Add(new ISM_PlannerReportsDTO
                                            {
                                                MailCc = Convert.ToString(dataReader["cc"]),
                                                MailBCc = Convert.ToString(dataReader["bcc"])
                                            });
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            if (dtonewtemp.Count > 0)
                            {
                                for (int j = 0; j < dtonewtemp.Count; j++)
                                {
                                    mailcc = dtonewtemp[j].MailCc;
                                    mailbcc = dtonewtemp[j].MailBCc;
                                }
                                if (mailcc != null && mailcc != "")
                                {
                                    string[] ccmaildetails = mailcc.Split(',');

                                    foreach (var c in ccmaildetails)
                                    {
                                        if (c != Email)
                                        {
                                            message.AddCc(c);
                                        }
                                    }
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    string[] bccmaildetails = mailbcc.Split(',');

                                    foreach (var c in bccmaildetails)
                                    {
                                        if (c != Email)
                                        {
                                            message.AddBcc(c);
                                        }
                                    }
                                }
                            }

                        }
                        catch (Exception error)
                        {
                            //
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
                                emailMessage.To.Add(new MailAddress(Email));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


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
                            Value = "PRE-SALES"
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
        //onload
        public string sendmailWTT(long MI_Id, string Email, string sms, string sub, string footer, NAACCriteriaFivefileDTO[] files, bool aflag, string fhead)
        {
            try
            {
                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                string Mailcontent = sms;
                string Mailmsg = sms;
                Mailcontent = "<br />";
                Mailmsg = "<br />";

                Mailcontent = Mailcontent + sms;
                Mailmsg = Mailmsg + sms;
                if (fhead != null && fhead != "")
                {
                    Mailcontent = Mailcontent + "<br />" + fhead + "<br />";
                    Mailmsg = Mailmsg + "<br />" + fhead + "<br />";
                }
                if (footer != "" && footer != null)
                {
                    Mailcontent = Mailcontent + "<br />" + footer;
                    Mailmsg = Mailmsg + "<br />" + footer;
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
                    string Subject = sub;
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {

                        string mailcc = "";
                        string mailcc1 = "";
                        string mailcc2 = "";
                        string mailbcc = "";


                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);


                        if (aflag == true)
                        {
                            if (files.Length > 0)
                            {
                                foreach (var item in files)
                                {
                                    if (item.cfilepath != null && item.cfilepath != "")
                                    {
                                        var webClient = new WebClient();
                                        byte[] imageBytes = webClient.DownloadData(item.cfilepath);
                                        // byte[] bytes = File.ReadAllBytes(a);
                                        string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                        //message.AddAttachment(item.cfilepath, "", null, null, null);
                                        message.AddAttachment(item.cfilename, fileContentsAsBase64, null, null, null);
                                    }
                                }
                            }
                        }

                        try
                        {
                            string TempName = "SALESWITHOUTTEMPL";
                            if (fhead == "")
                            {
                                TempName = "";
                                TempName = "Exhibition";
                            }
                            List<ISM_PlannerReportsDTO> dtonewtemp = new List<ISM_PlannerReportsDTO>();
                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "FETCH_GETGREETINGCCBCC";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                                SqlDbType.BigInt)
                                {
                                    Value = 1
                                });
                                cmd.Parameters.Add(new SqlParameter("@Template",
                               SqlDbType.Char)
                                {
                                    Value = TempName
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
                                            dtonewtemp.Add(new ISM_PlannerReportsDTO
                                            {
                                                MailCc = Convert.ToString(dataReader["cc"]),
                                                MailBCc = Convert.ToString(dataReader["bcc"])
                                            });
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            if (dtonewtemp.Count > 0)
                            {
                                for (int j = 0; j < dtonewtemp.Count; j++)
                                {
                                    mailcc = dtonewtemp[j].MailCc;
                                    mailbcc = dtonewtemp[j].MailBCc;
                                }
                                if (mailcc != null && mailcc != "")
                                {
                                    string[] ccmaildetails = mailcc.Split(',');

                                    foreach (var c in ccmaildetails)
                                    {
                                        if (c != Email)
                                        {
                                            message.AddCc(c);
                                        }
                                    }
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    string[] bccmaildetails = mailbcc.Split(',');

                                    foreach (var c in bccmaildetails)
                                    {
                                        if (c != Email)
                                        {
                                            message.AddBcc(c);
                                        }
                                    }
                                }
                            }

                        }
                        catch (Exception error)
                        {
                            //
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
                                emailMessage.To.Add(new MailAddress(Email));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


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
                            Value = "PRE-SALES"
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

        public SalesSMSEMAILDTO onload(SalesSMSEMAILDTO data)
        {

            try
            {
                data.allcompany = _db.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();

                data.state_list = _db.State.Where(R => R.IVRMMC_Id == 101).Distinct().ToArray();
                
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Exibition_Master_Source";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
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
                        data.sourcelist = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

            }

            catch (Exception ee)
            {
                data.retrunMsg = "false";
                Console.WriteLine(ee.Message);
            }

            return data;
        }

    }
}
