using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.AssetTracking;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Implementation
{
    public class AssetTagSMS_NotificationImpl : Interface.AssetTagSMS_NotificationInterface
    {
        public AssetTrackingContext _ATContext;
        public DomainModelMsSqlServerContext _db;

        public AssetTagSMS_NotificationImpl(AssetTrackingContext ATContext, DomainModelMsSqlServerContext db)
        {
            _ATContext = ATContext;
            _db = db;

        }


        public AssetTagDTO getdata(AssetTagDTO dto)
        {
            try
            {
                var miid = _ATContext.Institution.Where(a => a.MI_ActiveFlag == 1).ToList();
                foreach (var mi in miid)
                {


                    List<AssetTagDTO> que1 = new List<AssetTagDTO>();
                    using (var cmd = _ATContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "AssetTagSMSNotification_proc";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = mi.MI_Id
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
                                    que1.Add(new AssetTagDTO
                                    {
                                        INVAAT_WarantyExpiryDate = Convert.ToDateTime(dataReader["INVAAT_WarantyExpiryDate"]),

                                        MI_Id = Convert.ToInt64(dataReader["MI_Id"]),
                                        HRME_EmailId = Convert.ToString(dataReader["HRME_EmailId"])

                                    });
                                }
                            }

                        }

                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }

                    if (que1.Count > 0)
                    {
                        foreach (var item in que1)
                        {
                            int rownumber = 0;
                            string str = "";
                            string str1 = "";
                            string str2 = "";
                            DateTime s1 = System.Convert.ToDateTime(item.INVAAT_WarantyExpiryDate);
                            DateTime date = (s1);
                            String frmdt = date.ToString("yyyy-MM-dd");
                            List<AssetTagDTO> dtonewtemp = new List<AssetTagDTO>();
                            using (var cmd = _ATContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "AssetTagSMSNotificationDetails_proc";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                                {
                                    Value = mi.MI_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@WarantyExpiryDate", SqlDbType.VarChar)
                                {
                                    Value = frmdt
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
                                            dtonewtemp.Add(new AssetTagDTO
                                            {
                                                INVMI_ItemName = (dataReader["INVMI_ItemName"].ToString()),
                                                INVMS_ContactNo = (dataReader["INVMS_ContactNo"].ToString()),
                                                INVAAT_WarantyExpiryDate = Convert.ToDateTime(dataReader["INVAAT_WarantyExpiryDate"]),
                                                count_date = Convert.ToInt64(dataReader["count_date"]),
                                                HRME_EmailId = Convert.ToString(dataReader["HRME_EmailId"]),
                                                MI_Id = Convert.ToInt64(dataReader["MI_Id"]),
                                                INVMS_StoreName = Convert.ToString(dataReader["INVMS_StoreName"]),
                                                INVMS_StoreLocation = Convert.ToString(dataReader["INVMS_StoreLocation"]),

                                            });
                                        }
                                    }

                                }

                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }

                            if (dtonewtemp.Count > 0)
                            {

                                // var stdate = Convert.ToDateTime(data.ISMSLECOM_Date).ToString("dd/MM/yyyy");

                                str += "<p> <span style=" + "'font-weight:bold'" + "><span style=" + "'float: center;padding-left:10%;'" + ">" + "</span> </span></p> <table width='130%' id=" + "'tablepaging'" + " class=" + "'yui'" + " style=" + "'border-collapse: collapse; width: 100%;'" + "><tr>" +
                                       "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > SL.NO. </th>" +
                                       "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Item Name</th>" +
                                       "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Waranty Expiry Date</th>" +
                                       "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;''" + " >  Waranty day </th>" +
                                    "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;' " + " >  Store Name </th>" +
                                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;' " + " >  Store Store Location </th>" +
                                             "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;' " + " >  Store Email Id </th>" +

                                         "</tr>";
                                for (int j = 0; j < dtonewtemp.Count; j++)
                                {
                                    rownumber += 1;
                                    str1 += "<tr> " +
                                        "<td style=" + "'border: 1px solid black; text-align:center;font-size:14px;font-weight:bold;'" + " > " + rownumber + "</td> " +
                                        "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'" + " >" + dtonewtemp[j].INVMI_ItemName + "</td>" +
                                        "<td align=" + "left " + "style=" + "'font-size:14px;border: 1px solid black;font-weight:bold;font-family: sans-serif;'" + ">" + dtonewtemp[j].INVAAT_WarantyExpiryDate + "</td>" +
                                "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp[j].count_date + "</td> " +
                                 "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp[j].INVMS_StoreName + "</td> " +
                                 "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp[j].INVMS_StoreLocation + "</td> " +
                                   "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp[j].HRME_EmailId + "</td> " +


                                         "</tr>";
                                }
                                str2 = "</table>";

                            }
                            dto.message = str + str1 + str2;

                            var devicenew = "";
                            long mi_id = 0;
                            var sound = "";
                            //callnotification(devicenew, mi_id, sound, message);
                            //  Send_SMS(item.INVMS_ContactNo, message);
                            var Template = "AssetsExpireDate";
                            sendmail(item, Template, dto.message);
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

        public string callnotification(string devicenew, long mi_id, string sound, string message)
        {
            long AMST_Id = 0;
            try
            {
                //var employees = (from a in _vmsconte.Hr_Master_Employee_con
                //                 where (a.HRME_ActiveFlag == true && empids.Contains(a.HRME_Id) && a.MI_Id == mi_id)
                //                 select new HR_Training_Create_DTO
                //                 {
                //                     HRME_Id = a.HRME_Id,
                //                     HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                //                     HRME_MobileNo = a.HRME_MobileNo,
                //                     HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                //                 }).Distinct().ToList().ToArray();

                string url = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";
                long notId = 1;
                //string message = "You are Select Induction Training";


                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," + "" + '"' + "data" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "foreground" + '"' + ":" + '"' + true + '"' + " , " + '"' + "title" + '"' + ":" + '"' + /*nt.HRME_EmployeeFirstName*/ +'"' + " , " + '"' + "body" + '"' + ":" + '"' + message + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#bd3f32" + '"' + " } }";


                notificationparams.Add(daata.ToString());
                // var mycontent = JsonConvert.SerializeObject(notificationparams);
                var mycontent = notificationparams[0];
                string postdata = mycontent.ToString();
                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                connection.ContentType = "application/json";
                connection.MediaType = "application/json";
                connection.Accept = "application/json";

                connection.Method = "post";
                connection.Headers["authorization"] = "key=AAAADrksgbk:APA91bGjurLMMB23AWc8SklzSksUUMoFt6zA_XY2TMkk0BxzDYFIkYuKNpNlhtYdVIWiQ8zjsQxXIlGdWI-Zrqb9UHhNpJf9DMM7qtAFxxgZPWbhenI4KWsnpZaaeWtM6O2qR_vIHXqS";
                using (StreamWriter requestwriter = new StreamWriter(connection.GetRequestStream()))
                {
                    requestwriter.Write(postdata);
                }
                string responsedata = string.Empty;

                using (StreamReader responsereader = new StreamReader(connection.GetResponse().GetResponseStream()))
                {
                    responsedata = responsereader.ReadToEnd();
                    JObject joresponse1 = JObject.Parse(responsedata);
                }

                //
                PushNotification push_noti = new PushNotification(_db);

                //push_noti.Insert_PushNotification(mi_id, nt.HRME_EmployeeFirstName, message, nt.HRME_MobileNo, nt.HRME_AppDownloadedDeviceId, nt.HRME_Id, AMST_Id);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }

            return "success";
        }

        public string Send_SMS(string INVMS_ContactNo, string message)
        {
            string sms = "";
            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                sms = message;

                string result = "";



                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _ATContext.SMS_DETAILS_DMO.Where(t => t.MI_ID == 4).ToList();


                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = INVMS_ContactNo.ToString();
                    // PHNO = "9686061628";
                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;

                    Stream stream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);
                    //List<SMSParameters> list = JsonConvert.DeserializeObject<List<SMSParameters>>(responseparameters);
                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    //  string initialstatusss = responsedata.status;
                    //   string responsemessage = responsedata.message;
                    string messageid = responsedata;

                    //IVRM_sms_sentBoxDMO dmo2 = new IVRM_sms_sentBoxDMO();
                    //dmo2.CreatedDate = DateTime.Now;
                    //dmo2.Datetime = DateTime.Now;
                    //dmo2.Message = sms.ToString();
                    //dmo2.Message_id = messageid;
                    //dmo2.MI_Id = MID;
                    //dmo2.Mobile_no = PHNO;
                    //dmo2.Module_Name = "Calendar of Event";
                    //dmo2.To_FLag = type;
                    //dmo2.UpdatedDate = DateTime.Now;
                    //if (messageid.Contains("GID") && messageid.Contains("ID"))
                    //{
                    //    dmo2.statusofmessage = "Delivered";
                    //}
                    //else
                    //{
                    //    dmo2.statusofmessage = messageid;
                    //}
                    //_db.Add(dmo2);
                    //var flag = _db.SaveChanges();


                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public string sendmail(AssetTagDTO dto, string Template, string message1)
        {
            try
            {
                string mailcc = "";
                string mailbcc = "";


                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _ATContext.SMSEmailSetting.Where(e => e.MI_Id == dto.MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "";
                }
                var institutionName = _ATContext.Institution.Where(i => i.MI_Id == dto.MI_Id).ToList();
                var Paramaeters = _ATContext.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == dto.MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _ATContext.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;
                //string Resultsms = Mailcontent;
                //string result = Mailmsg;
                
                string result = "";
                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
                //using (var cmd = _ATContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "ASSET_EXPIRY_PARAMETER";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //      SqlDbType.BigInt)
                //    {
                //        Value = dto.MI_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@ItemName",
                //      SqlDbType.VarChar)
                //    {
                //        Value = dto.INVMI_ItemName
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@WarantyExpiryDate",
                //       SqlDbType.DateTime)
                //    {
                //        Value = dto.INVAAT_WarantyExpiryDate
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@count_date",
                //     SqlDbType.BigInt)
                //    {
                //        Value = dto.count_date
                //    });






                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    try
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
                //            {
                //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                {
                //                    dataRow.Add(
                //                        dataReader.GetName(iFiled),
                //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                    );
                //                    var datatype = dataReader.GetFieldType(iFiled);
                //                    if (datatype.Name == "DateTime")
                //                    {
                //                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                //                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                //                    }
                //                    else
                //                    {
                //                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                //                    }
                //                }
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        return ex.Message;
                //    }
                //}

                // val.Add("[MSG]", message1);

                //for (int j = 0; j < ParamaetersName.Count; j++)
                //{
                //    for (int p = 0; p < val.Count; p++)
                //    {
                //        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                //        {
                //            result = Mailmsg.Replace("[MSG]", message1);
                //            Mailmsg = result;
                //        }
                //    }
                //}
                result = Mailmsg.Replace("[MSG]", message1);
                Mailmsg = result;

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _ATContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(dto.MI_Id)).ToList();
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
                    smstpdetails = _ATContext.GeneralConfigDMO.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                    string emailccdetails = "";
                    string emailbccdetails = "";

                    if (template[0].ISES_MailCCId != null || template[0].ISES_MailCCId != "")
                    {
                        emailccdetails = template[0].ISES_MailCCId;
                        //emailbccdetails = emaildetails.FirstOrDefault().ISM_Email_BCC;
                    }
                    else
                    {
                        emailccdetails = "";
                        emailbccdetails = "";
                    }

                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }
                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(dto.HRME_EmailId);



                        if (emailccdetails != null && emailccdetails != "")
                        {
                            string[] ccmaildetails = emailccdetails.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                if (c != dto.HRME_EmailId)
                                {
                                    message.AddCc(c);
                                }

                            }
                        }

                        if (emailbccdetails != null && emailbccdetails != "")
                        {
                            string[] bccmaildetails = emailbccdetails.Split(',');

                            foreach (var bc in bccmaildetails)
                            {
                                if (bc != dto.HRME_EmailId)
                                {
                                    message.AddBcc(bc);
                                }
                            }
                        }


                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        client.SendEmailAsync(message).Wait();




                        //using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                        //{
                        //    var template1010 = _vmsconte.SMSEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template).Select(d => d.IVRMIM_Id).ToList();

                        //    var moduleid = _vmsconte.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        //    var modulename = _vmsconte.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        //    if (modulename.Count == 0)
                        //    {
                        //        modulename[0] = "Issue Manager";
                        //    }

                        //    cmd.CommandText = "IVRM_Email_Outgoing";
                        //    cmd.CommandType = CommandType.StoredProcedure;
                        //    cmd.Parameters.Add(new SqlParameter("@EmailId",
                        //        SqlDbType.NVarChar)
                        //    {
                        //        Value = Email
                        //    });
                        //    cmd.Parameters.Add(new SqlParameter("@Message",
                        //       SqlDbType.NVarChar)
                        //    {
                        //        Value = template
                        //    });
                        //    cmd.Parameters.Add(new SqlParameter("@module",
                        //    SqlDbType.VarChar)
                        //    {
                        //        Value = modulename[0]
                        //    });
                        //    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        //   SqlDbType.BigInt)
                        //    {
                        //        Value = MI_Id
                        //    });

                        //    if (cmd.Connection.State != ConnectionState.Open)
                        //        cmd.Connection.Open();

                        //    try
                        //    {
                        //        using (var dataReader = cmd.ExecuteReader())
                        //        {
                        //        }
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        return ex.Message;
                        //    }
                        //}
                    }
                    else
                    {

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
                                emailMessage.To.Add(new MailAddress(dto.HRME_EmailId));
                                emailMessage.From = new MailAddress(SendingEmail, institutionName[0].MI_Name);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;




                                if (emailccdetails != null && emailccdetails != "")
                                {
                                    string[] ccmaildetails = emailccdetails.Split(',');

                                    foreach (var c in ccmaildetails)
                                    {
                                        if (c != dto.HRME_EmailId)
                                        {
                                            emailMessage.CC.Add(c);
                                        }
                                    }
                                }

                                if (emailbccdetails != null && emailbccdetails != "")
                                {
                                    string[] bccmaildetails = emailbccdetails.Split(',');

                                    foreach (var bc in bccmaildetails)
                                    {
                                        if (bc != dto.HRME_EmailId)
                                        {
                                            emailMessage.Bcc.Add(bc);
                                        }
                                    }
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

        public AssetTagDTO mailDemo(AssetTagDTO dto)
        {
            try
            {
                sendmailDemo("TEST", dto.message, dto.HRME_EmailId);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return dto;

        }
        public string sendmailDemo(string Template, string message1,string HRME_EmailId)
        {
            try
            {
                string mailcc = "";
                string mailbcc = "";


                Dictionary<string, string> val = new Dictionary<string, string>();
                //var template = _ATContext.SMSEmailSetting.Where(e => e.MI_Id == dto.MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                //if (template.Count == 0)
                //{
                //    return "";
                //}
                //var institutionName = _ATContext.Institution.Where(i => i.MI_Id == dto.MI_Id).ToList();
                //var Paramaeters = _ATContext.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == dto.MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                //var ParamaetersName = _ATContext.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailcontent = "DEMO MAIL";
                string Mailmsg = "DEMO MAIL";
                //string Resultsms = Mailcontent;
                //string result = Mailmsg;

                string result = "";
                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
               
                result = Mailmsg.Replace("[MSG]", message1);
                Mailmsg = result;

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                //alldetails = _ATContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(dto.MI_Id)).ToList();
                //string Attechement = "";
                //if (alldetails.Count > 0)
                //{

                    string SendingEmail = "radha@vapstech.com";
                    //string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    //string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    //string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                   // Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = "DEMO SUBJECT";
                    //string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    //List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    //smstpdetails = _ATContext.GeneralConfigDMO.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                    string emailccdetails = "";
                    string emailbccdetails = "";

                    //if (template[0].ISES_MailCCId != null || template[0].ISES_MailCCId != "")
                    //{
                    //    emailccdetails = template[0].ISES_MailCCId;
                    //    //emailbccdetails = emaildetails.FirstOrDefault().ISM_Email_BCC;
                    //}
                    //else
                    //{
                    //    emailccdetails = "";
                    //    emailbccdetails = "";
                    //}

                

                    //if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    //{
                    //    Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    //}
                  

                    //    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    //    {
                    //        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    //    }
                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = "demosmtp",
                                Password = "India@1234@9876"
                            };
                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = "216.117.186.61";
                            clientsmtp.Port = 2771;
                            clientsmtp.EnableSsl = false;
                            using (var emailMessage = new MailMessage())
                            {
                                emailMessage.To.Add(new MailAddress(HRME_EmailId));
                                emailMessage.From = new MailAddress(SendingEmail, "DEMO");
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;




                        //if (emailccdetails != null && emailccdetails != "")
                        //{
                        //    string[] ccmaildetails = emailccdetails.Split(',');

                        //    foreach (var c in ccmaildetails)
                        //    {
                        //        if (c != dto.HRME_EmailId)
                        //        {
                        //            emailMessage.CC.Add(c);
                        //        }
                        //    }
                        //}

                        //if (emailbccdetails != null && emailbccdetails != "")
                        //{
                        //    string[] bccmaildetails = emailbccdetails.Split(',');

                        //    foreach (var bc in bccmaildetails)
                        //    {
                        //        if (bc != dto.HRME_EmailId)
                        //        {
                        //            emailMessage.Bcc.Add(bc);
                        //        }
                        //    }
                        //}

                        emailMessage.CC.Add("shainesh@digiglitz.com");
                        emailMessage.CC.Add("ehsan@digiglitz.com");
                        clientsmtp.Send(emailMessage);



                            }
                        }
                   

                //}
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

